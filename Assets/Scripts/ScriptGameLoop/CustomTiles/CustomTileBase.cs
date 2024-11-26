using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "New Custom Tile", menuName = "Tiles/Custom Tile")]
public class CustomTileBase : TileBase
{
    // Propriedades personalizadas
    public Sprite sprite;
    public GameObject customTilePrefab;  // Prefab do GameObject associado ao tile
    public GameObject plowedTilePrefab;  // Prefab do GameObject associado ao solo arado
    public Color color = Color.white;
    public bool isPlantable = false;  // O tile começa não plantável

    // Nutrientes NPK no tile
    public int nitrogen = 1000;
    public int phosphorus = 1000;
    public int potassium = 1000;
    public int humidity = 1000;

    public override bool StartUp(Vector3Int position, ITilemap tilemap, GameObject go)
    {
        if (!Application.isPlaying)
        {
            return true;
        }

        TilemapManager tilemapManager = Object.FindObjectOfType<TilemapManager>();

        if (tilemapManager != null)
        {
            TileInfo info = new TileInfo(
                isPlantable,
                nitrogen,
                phosphorus,
                potassium,
                humidity
            );

            tilemapManager.SetTileInfo(position, info);

            if (!tilemapManager.HasInstantiatedTile(position) && customTilePrefab != null)
            {
                Vector3 worldPosition = tilemapManager.tilemap.CellToWorld(position) + new Vector3(0.5f, 0, 0.5f);

                Transform parent = tilemapManager.parentTransform != null ? tilemapManager.parentTransform : tilemapManager.transform;

                GameObject instantiatedTile = Instantiate(customTilePrefab, worldPosition, Quaternion.identity, parent);
                instantiatedTile.transform.position = worldPosition;
                instantiatedTile.name = $"CustomTile_{position.x}_{position.y}_{position.z}";

                tilemapManager.SetInstantiatedTile(position, instantiatedTile);
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

            Vector3 worldPosition = tilemapManager.tilemap.CellToWorld(position) + new Vector3(0.5f, 0, 0.5f);

            Transform parent = tilemapManager.parentTransform != null ? tilemapManager.parentTransform : tilemapManager.transform;

            GameObject plowedTile = Instantiate(plowedTilePrefab, worldPosition, Quaternion.identity, parent);
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

    public bool HasAdjacentWaterTile(Vector3Int position)
    {
        TilemapManager tilemapManager = Object.FindObjectOfType<TilemapManager>();
        if (tilemapManager == null)
        {
            Debug.LogError("TilemapManager não encontrado!");
            return false;
        }

        Vector3Int[] adjacentPositions =
        {
            new Vector3Int(position.x - 1, position.y, position.z),
            new Vector3Int(position.x + 1, position.y, position.z),
            new Vector3Int(position.x, position.y - 1, position.z),
            new Vector3Int(position.x, position.y + 1, position.z),
            new Vector3Int(position.x - 1, position.y - 1, position.z),
            new Vector3Int(position.x + 1, position.y + 1, position.z),
            new Vector3Int(position.x - 1, position.y + 1, position.z),
            new Vector3Int(position.x + 1, position.y - 1, position.z)
        };

        foreach (var adjPos in adjacentPositions)
        {
            TileBase adjacentTile = tilemapManager.tilemap.GetTile(adjPos);
            if (adjacentTile is CustomTileBase customTile)
            {
                // Verifica uma propriedade específica ou uma tag para identificar um "WaterTile"
                if (customTile.name == "WaterTile") // Substitua "WaterTile" pelo nome ou critério que identifica o WaterTile
                {
                    return true;
                }
            }
        }

        return false;
    }

}
