using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "New Tree Tile", menuName = "Tiles/Tree Tile")]
public class TreeTile : TileBase
{
    public Sprite sprite;                  // Sprite do tile
    public GameObject treePrefab;          // Prefab da árvore a ser instanciada
    public GameObject groundPrefab;        // Prefab do chão a ser instanciado
    public CustomTileBase defaultTile;
    public Color color = Color.white;      // Cor do tile (opcional)

    // Propriedades para TileInfo
    public bool isPlantable = false;
    public int nitrogen = 2000;
    public int phosphorus = 2000;
    public int potassium = 2000;
    public int humidity = 50;

    public override bool StartUp(Vector3Int position, ITilemap tilemap, GameObject go)
    {
        if (!Application.isPlaying || treePrefab == null || groundPrefab == null)
        {
            return true;
        }

        TilemapManager tilemapManager = Object.FindObjectOfType<TilemapManager>();

        if (tilemapManager != null)
        {
            // Verificar se há informações salvas no TileInfoDictionary
            if (tilemapManager.tileInfoDictionary.TryGetValue(position, out TileInfo savedInfo))
            {
                Debug.Log("Tile encontrado no dicionário.");
                // Restaurar as informações salvas
                isPlantable = savedInfo.isPlantable;
                nitrogen = savedInfo.nitrogen;
                phosphorus = savedInfo.phosphorus;
                potassium = savedInfo.potassium;
                humidity = savedInfo.humidity;

                // Verificar se há um objeto instanciado salvo
                if (tilemapManager.instantiatedTileDictionary.TryGetValue(position, out GameObject savedTree) && savedTree != null)
                {
                    // Atualizar a referência do objeto instanciado no dicionário
                    tilemapManager.SetInstantiatedTile(position, savedTree);
                    savedTree.transform.position = tilemapManager.tilemap.CellToWorld(position) + new Vector3(0.5f, 0, 0.5f);
                    Debug.Log($"Árvore restaurada na posição: {position}");
                    return true;
                }
                else
                {
                    Debug.Log($"Nao encontramos objeto na posicao: {position}");
                }
            }

            // Caso não existam dados salvos, criar novos objetos
            TileInfo newInfo = new TileInfo(isPlantable, nitrogen, phosphorus, potassium, humidity);
            tilemapManager.SetTileInfo(position, newInfo);

            Vector3 worldPosition = tilemapManager.tilemap.CellToWorld(position) + new Vector3(0.5f, 0, 0.5f);

            Transform parent = tilemapManager.parentTransform != null ? tilemapManager.parentTransform : tilemapManager.transform;

            // Instanciar o chão, se ainda não estiver salvo
            if (!tilemapManager.HasInstantiatedTile(position) && groundPrefab != null)
            {
                GameObject ground = Instantiate(groundPrefab, worldPosition, Quaternion.identity, parent);
                ground.name = $"Ground_{position.x}_{position.y}_{position.z}";
                //Debug.Log($"Chão instanciado na posição: {position}");
            }

            // Instanciar a árvore
            Vector3 treePosition = worldPosition + new Vector3(0, 0.5f, 0);
            GameObject tree = Instantiate(treePrefab, treePosition, Quaternion.identity, parent);

            // Randomizar escala e rotação da árvore
            float randomScale = Random.Range(0.85f, 1.15f);
            tree.transform.localScale = new Vector3(randomScale, randomScale, randomScale);

            float randomRotation = Random.Range(0f, 360f);
            tree.transform.rotation = Quaternion.Euler(0, randomRotation, 0);

            tree.name = $"Tree_{position.x}_{position.y}_{position.z}";

            // Registrar a árvore no TilemapManager
            tilemapManager.SetInstantiatedTile(position, tree);

            //Debug.Log($"Árvore e chão instanciados em {position} com escala {randomScale} e rotação {randomRotation}");
        }
        else
        {
            Debug.LogWarning("TilemapManager não encontrado na cena.");
        }

        return true;
    }


    public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
    {
        // Configura o sprite e a cor do tile
        tileData.sprite = sprite;
        tileData.color = color;
    }

    public override void RefreshTile(Vector3Int position, ITilemap tilemap)
    {
        tilemap.RefreshTile(position);
    }

    // Método para cortar a árvore e atualizar a referência para o chão
    public void CutDown(Vector3Int position)
    {
        TilemapManager tilemapManager = Object.FindObjectOfType<TilemapManager>();

        if (tilemapManager != null)
        {
            // Remove a árvore atual da posição
            GameObject treeObject = tilemapManager.GetInstantiatedTile(position);
            if (treeObject != null && treeObject.name.StartsWith("Tree"))
            {
                Debug.Log($"Árvore encontrada na posição {position}, removendo...");
                Destroy(treeObject); // Remove o objeto da árvore
                tilemapManager.RemoveInstantiatedTile(position); // Remove a referência da árvore no dicionário
            }

            // Recuperar ou manter o TileInfo existente
            TileInfo existingTileInfo = tilemapManager.GetTileInfo(position);

            if (existingTileInfo == null)
            {
                Debug.LogError($"Erro: Nenhum TileInfo encontrado para a posição {position}. Não é possível preservar as informações.");
                return;
            }

            // Atualiza o tile no Tilemap para o novo tipo, mas mantém o TileInfo
            tilemapManager.tilemap.SetTile(position, tilemapManager.defaultTile); // Substitui o tile pelo FarmTile ou outro CustomTileBase
            tilemapManager.tilemap.RefreshTile(position);

            // Mantém o TileInfo existente (não sobrescreve)
            tilemapManager.SetTileInfo(position, existingTileInfo);

            // Atualiza o objeto instanciado no dicionário para o chão (ou qualquer outro associado ao novo tile)
            string groundObjectName = $"Ground_{position.x}_{position.y}_{position.z}";
            GameObject groundObject = GameObject.Find(groundObjectName);

            if (groundObject != null)
            {
                Debug.Log($"Atualizando a referência do dicionário para o chão na posição {position}");
                tilemapManager.SetInstantiatedTile(position, groundObject); // Atualiza o dicionário para o chão existente
            }
            else
            {
                Debug.LogWarning($"Chão não encontrado na posição {position}. Pode haver inconsistência visual.");
            }

            Debug.Log($"Tile transformado de TreeTile para CustomTileBase na posição {position}, mantendo informações do dicionário.");
        }
        else
        {
            Debug.LogWarning("TilemapManager não encontrado na cena.");
        }
    }


}
