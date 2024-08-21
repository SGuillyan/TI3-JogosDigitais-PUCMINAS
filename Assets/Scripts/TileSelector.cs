using UnityEngine;
using UnityEngine.Tilemaps;

public class TileSelector : MonoBehaviour
{
    public Tilemap tilemap;  // Referência ao Tilemap
    public Tile highlightTile;  // Tile a ser usado como destaque
    private Vector3Int previousPosition;  // Armazena a última posição selecionada
    private TileBase previousTile;  // Armazena o tile original
    private bool hasPreviousTile = false;  // Verifica se já houve um tile selecionado

    public InventoryManager inventoryManager;  // Referência ao InventoryManager para verificar sementes selecionadas
    public TilemapPlant tilemapPlant;  // Referência ao sistema de plantio

    void SelectTile()
    {
        if (Input.GetMouseButtonDown(0))  // Detecta clique do mouse
        {
            // Converte a posição do mouse na tela para uma posição no mundo
            Vector3 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            // Converte a posição no mundo para uma célula do Tilemap
            Vector3Int gridPosition = tilemap.WorldToCell(worldPoint);
            gridPosition.z = 0;

            // Verifica se há uma semente selecionada no inventário
            if (inventoryManager.HasSelectedSeed())
            {
                // Se houver uma semente selecionada, tenta plantar
                tilemapPlant.PlantSeedAt(gridPosition, inventoryManager.GetSelectedSeedID());
                inventoryManager.PlantSeedAt(gridPosition);
            }
            else
            {
                // Se não houver semente selecionada, exibe informações do tile no console
                DisplayTileInfo(gridPosition);
            }
        }
    }

    void DisplayTileInfo(Vector3Int gridPosition)
    {
        TileBase clickedTile = tilemap.GetTile(gridPosition);

        if (clickedTile != null)
        {
            // Exibe as informações básicas do tile no console
            Debug.Log("Tile selecionado na posição: " + gridPosition);
            Debug.Log("Nome do Tile: " + clickedTile.name);

            // Tenta obter informações adicionais do Tile (se for um Tile 2D do tipo padrão)
            Tile tileData = tilemap.GetTile<Tile>(gridPosition);
            if (tileData != null)
            {
                Debug.Log("Sprite do Tile: " + tileData.sprite.name);
                Debug.Log("Cor do Tile: " + tilemap.GetColor(gridPosition));
                // Adicione aqui qualquer outra propriedade do Tile que você queira exibir
            }
        }
        else
        {
            Debug.Log("Nenhum tile na posição: " + gridPosition);
        }
    }

    private void Update()
    {
        SelectTile();
    }
}
