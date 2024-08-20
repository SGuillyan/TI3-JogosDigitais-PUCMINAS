using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapTest : MonoBehaviour
{
    public TilemapManager tilemapManager;

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Clique com o botão esquerdo do mouse
        {
            Vector3 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int gridPosition = tilemapManager.tilemap.WorldToCell(worldPoint);

            // Testa os dados do tile na posição clicada
            tilemapManager.TestTileData(gridPosition);
        }

        if (Input.GetMouseButtonDown(1)) // Clique com o botão direito do mouse
        {
            Vector3 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int gridPosition = tilemapManager.tilemap.WorldToCell(worldPoint);

            // Modifica os dados do tile na posição clicada
            TileInfo newInfo = new TileInfo()
            {
                isPlantable = false,
                nutrienteX = Random.Range(0, 10),
                nutrienteY = Random.Range(0, 10)
            };
            tilemapManager.SetTileInfo(gridPosition, newInfo);

            // Testa os dados após a modificação
            tilemapManager.TestTileData(gridPosition);
        }
    }
}
