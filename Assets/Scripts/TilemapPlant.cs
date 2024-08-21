using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapPlant : MonoBehaviour
{
    public Tilemap tilemap;  // Referência ao Tilemap
    public PlantTile plantTilePrefab;  // Referência ao PlantTile usado como prefab

    public void PlantSeedAt(Vector3Int position)
    {
        //if (tilemap.GetTile(position) is SoilTile)  // Verifica se o Tile atual é de solo
        //{
            PlantTile newPlantTile = Instantiate(plantTilePrefab);
            tilemap.SetTile(position, newPlantTile);
            newPlantTile.Plant(tilemap, position, this);  // Passa o MonoBehaviour (TilemapManager) como 'caller'
            tilemap.RefreshTile(position);
        //}
    }

    public void CollectPlantAt(Vector3Int position)
    {
        PlantTile plantTile = tilemap.GetTile<PlantTile>(position);
        if (plantTile != null && plantTile.isFullyGrown)
        {
            plantTile.Collect(tilemap, position);
        }
    }

    public void ResetPlantAt(Vector3Int position)
    {
        PlantTile plantTile = tilemap.GetTile<PlantTile>(position);
        if (plantTile != null)
        {
            plantTile.ResetGrowth();
            tilemap.RefreshTile(position);
        }
    }
}
