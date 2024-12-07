using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapFertilize : MonoBehaviour
{
    public Tilemap tilemap;  // Referência ao Tilemap
    public Item[] fertilizers;  // Array para armazenar todos os tipos de sementes (PlantTiles)
    public TilemapManager tilemapManager;

    public void FertilizeAt(Vector3Int position, int fertIndex)
    {
        fertIndex = fertIndex - 200;
        if (fertIndex >= 0 && fertIndex < fertilizers.Length)
        {
            TileInfo currentTileInfo = tilemapManager.GetTileInfo(position);
            
            currentTileInfo.nitrogen += (int)fertilizers[fertIndex].reqNitro;
            currentTileInfo.phosphorus += (int)fertilizers[fertIndex].reqPhosf;
            currentTileInfo.potassium += (int)fertilizers[fertIndex].reqK;

            tilemapManager.SetTileInfo(position, currentTileInfo);

            Debug.Log("Fertilizante " + fertIndex + " em " + position);

            // Analytics
            // AnalyticsSystem.AddAnalyticPlants_Planted(this.name, selectedPlantTile.name, 1);
        }
        else
        {
            Debug.LogError("Índice de fertilizante inválido: " + fertIndex);
        }
    }
}
