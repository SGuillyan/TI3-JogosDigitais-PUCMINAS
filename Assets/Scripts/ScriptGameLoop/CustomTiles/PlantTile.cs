using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections;
using UnityEngine.UI;
using TMPro;

[CreateAssetMenu(menuName = "Tiles/Plant Tile2")]
public class PlantTile : Tile
{
    [Header("Growth Settings")]
    [SerializeField] private GameObject[] growthPrefabs;  // Prefabs para cada fase de crescimento da planta
    [SerializeField] private GameObject rotPrefab;
    [SerializeField] private float[] growthTimes;  // Array para armazenar o tempo necessário para cada fase de crescimento

    public GameObject[] GrowthPrefabs => growthPrefabs;
    public GameObject RotPrefab => rotPrefab;
    public float[] GrowthTimes => growthTimes;

    [Header("Ambient")]
    [SerializeField] private Ambient.Temperature idealTemperature;  // Temperatura ideal para o crescimento da planta
    [SerializeField] private Ambient.Climate idealClimate;          // Clima ideal para o crescimento da planta
    [SerializeField] [Range(0, 2)] private int greenTolerance = 0;  // Tolerância máxima para o buff de crescimento
    [SerializeField] [Range(1, 4)] private int yellowTolerance = 2; // Tolerância máxima para crescimento normal

    public Ambient.Temperature IdealTemperature => idealTemperature;
    public Ambient.Climate IdealClimate => idealClimate;
    public int GreenTolerance => greenTolerance;
    public int YellowTolerance => yellowTolerance;

    [Header("Progress Bar")]
    [SerializeField] private GameObject progressBarPrefab;
    [SerializeField] private GameObject plowedTilePrefab;

    public GameObject ProgressBarPrefab => progressBarPrefab;
    public GameObject PlowedTilePrefab => plowedTilePrefab;

    [Header("Harvest Settings")]
    [SerializeField] public Item harvestedItem;
    [SerializeField] private TileBase soilTile;
    [SerializeField] private int requiredNitrogen = 10;
    [SerializeField] private int requiredPhosphorus = 5;
    [SerializeField] private int requiredPotassium = 5;
    [SerializeField] private int returnNitrogen = 5;
    [SerializeField] private int returnPhosphorus = 3;
    [SerializeField] private int returnPotassium = 4;

    public Item HarvestedItem => harvestedItem;
    public TileBase SoilTile => soilTile;
    public int RequiredNitrogen => requiredNitrogen;
    public int RequiredPhosphorus => requiredPhosphorus;
    public int RequiredPotassium => requiredPotassium;
    public int ReturnNitrogen => returnNitrogen;
    public int ReturnPhosphorus => returnPhosphorus;
    public int ReturnPotassium => returnPotassium;

    // Private fields to store runtime data (not serialized)
    private GameObject currentGrowthInstance;  // Instância atual do estágio de crescimento
    private bool isPlanted = false;
    private bool isFullyGrown = false;
    private bool isRotten = false;
    private bool isCollected = false;
    private int growthStage = 0;
    private float totalGrowthTime;
    private float currentGrowthTime;
    private GameObject progressBarInstance; // Instância da barra de progresso
    private Image progressBarFill;          // Referência ao preenchimento da barra de progresso
    private TMP_Text progressBarText;       // Referência ao texto da barra de progresso

    public bool IsPlanted => isPlanted;
    public bool IsFullyGrown => isFullyGrown;
    public bool IsRotten => isRotten;
    public bool IsCollected => isCollected;
    public int GrowthStage => growthStage;
    public float TotalGrowthTime => totalGrowthTime;
    public float CurrentGrowthTime => currentGrowthTime;

