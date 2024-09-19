using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections;
using UnityEngine.UI;
using TMPro;

[CreateAssetMenu(menuName = "Tiles/Plant Tile")]
public class PlantTile : Tile
{
    public Sprite[] growthSprites;
    public float[] growthTimes;
    public GameObject progressBarPrefab;
    private GameObject progressBarInstance;
    private Image progressBarFill;
    private TMP_Text progressBarText;

    public bool isPlanted = false;
    public bool isFullyGrown = false;

    public Item harvestedItem;
    public TileBase soilTile;

    private int growthStage = 0;
    private float totalGrowthTime;
    private float currentGrowthTime;

    // Requisitos de Nutrientes NPK para crescer
    public int requiredNitrogen = 10;
    public int requiredPhosphorus = 5;
    public int requiredPotassium = 5;

    public void Plant(Tilemap tilemap, Vector3Int position, MonoBehaviour caller)
    {
        TilemapManager tilemapManager = Object.FindObjectOfType<TilemapManager>();

        if (tilemapManager == null)
        {
            Debug.LogError("TilemapManager não encontrado!");
            return;
        }

        TileInfo tileInfo = tilemapManager.GetTileInfo(position);

        if (tileInfo == null)
        {
            Debug.LogError("TileInfo não encontrado para a posição: " + position);
            return;
        }

        // Verifica se há nutrientes suficientes
        if (!HasEnoughNutrients(tileInfo))
        {
            Debug.LogWarning("Nutrientes insuficientes para plantar!");
            return; // Interrompe o plantio
        }

        // Consome os nutrientes
        ConsumeNutrients(tileInfo, tilemapManager, position);

        // Inicia o plantio e crescimento
        StartPlanting(tilemap, position, caller);
    }

// Verifica se há nutrientes suficientes no momento do plantio
    private bool HasEnoughNutrients(TileInfo tileInfo)
    {
        return tileInfo.nitrogen >= requiredNitrogen 
            && tileInfo.phosphorus >= requiredPhosphorus 
            && tileInfo.potassium >= requiredPotassium;
    }

// Consome os nutrientes do solo
    private void ConsumeNutrients(TileInfo tileInfo, TilemapManager tilemapManager, Vector3Int position)
    {
        tileInfo.nitrogen -= requiredNitrogen;
        tileInfo.phosphorus -= requiredPhosphorus;
        tileInfo.potassium -= requiredPotassium;
        tileInfo.isPlantable = false; // Atualiza o estado para não plantável

        // Atualiza o dicionário no TilemapManager
        tilemapManager.SetTileInfo(position, tileInfo);
    }

// Inicia o processo de plantio e crescimento da planta
    private void StartPlanting(Tilemap tilemap, Vector3Int position, MonoBehaviour caller)
    {
        isPlanted = true;
        isFullyGrown = false;
        growthStage = 0;
        totalGrowthTime = 0f;
        currentGrowthTime = 0f;

        foreach (var time in growthTimes)
        {
            totalGrowthTime += time;
        }

        // Inicializa a barra de progresso
        InitializeProgressBar(tilemap, position);

        UpdateSprite(tilemap, position);
        caller.StartCoroutine(Grow(tilemap, position, caller));
    }

// Inicializa a barra de progresso da planta
    private void InitializeProgressBar(Tilemap tilemap, Vector3Int position)
    {
        if (progressBarPrefab != null)
        {
            Vector3 worldPos = tilemap.CellToWorld(position) + new Vector3(0.5f, 1.5f, 0);
            progressBarInstance = Instantiate(progressBarPrefab, worldPos, Quaternion.identity);

            Transform fillTransform = progressBarInstance.transform.Find("ProgressBarFill"); // Substitua "ProgressBarFill" pelo nome correto
            if (fillTransform != null)
            {
                progressBarFill = fillTransform.GetComponent<Image>();
                if (progressBarFill != null)
                {
                    progressBarFill.fillAmount = 0f;
                }

                progressBarText = progressBarInstance.GetComponentInChildren<TMP_Text>();
                if (progressBarText != null)
                {
                    progressBarText.text = Mathf.CeilToInt(totalGrowthTime).ToString() + "s";
                }
            }
            else
            {
                Debug.LogError("Objeto ProgressBarFill não encontrado no prefab.");
            }
        }
    }


    private IEnumerator Grow(Tilemap tilemap, Vector3Int position, MonoBehaviour caller)
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

                // Atualiza a barra de progresso corretamente
                UpdateProgressBarUI(totalElapsedTime);
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

    private void UpdateProgressBarUI(float totalElapsedTime)
    {
        if (progressBarFill != null)
        {
            float progress = currentGrowthTime / totalGrowthTime;
            progressBarFill.fillAmount = Mathf.Clamp01(progress);

            if (progressBarText != null)
            {
                float timeRemaining = totalGrowthTime - totalElapsedTime;
                progressBarText.text = Mathf.CeilToInt(timeRemaining).ToString() + "s";  // Adiciona "s" à string de segundos
            }
        }
    }

    public void UpdateSprite(Tilemap tilemap, Vector3Int position)
    {
        if (growthSprites != null && growthSprites.Length > 0)
        {
            this.sprite = growthSprites[growthStage];
        }
    }

    public void Collect(Tilemap tilemap, Vector3Int position, Inventory playerInventory)
    {
        if (isFullyGrown)
        {
            TilemapManager tilemapManager = Object.FindObjectOfType<TilemapManager>();

            if (tilemapManager == null)
            {
                Debug.LogError("TilemapManager não encontrado!");
                return;
            }

            // Obtém o estado atual dos nutrientes do solo antes de remover o plantTile
            TileInfo currentTileInfo = tilemapManager.GetTileInfo(position);

            if (currentTileInfo == null)
            {
                Debug.LogError("TileInfo não encontrado para a posição: " + position);
                return;
            }

            // Adiciona o item coletado ao inventário do jogador
            playerInventory.AddItem(harvestedItem, 1);

            // Restaura o tile de solo
            tilemap.SetTile(position, soilTile);

            // Atualiza o estado para plantável
            currentTileInfo.isPlantable = true;

            // Mantém os nutrientes atuais no dicionário do TilemapManager
            tilemapManager.SetTileInfo(position, currentTileInfo);
        }
    }

    public void ResetGrowth()
    {
        isPlanted = false;
        isFullyGrown = false;
        growthStage = 0;
        currentGrowthTime = 0f;

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
