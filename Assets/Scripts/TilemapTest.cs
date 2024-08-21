using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapTest : MonoBehaviour
{
    public TilemapManager tilemapManager;
    public TilemapPlant tilemapPlant;

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Clique com o botão esquerdo do mouse
        {
            Vector3 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int gridPosition = tilemapManager.tilemap.WorldToCell(worldPoint);
            gridPosition.z = 0;
            // Testa os dados do tile na posição clicada
            tilemapManager.TestTileData(gridPosition);
        }

        if (Input.GetMouseButtonDown(1)) // Clique com o botão direito do mouse
        {
            Vector3 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int gridPosition = tilemapManager.tilemap.WorldToCell(worldPoint);
            gridPosition.z = 0;
            // Modifica os dados do tile na posição clicada
            tilemapPlant.PlantSeedAt(gridPosition);
        }
    }
}
