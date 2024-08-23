using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections;
using UnityEngine.UI;
using TMPro;  // Se estiver usando TextMeshPro

[CreateAssetMenu(menuName = "Tiles/Plant Tile")]
public class PlantTile : Tile
{
    public Sprite[] growthSprites;  // Array para armazenar os sprites de cada fase de crescimento
    public float[] growthTimes;  // Array para armazenar o tempo necessário para cada fase de crescimento
    public GameObject progressBarPrefab;  // Prefab da barra de progresso
    private GameObject progressBarInstance;  // Instância da barra de progresso
    private Image progressBarFill;  // Referência ao preenchimento da barra de progresso
    private TextMeshProUGUI progressBarText;  // Referência ao texto dentro da barra (TextMeshProUGUI) ou Text se não usar TMP

    public bool isPlanted = false;  // Indica se a planta foi plantada
    public bool isFullyGrown = false;  // Indica se a planta atingiu o estágio final de crescimento

    private int growthStage = 0;  // Fase de crescimento atual
    private float totalGrowthTime;
    private float currentGrowthTime;

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
            Transform fillTransform = progressBarInstance.transform.Find("ProgressBarFill");

            if (fillTransform != null)
            {
                progressBarFill = fillTransform.GetComponent<Image>();
                progressBarFill.fillAmount = 0f;

                // Encontra o texto dentro da barra de progresso
                progressBarText = fillTransform.GetComponentInChildren<TextMeshProUGUI>();
                if (progressBarText != null)
                {
                    progressBarText.text = $"{Mathf.CeilToInt(totalGrowthTime)}s";  // Define o texto inicial
                }
                else
                {
                    Debug.LogError("ProgressBarText not found in the prefab.");
                }
            }
            else
            {
                Debug.LogError("ProgressBarFill not found in the prefab.");
            }
        }

        UpdateSprite(tilemap, position);
        caller.StartCoroutine(Grow(tilemap, position));  // Inicia a corrotina de crescimento
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
            }

            growthStage++;
            UpdateSprite(tilemap, position);
            tilemap.RefreshTile(position);
        }

        isFullyGrown = true;

        if (progressBarInstance != null)
        {
            Destroy(progressBarInstance);
        }
    }

    private IEnumerator UpdateProgressBar()
    {
        while (!isFullyGrown)
        {
            yield return null;

            if (progressBarFill != null)
            {
                float progress = currentGrowthTime / totalGrowthTime;
                progressBarFill.fillAmount = progress;

                if (progressBarText != null)
                {
                    int secondsRemaining = Mathf.CeilToInt(totalGrowthTime - currentGrowthTime);
                    progressBarText.text = $"{secondsRemaining}s";
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
    public void Collect(Tilemap tilemap, Vector3Int position)
    {
        if (isFullyGrown)
        {
            tilemap.SetTile(position, null);  // Remove o tile (ou substitui por outro tile, se necessário)
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