    public void RecreatePlantInstance(Tilemap tilemap, Vector3Int position, string tileType, string instantiatedObjectName)
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
            Debug.LogWarning($"TileInfo não encontrado na posição {position}. Não é possível recriar a planta.");
            return;
        }

        string groundObjectName = $"Ground_{position.x}_{position.y}_{position.z}";
        GameObject groundObject = GameObject.Find(groundObjectName);
        if (groundObject != null)
        {
            // Transformar o Ground em um PlowedTile
            Destroy(groundObject); // Remove o objeto Ground
            Vector3 worldPosition = tilemap.CellToWorld(position) + new Vector3(0.5f, 0, 0.5f);
            GameObject plowedTile = Instantiate(plowedTilePrefab, worldPosition, Quaternion.identity, tilemap.transform);
            plowedTile.name = $"PlowedTile_{position.x}_{position.y}_{position.z}";

            // Atualizar o dicionário de objetos instanciados
            // tilemapManager.SetInstantiatedTile(position, plowedTile);

            //Debug.Log($"Ground transformado em PlowedTile na posição {position}");
        }
        else
        {
            Debug.LogWarning($"Ground não encontrado na posição {position}. Continuando sem transformá-lo.");
        }




        // Extrair o estágio de crescimento do nome do objeto
        int savedGrowthStage = ParseGrowthStageFromName(instantiatedObjectName);

    // Recriar a instância da planta com base no prefab correto
        if (growthPrefabs != null && savedGrowthStage < growthPrefabs.Length)
        {
            GameObject newPlantInstance = Instantiate(growthPrefabs[savedGrowthStage],
                tilemap.CellToWorld(position) + new Vector3(0.5f, 0, 0.5f),
                Quaternion.identity,
                tilemap.transform);
            newPlantInstance.name = $"{tileType}_Stage{savedGrowthStage}_{position.x}_{position.y}_{position.z}";
            tilemapManager.SetInstantiatedTile(position, newPlantInstance);
            ResumeGrowth(tilemap, position, savedGrowthStage, 0, tilemapManager);
        }
    }

    private int ParseGrowthStageFromName(string instantiatedObjectName)
    {
        if (!string.IsNullOrEmpty(instantiatedObjectName) && instantiatedObjectName.Contains("Stage"))
        {
            string[] parts = instantiatedObjectName.Split('_');
            foreach (string part in parts)
            {
                if (part.StartsWith("Stage") && int.TryParse(part.Replace("Stage", ""), out int stage))
                {
                    return stage;
                }
            }
        }
        return 0; // Retorna 0 se o estágio não puder ser determinado
    }

    public void ResumeGrowth(Tilemap tilemap, Vector3Int position, int savedGrowthStage, float savedGrowthTime, MonoBehaviour caller)
    {
        TilemapManager tilemapManager = Object.FindObjectOfType<TilemapManager>();

        if (tilemapManager == null)
        {
            Debug.LogError("TilemapManager não encontrado!");
            return;
        }

        if (savedGrowthStage >= growthPrefabs.Length)
        {
            Debug.LogWarning($"GrowthStage inválido ({savedGrowthStage}) para a planta na posição {position}. Definindo como totalmente crescida.");
            isFullyGrown = true;
            UpdateGrowthInstance(tilemap, position, growthPrefabs[growthPrefabs.Length - 1]);
            return;
        }

        // Configura os valores salvos
        growthStage = savedGrowthStage;
        currentGrowthTime = savedGrowthTime;
        isPlanted = true;
        isFullyGrown = growthStage == growthPrefabs.Length - 1;
        isRotten = false; // Reset apodrecimento ao carregar

        // Atualiza a instância da planta para o estágio salvo
        UpdateGrowthInstance(tilemap, position, growthPrefabs[growthStage]);

        // Se não estiver completamente crescida, retoma o crescimento
        if (!isFullyGrown)
        {
            caller.StartCoroutine(GrowFromStage(tilemap, position, caller));
        }

        Debug.Log($"Crescimento retomado na posição {position}. Estágio: {growthStage}, Tempo: {currentGrowthTime}s.");
    }

    private IEnumerator GrowFromStage(Tilemap tilemap, Vector3Int position, MonoBehaviour caller)
    {
        float totalElapsedTime = currentGrowthTime;

        while (growthStage < growthTimes.Length)
        {
            float stageGrowthTime = growthTimes[growthStage];

            while (totalElapsedTime < stageGrowthTime)
            {
                yield return null;

                float additionalTime = Time.deltaTime * VerifyAmbient();
                totalElapsedTime += additionalTime;
                currentGrowthTime += additionalTime;

                UpdateProgressBarUI(totalElapsedTime);
            }

            growthStage++;

            // Atualiza o prefab do estágio atual
            if (growthStage < growthPrefabs.Length)
            {
                UpdateGrowthInstance(tilemap, position, growthPrefabs[growthStage]);
            }

            tilemap.RefreshTile(position);

            if (growthStage == growthPrefabs.Length - 1)
            {
                isFullyGrown = true;

                // Após o crescimento completo, inicia o apodrecimento
                caller.StartCoroutine(Rot(tilemap, position));
                yield break;
            }
        }
    }


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
        // InitializeProgressBar(tilemap, position);

        UpdateGrowthInstance(tilemap, position, growthPrefabs[0]);
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

        // Itera sobre os estágios de crescimento
        while (growthStage < growthTimes.Length)
        {
            float stageGrowthTime = growthTimes[growthStage];
            float stageStartTime = totalElapsedTime;

            // Cresce a planta enquanto o tempo não passar para o próximo estágio
            while (totalElapsedTime < stageStartTime + stageGrowthTime)
            {
                yield return null;
                float additionalTime = Time.deltaTime * VerifyAmbient();
                totalElapsedTime += additionalTime;
                currentGrowthTime += additionalTime;

                // Atualiza a barra de progresso corretamente
                UpdateProgressBarUI(totalElapsedTime);
            }

            // Passa para o próximo estágio de crescimento
            growthStage++;

            // Atualiza o prefab do estágio atual
            GameObject growthPrefab = (growthStage < growthPrefabs.Length) ? growthPrefabs[growthStage] : null;
            UpdateGrowthInstance(tilemap, position, growthPrefab);

            tilemap.RefreshTile(position);

            // Verifica se a planta está completamente crescida
            if (growthStage == growthTimes.Length - 1)
            {
                isFullyGrown = true;

                // Após o crescimento completo, chama o método de apodrecimento
                caller.StartCoroutine(Rot(tilemap, position));
                yield break; // Interrompe a execução do método Grow, já que o apodrecimento foi iniciado
            }
        }
    }

    private IEnumerator Rot(Tilemap tilemap, Vector3Int position)
    {
        // Soma o tempo total de crescimento
        float totalGrowthTime = 0f;
        foreach (float time in growthTimes)
        {
            totalGrowthTime += time;
        }

        // Define o tempo de apodrecimento (após o crescimento completo)
        float timeToRot = totalGrowthTime;

        // Aguarda o tempo de apodrecimento após o crescimento completo
        while (timeToRot > 0)
        {
            if (isCollected)
            {
                // Interrompe a rotina de apodrecimento se a planta foi colhida
                yield break;
            }

            yield return null;
            timeToRot -= Time.deltaTime;  // Reduz o tempo restante até apodrecer
        }

        // Se o tempo de apodrecimento passar, a planta apodrece
        if (!isCollected && isFullyGrown)
        {
            isRotten = true;
            // Atualiza o prefab para o rotPrefab (em vez de destruir)
            UpdateGrowthInstance(tilemap, position, rotPrefab);  // Muda para o prefab de apodrecimento
        }

        // Opcional: Se houver uma barra de progresso, destruí-la após o apodrecimento
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
    private void UpdateGrowthInstance(Tilemap tilemap, Vector3Int position, GameObject growthPrefab)
    {
        // Destruir a instância anterior, se existir
        if (currentGrowthInstance != null)
        {
            Destroy(currentGrowthInstance);
        }

        // Instanciar o prefab correspondente ao estágio de crescimento
        if (growthPrefab != null)
        {
            Vector3 worldPos = tilemap.CellToWorld(position) + new Vector3(0.5f, 0, 0.5f); // Ajuste a posição do prefab
            currentGrowthInstance = Instantiate(growthPrefab, worldPos, Quaternion.identity);

            // Atualiza o nome do objeto para refletir o tipo de planta, estágio de crescimento e posição
            string plantName = harvestedItem != null ? harvestedItem.itemName : "UnknownPlant";
            currentGrowthInstance.name = $"{plantName}_Stage{growthStage}_{position.x}_{position.y}_{position.z}";

            // Atualiza o dicionário do TilemapManager com a nova instância
            UpdateInstantiatedTileDictionary(position, currentGrowthInstance);
        }
    }




    private void UpdateInstantiatedTileDictionary(Vector3Int position, GameObject growthInstance)
    {
        TilemapManager tilemapManager = Object.FindObjectOfType<TilemapManager>();

        if (tilemapManager == null)
        {
            Debug.LogError("TilemapManager não encontrado!");
            return;
        }

        // Atualiza o objeto instanciado no dicionário
        if (growthInstance != null)
        {
            tilemapManager.SetInstantiatedTile(position, growthInstance);
            Debug.Log($"Objeto atualizado no dicionário para {growthInstance.name} na posição {position}");
        }
        else
        {
            Debug.LogWarning($"Nenhuma instância de crescimento encontrada para a posição {position}");
        }
    }



   public void Collect(Tilemap tilemap, Vector3Int position, Inventory playerInventory)
    {
        if (isFullyGrown && !isRotten)
        {
            isCollected = true; // Sinaliza que a planta foi colhida

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
        else if (isRotten)
        {
            // Mesma lógica para plantas apodrecidas
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

        if (greenTolerance > 0 && Mathf.Abs((int)idealTemperature - (int)Ambient.GetCurrentTemperature()) <= greenTolerance - 1)
        {
            r *= 1.5f;
        }
        else if (Mathf.Abs((int)idealTemperature - (int)Ambient.GetCurrentTemperature()) >= yellowTolerance - 1)
        {
            r *= 0.5f;
        }

        if (greenTolerance > 0 && Mathf.Abs((int)idealClimate - (int)Ambient.GetCurrentClimate()) <= greenTolerance - 1)
        {
            r *= 1.5f;
        }
        else if (Mathf.Abs((int)idealClimate - (int)Ambient.GetCurrentClimate()) >= yellowTolerance - 1)
        {
            r *= 0.5f;
        }

        return r;
    }
}
