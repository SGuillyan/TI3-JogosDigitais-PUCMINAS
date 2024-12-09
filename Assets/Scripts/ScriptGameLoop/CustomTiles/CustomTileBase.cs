using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "New Custom Tile", menuName = "Tiles/Custom Tile")]
public class CustomTileBase : TileBase
{
    // Propriedades personalizadas
    [SerializeField] private Sprite sprite;
    [SerializeField] private GameObject customTilePrefab;  // Prefab do GameObject associado ao tile
    [SerializeField] private GameObject plowedTilePrefab;  // Prefab do GameObject associado ao solo arado
    [SerializeField] private Color color = Color.white;
    [SerializeField] private bool isPlantable = false;  // O tile começa não plantável

    // Nutrientes NPK no tile
    [SerializeField] private int nitrogen = 1000;
    [SerializeField] private int phosphorus = 1000;
    [SerializeField] private int potassium = 1000;
    [SerializeField] private int humidity = 1000;

    // Estado de rotação (0 = 0°, 1 = 90°, 2 = 180°, 3 = 270°)
    [SerializeField] private int rotationState = 0;

    // Propriedades públicas somente leitura
    public Sprite Sprite => sprite;
    public GameObject CustomTilePrefab => customTilePrefab;
    public GameObject PlowedTilePrefab => plowedTilePrefab;
    public Color TileColor => color;
    public bool IsPlantable => isPlantable;
    public int Nitrogen => nitrogen;
    public int Phosphorus => phosphorus;
    public int Potassium => potassium;
    public int Humidity => humidity;
    public int RotationState => rotationState;

    public override bool StartUp(Vector3Int position, ITilemap tilemap, GameObject go)
    {
        if (!Application.isPlaying)
        {
            return true;
        }

        TilemapManager tilemapManager = Object.FindObjectOfType<TilemapManager>();

        if (tilemapManager != null)
        {
            // Verificar se há informações salvas no TileInfoDictionary
            if (tilemapManager.tileInfoDictionary.TryGetValue(position, out TileInfo savedInfo))
            {
                Debug.Log("Tile encontrado no dicionário");
                // Restaurar as informações salvas
                isPlantable = savedInfo.isPlantable;
                nitrogen = savedInfo.nitrogen;
                phosphorus = savedInfo.phosphorus;
                potassium = savedInfo.potassium;
                humidity = savedInfo.humidity;

                // Verificar se há uma árvore na posição e removê-la
                string treeObjectName = $"Tree_{position.x}_{position.y}_{position.z}";
                GameObject treeObject = GameObject.Find(treeObjectName);
                if (treeObject != null)
                {
                    tilemapManager.RemoveInstantiatedTile(position);
                    Debug.Log($"Árvore encontrada na posição {position}, removendo...");
                    Destroy(treeObject);
                }

                string groundObjectName = $"Ground_{position.x}_{position.y}_{position.z}";
                GameObject groundObject = GameObject.Find(groundObjectName);
                if (groundObject != null)
                {
                    tilemapManager.SetInstantiatedTile(position,groundObject);
                    // Debug.Log($"Adicionando ground em {position}, ao dicionario");
                }

                // Verificar se o tile é plantável e transformar em estado arado
                if (savedInfo.isPlantable)
                {
                    Debug.Log($"Tile na posição {position} é plantável. Alterando para estado arado...");
                    ChangeToPlowedState(position);
                    return true;
                }

                // Verificar se há um objeto instanciado salvo e atualizá-lo
                if (tilemapManager.instantiatedTileDictionary.TryGetValue(position, out GameObject savedObject) && savedObject != null)
                {
                    //Debug.Log($"Tile guardado na posicao {position} eh {savedObject}");
                    tilemapManager.SetInstantiatedTile(position, savedObject);
                    savedObject.transform.position = tilemapManager.tilemap.CellToWorld(position) + new Vector3(0.5f, 0, 0.5f);
                    //Debug.Log($"Tile restaurado na posição {position}");
                    return true;
                }
            }

            // Caso não existam dados salvos, criar um novo tile
            TileInfo newInfo = new TileInfo(
                isPlantable,
                nitrogen,
                phosphorus,
                potassium,
                humidity
            );
            tilemapManager.SetTileInfo(position, newInfo);

            if (!tilemapManager.HasInstantiatedTile(position) && customTilePrefab != null)
            {
                Vector3 worldPosition = tilemapManager.tilemap.CellToWorld(position) + new Vector3(0.5f, 0, 0.5f);

                Transform parent = tilemapManager.parentTransform != null ? tilemapManager.parentTransform : tilemapManager.transform;

                // Determina a rotação com base no estado de rotação
                Quaternion rotation = Quaternion.Euler(0, rotationState * 90f, 0);

                GameObject instantiatedTile = Instantiate(customTilePrefab, worldPosition, rotation, parent);
                instantiatedTile.transform.position = worldPosition;
                instantiatedTile.name = $"Ground_{position.x}_{position.y}_{position.z}";

                tilemapManager.SetInstantiatedTile(position, instantiatedTile);
                //Debug.Log($"Novo tile criado na posição: {position}");
            }
        }
        else
        {
            Debug.LogWarning("TilemapManager não encontrado na cena.");
        }

        return true;
    }




