using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections;
using UnityEngine.UI;
using TMPro;

[CreateAssetMenu(menuName = "Tiles/Plant Tile2")]
public class PlantTile : Tile
{
    public GameObject[] growthPrefabs;  // Prefabs para cada fase de crescimento da planta
    public float[] growthTimes;  // Array para armazenar o tempo necessário para cada fase de crescimento

    [Header("Ambient")]
    [Tooltip("Ambiente em que a planta está")]
    [SerializeField] private Ambient ambient;
    [Tooltip("Temperatura ideal para o crescimento da planta")]
    [SerializeField] private Ambient.Temperature idealTemperature;
    [Tooltip("Clima ideal para o crescimento da planta")]
    [SerializeField] private Ambient.Climate idealClimate;
    [Tooltip("Tolerância máxima para o buff de crescimento, deve ser menor que a 'yellowTolerance'")]
    [Range(0, 2)]
    [SerializeField] private int greenTolerance = 0;
    [Tooltip("Tolerância máxima para o crescimento normal, deve ser maior que a 'greenTolerance'")]
    [Range(1, 4)]
    [SerializeField] private int yellowTolerance = 2;

    [Space(5)]

    [Header("Progress Bar")]
    public GameObject progressBarPrefab;  // Prefab da barra de progresso
    private GameObject progressBarInstance;  // Instância da barra de progresso
    private Image progressBarFill;  // Referência ao preenchimento da barra de progresso
    private TMP_Text progressBarText;  // Referência ao texto da barra de progresso

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

    // Valores de NPK que a planta devolve ao solo após ser colhida
    public int returnNitrogen = 5;
    public int returnPhosphorus = 3;
    public int returnPotassium = 4;

    private GameObject currentGrowthInstance; // Instância atual do estágio de crescimento

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

        UpdateGrowthInstance(tilemap, position);
        caller.StartCoroutine(Grow(tilemap, position, caller));
    }

    // Inicializa a barra de progresso da planta
    private void InitializeProgressBar(Tilemap tilemap, Vector3Int position)
    {
        if (progressBarPrefab != null)
        {
            Vector3 worldPos = tilemap.CellToWorld(position) + new Vector3(0.5f, 2.5f, 0);
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
                float additionalTime = Time.deltaTime * VerifyAmbient();
                totalElapsedTime += additionalTime;
                currentGrowthTime += additionalTime;

                // Atualiza a barra de progresso corretamente
                UpdateProgressBarUI(totalElapsedTime);
            }

            growthStage++;
            UpdateGrowthInstance(tilemap, position);
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

    // Substitui a lógica de sprites por instâncias de GameObjects para cada estágio de crescimento
    private void UpdateGrowthInstance(Tilemap tilemap, Vector3Int position)
    {
        // Destruir a instância anterior, se existir
        if (currentGrowthInstance != null)
        {
            Destroy(currentGrowthInstance);
        }

        // Instancia o prefab correspondente ao estágio de crescimento
        if (growthPrefabs != null && growthStage < growthPrefabs.Length)
        {
            Vector3 worldPos = tilemap.CellToWorld(position) + new Vector3(0.5f, 0, 0.5f);  // Ajuste a posição do prefab
            currentGrowthInstance = Instantiate(growthPrefabs[growthStage], worldPos, Quaternion.identity);
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

            // Obtém o estado atual dos nutrientes do solo antes de remover o PlantTile
            TileInfo currentTileInfo = tilemapManager.GetTileInfo(position);

            if (currentTileInfo == null)
            {
                Debug.LogError("TileInfo não encontrado para a posição: " + position);
                return;
            }

            // Adiciona o item coletado ao inventário do jogador
            playerInventory.AddItem(harvestedItem, 1);

            // Devolve os nutrientes ao solo após a colheita
            currentTileInfo.nitrogen += returnNitrogen;
            currentTileInfo.phosphorus += returnPhosphorus;
            currentTileInfo.potassium += returnPotassium;

            // Atualiza o estado para plantável após a colheita
            currentTileInfo.isPlantable = true;

            // Restaura o tile de solo
            tilemap.SetTile(position, soilTile);
            tilemapManager.SetTileInfo(position, currentTileInfo);

            // Destroi a instância da planta
            if (currentGrowthInstance != null)
            {
                Destroy(currentGrowthInstance);
            }
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

        if (currentGrowthInstance != null)
        {
            Destroy(currentGrowthInstance);
        }
    }

    public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
    {
        tileData.color = Color.white;
    }

    private float VerifyAmbient()
    {
        float r = 1;

        if (greenTolerance > 0 && Mathf.Abs((int)idealTemperature - (int)ambient.currentTemperature) <= greenTolerance - 1)
        {
            r *= 1.5f;
        }
        else if (Mathf.Abs((int)idealTemperature - (int)ambient.currentTemperature) >= yellowTolerance - 1)
        {
            r *= 0.5f;
        }

        if (greenTolerance > 0 && Mathf.Abs((int)idealClimate - (int)ambient.currentClimate) <= greenTolerance - 1)
        {
            r *= 1.5f;
        }
        else if (Mathf.Abs((int)idealClimate - (int)ambient.currentClimate) >= yellowTolerance - 1)
        {
            r *= 0.5f;
        }

        return r;
    }
}
