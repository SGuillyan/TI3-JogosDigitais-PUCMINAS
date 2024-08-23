using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections;
using UnityEngine.UI;
using TMPro;

[CreateAssetMenu(menuName = "Tiles/Plant Tile")]
public class PlantTile : Tile
{
    public Sprite[] growthSprites;  // Array para armazenar os sprites de cada fase de crescimento
    public float[] growthTimes;  // Array para armazenar o tempo necessário para cada fase de crescimento
    public GameObject progressBarPrefab;  // Prefab da barra de progresso
    private GameObject progressBarInstance;  // Instância da barra de progresso
    private Image progressBarFill;  // Referência ao preenchimento da barra de progresso
    private TMP_Text progressBarText;  // Referência ao texto da barra de progresso

    public bool isPlanted = false;  // Indica se a planta foi plantada
    public bool isFullyGrown = false;  // Indica se a planta atingiu o estágio final de crescimento

    public Item harvestedItem;  // Item a ser adicionado ao inventário ao coletar
    public TileBase soilTile;  // Tile original para ser restaurado após a colheita

    private int growthStage = 0;  // Fase de crescimento atual
    private float totalGrowthTime;
    private float currentGrowthTime;

    // Método para iniciar o crescimento ao plantar a semente
    public void Plant(Tilemap tilemap, Vector3Int position, MonoBehaviour caller)
    {
        isPlanted = true;
        isFullyGrown = false;
        growthStage = 0;
        totalGrowthTime = 0f;
        currentGrowthTime = 0f;

        // Calcula o tempo total de crescimento
        foreach (var time in growthTimes)
        {
            totalGrowthTime += time;
        }

        // Instancia a barra de progresso
        if (progressBarPrefab != null)
        {
            Vector3 worldPos = tilemap.CellToWorld(position) + new Vector3(0.5f, 1.5f, 0); // Ajusta a posição da barra de progresso
            progressBarInstance = Instantiate(progressBarPrefab, worldPos, Quaternion.identity);

            // Encontra a imagem de preenchimento da barra de progresso
            progressBarFill = progressBarInstance.GetComponentInChildren<Image>();

            if (progressBarFill != null)
            {
                progressBarFill.fillAmount = 0f;
            }
            else
            {
                Debug.LogError("ProgressBarFill not found in the prefab.");
            }

            // Encontra o texto da barra de progresso usando GetComponentInChildren
            progressBarText = progressBarInstance.GetComponentInChildren<TMP_Text>();

            if (progressBarText != null)
            {
                progressBarText.text = Mathf.CeilToInt(totalGrowthTime).ToString() + "s";

            }
            else
            {
                Debug.LogError("ProgressBarText not found in the prefab.");
            }
        }

        UpdateSprite(tilemap, position);
        caller.StartCoroutine(Grow(tilemap, position));  // Inicia a corrotina de crescimento através de um MonoBehaviour
        caller.StartCoroutine(UpdateProgressBar());  // Inicia a corrotina para atualizar a barra de progresso
    }

    private IEnumerator Grow(Tilemap tilemap, Vector3Int position)
    {
        float totalElapsedTime = 0f;

        while (growthStage < growthTimes.Length)
        {
            float stageGrowthTime = growthTimes[growthStage];
            float stageStartTime = totalElapsedTime;

            while (totalElapsedTime < stageStartTime + stageGrowthTime)
            {
                yield return null;
                totalElapsedTime += Time.deltaTime;
                currentGrowthTime += Time.deltaTime;

                // Atualiza a barra de progresso com base no tempo total
                if (progressBarFill != null)
                {
                    float progress = totalElapsedTime / totalGrowthTime;
                    progressBarFill.fillAmount = progress;

                    // Atualiza o texto com o tempo restante
                    if (progressBarText != null)
                    {
                        float timeRemaining = totalGrowthTime - totalElapsedTime;
                        progressBarText.text = Mathf.CeilToInt(timeRemaining).ToString();
                    }
                }
            }

            // Avança para o próximo estágio de crescimento
            growthStage++;
            UpdateSprite(tilemap, position);
            tilemap.RefreshTile(position);
        }

        // Quando o crescimento estiver completo
        isFullyGrown = true;

        // Remove a barra de progresso
        if (progressBarInstance != null)
        {
            Destroy(progressBarInstance);
        }
    }

    // Corrotina separada para atualizar a barra de progresso
    private IEnumerator UpdateProgressBar()
    {
        while (!isFullyGrown)  // Continua até que a planta esteja completamente crescida
        {
            yield return null;  // Espera um frame
            if (progressBarFill != null)
            {
                float progress = currentGrowthTime / totalGrowthTime;
                progressBarFill.fillAmount = progress;

                // Atualiza o texto com o tempo restante
                if (progressBarText != null)
                {
                    float timeRemaining = totalGrowthTime - currentGrowthTime;
                    progressBarText.text = Mathf.CeilToInt(timeRemaining).ToString();
                }
            }
        }
    }

    // Método para atualizar o sprite conforme a fase de crescimento
    public void UpdateSprite(Tilemap tilemap, Vector3Int position)
    {
        if (growthSprites != null && growthSprites.Length > 0)
        {
            this.sprite = growthSprites[growthStage];
        }
    }

    // Método para coletar a planta quando estiver completamente crescida
    public void Collect(Tilemap tilemap, Vector3Int position, Inventory playerInventory)
    {
        if (isFullyGrown)
        {
            // Adiciona o item correspondente ao inventário do jogador
            playerInventory.AddItem(harvestedItem, 1);

            // Restaura o tile original de solo
            tilemap.SetTile(position, soilTile);
        }
    }

    // Método para resetar a planta
    public void ResetGrowth()
    {
        isPlanted = false;
        isFullyGrown = false;
        growthStage = 0;
        currentGrowthTime = 0f;

        // Remove a barra de progresso, se ainda existir
        if (progressBarInstance != null)
        {
            Destroy(progressBarInstance);
        }
    }

    public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
    {
        tileData.sprite = this.sprite;
        tileData.color = Color.white;
    }
}
