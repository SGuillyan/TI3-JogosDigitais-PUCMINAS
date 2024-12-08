using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "New Tree Tile", menuName = "Tiles/Tree Tile")]
public class TreeTile : TileBase
{
    public Sprite sprite;                  // Sprite do tile
    public GameObject treePrefab;          // Prefab da árvore a ser instanciada
    public GameObject groundPrefab;        // Prefab do chão a ser instanciado
    public Color color = Color.white;      // Cor do tile (opcional)

    public override bool StartUp(Vector3Int position, ITilemap tilemap, GameObject go)
    {
        if (!Application.isPlaying || treePrefab == null || groundPrefab == null)
        {
            return true;
        }

        TilemapManager tilemapManager = Object.FindObjectOfType<TilemapManager>();

        if (tilemapManager != null)
        {
            // Calcula a posição no mundo com base no tilemap
            Vector3 worldPosition = tilemapManager.tilemap.CellToWorld(position) + new Vector3(0.5f, 0, 0.5f);

            // Define o pai para organização hierárquica na cena
            Transform parent = tilemapManager.parentTransform != null ? tilemapManager.parentTransform : tilemapManager.transform;

            // Instancia o chão na posição do tile
            GameObject ground = Instantiate(groundPrefab, worldPosition, Quaternion.identity, parent);
            ground.name = $"Ground_{position.x}_{position.y}_{position.z}";

            // Instancia a árvore acima do chão, com randomização de escala e rotação
            Vector3 treePosition = worldPosition + new Vector3(0, 0.5f, 0);
            GameObject tree = Instantiate(treePrefab, treePosition, Quaternion.identity, parent);

            // Randomiza a escala da árvore
            float randomScale = Random.Range(0.85f, 1.15f);
            tree.transform.localScale = new Vector3(randomScale, randomScale, randomScale);

            // Randomiza a rotação da árvore
            float randomRotation = Random.Range(0f, 360f); // Rotação aleatória no eixo Y
            tree.transform.rotation = Quaternion.Euler(0, randomRotation, 0);

            tree.name = $"Tree_{position.x}_{position.y}_{position.z}";

            // Registra a árvore no TilemapManager
            tilemapManager.SetInstantiatedTile(position, tree);

            //Debug.Log($"Chão e árvore instanciados em {position} com escala {randomScale} e rotação {randomRotation}");
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
            if (treeObject != null)
            {
                Destroy(treeObject); // Remove o objeto da árvore
            }

            // Atualiza a referência para o chão no TilemapManager
            Vector3 worldPosition = tilemapManager.tilemap.CellToWorld(position) + new Vector3(0.5f, 0, 0.5f);
            GameObject groundObject = GameObject.Find($"Ground_{position.x}_{position.y}_{position.z}");

            if (groundObject != null)
            {
                tilemapManager.SetInstantiatedTile(position, groundObject);
               // Debug.Log($"Árvore cortada e referência atualizada para chão na posição: {position}");
            }
            else
            {
                Debug.LogError($"Erro: Objeto de chão não encontrado na posição {position}");
            }
        }
        else
        {
            Debug.LogWarning("TilemapManager não encontrado na cena.");
        }
    }
}