    public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
    {
        tileData.sprite = sprite;
        tileData.color = color;
    }

    public override void RefreshTile(Vector3Int position, ITilemap tilemap)
    {
        tilemap.RefreshTile(position);
    }

    public void DisplayTileInfo()
    {
        Debug.Log($"Tile Info: Plantable = {isPlantable}, Nitrogênio = {nitrogen}, Fósforo = {phosphorus}, Potássio = {potassium}, Umidade = {humidity}");
    }

    public void ConsumeNutrients(int nAmount, int pAmount, int kAmount)
    {
        nitrogen = Mathf.Max(0, nitrogen - nAmount);
        phosphorus = Mathf.Max(0, phosphorus - pAmount);
        potassium = Mathf.Max(0, potassium - kAmount);
    }

    public void ChangeToPlowedState(Vector3Int position)
    {
        TilemapManager tilemapManager = Object.FindObjectOfType<TilemapManager>();

        if (plowedTilePrefab != null && tilemapManager != null)
        {
            GameObject currentTile = tilemapManager.GetInstantiatedTile(position);
            if (currentTile != null)
            {
                Destroy(currentTile);
            }
            else
            {
                Debug.LogWarning("Ref Tile is Null");
            }

            Vector3 worldPosition = tilemapManager.tilemap.CellToWorld(position) + new Vector3(0.5f, 0, 0.5f);

            Transform parent = tilemapManager.parentTransform != null ? tilemapManager.parentTransform : tilemapManager.transform;

            // Determina a rotação com base no estado de rotação
            Quaternion rotation = Quaternion.Euler(0, rotationState * 90f, 0);

            GameObject plowedTile = Instantiate(plowedTilePrefab, worldPosition, rotation, parent);
            plowedTile.transform.position = worldPosition;
            plowedTile.name = $"PlowedTile_{position.x}_{position.y}_{position.z}";

            tilemapManager.SetInstantiatedTile(position, plowedTile);

            TileInfo tileInfo = tilemapManager.GetTileInfo(position);
            if (tileInfo != null)
            {
                tileInfo.isPlantable = true;
                tilemapManager.SetTileInfo(position, tileInfo);
            }
        }
        else
        {
            Debug.LogWarning("PlowedTilePrefab ou TilemapManager não atribuído!");
        }
    }

    public void RevertToCustomTileState(Vector3Int position)
    {
        TilemapManager tilemapManager = Object.FindObjectOfType<TilemapManager>();

        if (customTilePrefab != null && tilemapManager != null)
        {
            GameObject currentTile = tilemapManager.GetInstantiatedTile(position);
            if (currentTile != null)
            {
                Destroy(currentTile);  // Remove o tile atual (o arado)
            }

            Vector3 worldPosition = tilemapManager.tilemap.CellToWorld(position) + new Vector3(0.5f, 0, 0.5f);

            Transform parent = tilemapManager.parentTransform != null ? tilemapManager.parentTransform : tilemapManager.transform;

            // Cria o tile customizado novamente
            GameObject customTile = Instantiate(customTilePrefab, worldPosition, Quaternion.identity, parent);
            customTile.transform.position = worldPosition;
            customTile.name = $"Ground_{position.x}_{position.y}_{position.z}";

            tilemapManager.SetInstantiatedTile(position, customTile);

            // Atualiza as informações do tile para refletir que ele voltou ao estado original (não arado)
            TileInfo tileInfo = tilemapManager.GetTileInfo(position);
            if (tileInfo != null)
            {
                tileInfo.isPlantable = this.isPlantable;  // Restaura o valor de plantabilidade
                tilemapManager.SetTileInfo(position, tileInfo);
            }
        }
        else
        {
            Debug.LogWarning("CustomTilePrefab ou TilemapManager não atribuído!");
        }
    }



    public bool HasNearbyWaterTile(Vector3Int position)
    {
        TilemapManager tilemapManager = Object.FindObjectOfType<TilemapManager>();
        if (tilemapManager == null)
        {
            Debug.LogError("TilemapManager não encontrado!");
            return false;
        }

        // Verifica em um raio de 2 tiles em torno da posição (X e Y)
        for (int xOffset = -2; xOffset <= 2; xOffset++)
        {
            for (int yOffset = -2; yOffset <= 2; yOffset++)
            {
                if (xOffset == 0 && yOffset == 0) continue; // Ignora a posição central

                Vector3Int checkPosition = new Vector3Int(position.x + xOffset, position.y + yOffset, position.z);
                TileBase nearbyTile = tilemapManager.tilemap.GetTile(checkPosition);

                if (nearbyTile is CustomTileBase customTile)
                {
                    // Verifica se é um "WaterTile" com base no nome ou em outro critério
                    if (customTile.name == "WaterTile") // Substitua "WaterTile" pelo critério que identifica os tiles de água
                    {
                        return true;
                    }
                }
            }
        }

        return false;
    }



}
