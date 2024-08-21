using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "Tiles/Plant Tile")]
public class PlantTile : Tile
{
    public Sprite[] growthSprites;  // Array para armazenar os sprites de cada fase de crescimento
    public float[] growthTimes;  // Array para armazenar o tempo necessário para cada fase de crescimento
    public GameObject progressBarPrefab;  // Prefab da barra de progresso
    private GameObject progressBarInstance;  // Instância da barra de progresso
    private Image progressBarFill;  // Referência ao preenchimento da barra de progresso

    public bool isPlanted = false;  // Indica se a planta foi plantada
    public bool isFullyGrown = false;  // Indica se a planta atingiu o estágio final de crescimento

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
            progressBarFill = progressBarInstance.GetComponentInChildren<Image>();

        if (progressBarFill != null)
        {
            progressBarFill.fillAmount = 0f;
        }
        else
        {
            Debug.LogError("ProgressBarFill not found using GetComponentInChildren.");
        }
        }

        UpdateSprite(tilemap, position);
        caller.StartCoroutine(Grow(tilemap, position));  // Inicia a corrotina de crescimento através de um MonoBehaviour
    }

    private IEnumerator Grow(Tilemap tilemap, Vector3Int position)
    {
        float totalElapsedTime = 0f;  // Tempo total que passou desde o início do crescimento

        while (growthStage < growthTimes.Length)
        {
            float stageGrowthTime = growthTimes[growthStage];  // Tempo necessário para o estágio atual
            float stageStartTime = totalElapsedTime;  // Tempo no qual o estágio atual começa

            while (totalElapsedTime < stageStartTime + stageGrowthTime)
            {
                yield return null;
                totalElapsedTime += Time.deltaTime;
                currentGrowthTime += Time.deltaTime;

                // Atualiza a barra de progresso com base no tempo total
                if (progressBarFill != null)
                {
                    float progress = totalElapsedTime / totalGrowthTime;
                    progressBarFill.fillAmount = progress;  // Atualiza o preenchimento da barra de progresso
                }
                else
                {
                    Debug.Log("Sem barra de progresso pra voce");
                }
            }

            // Avança para o próximo estágio de crescimento
            growthStage++;
            UpdateSprite(tilemap, position);
            tilemap.RefreshTile(position);  // Atualiza o tile no Tilemap
        }

        // Quando o crescimento estiver completo
        isFullyGrown = true;

        // Remove a barra de progresso
        if (progressBarInstance != null)
        {
            Destroy(progressBarInstance);
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
            // Aqui você pode implementar a lógica para coletar a planta
            // Exemplo: restaurar o tile original, adicionar itens ao inventário, etc.
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
