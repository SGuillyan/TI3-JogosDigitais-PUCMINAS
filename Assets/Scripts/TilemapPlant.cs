using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapPlant : MonoBehaviour
{
    public Tilemap tilemap;  // Referência ao Tilemap
    public PlantTile[] plantTilePrefabs;  // Array para armazenar todos os tipos de sementes (PlantTiles)

    public void PlantSeedAt(Vector3Int position, int seedIndex)
    {
        if (seedIndex >= 0 && seedIndex < plantTilePrefabs.Length)
        {
            PlantTile selectedPlantTile = Instantiate(plantTilePrefabs[seedIndex]);
            tilemap.SetTile(position, selectedPlantTile);
            selectedPlantTile.Plant(tilemap, position, this);  // Passa o MonoBehaviour (TilemapPlant) como 'caller'
            tilemap.RefreshTile(position);
            Debug.Log("Semente plantada: " + seedIndex + " em " + position);
        }
        else
        {
            Debug.LogError("Índice de semente inválido: " + seedIndex);
        }
    }
}
