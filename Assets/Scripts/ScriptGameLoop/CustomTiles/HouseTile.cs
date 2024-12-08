using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "New House Tile", menuName = "Tiles/House Tile")]
public class HouseTile : TileBase
{
    public Sprite sprite;                  // Sprite do tile
    public GameObject groundPrefab;        // Prefab do chão a ser instanciado
    public Color color = Color.white;      // Cor do tile (opcional)

    public override bool StartUp(Vector3Int position, ITilemap tilemap, GameObject go)
    {
        if (!Application.isPlaying || groundPrefab == null)
        {
            return true; // Retorna imediatamente se o jogo não está em execução ou o prefab não está configurado
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

            // Registra o chão no TilemapManager
            tilemapManager.SetInstantiatedTile(position, ground);

            Debug.Log($"Chão instanciado em {position}");
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
}
