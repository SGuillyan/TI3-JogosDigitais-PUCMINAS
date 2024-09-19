using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.EventSystems;  // Necessário para detectar cliques em UI

public class TileSelector : MonoBehaviour
{
    public Tilemap tilemap;  // Referência ao Tilemap
    public Tile highlightTile;  // Tile a ser usado como destaque
    private Vector3Int previousPosition;  // Armazena a última posição selecionada
    private TileBase previousTile;  // Armazena o tile original
    private bool hasPreviousTile = false;  // Verifica se já houve um tile selecionado

    public InventoryManager inventoryManager;  // Referência ao InventoryManager para verificar sementes selecionadas
    public TilemapPlant tilemapPlant;  // Referência ao sistema de plantio
    public InventoryUI inventoryUI;  // Referência ao script de UI do inventário
    public TilemapManager tilemapManager;  // Referência ao script TilemapManager para verificar informações do tile
    public UIManager uiManager;  // Referência ao UIManager para exibir as informações
    public Inventory playerInventory;  // Referência ao inventário do jogador

    void SelectTile()
    {
        // Verifica se o inventário está aberto ou se o clique é na UI
        if (inventoryUI.inventoryUI.activeSelf || EventSystem.current.IsPointerOverGameObject())
        {
            // Se o inventário estiver aberto ou o clique for na UI, não processa o clique
            return;
        }

        if (Input.GetMouseButtonDown(0))  // Detecta clique do mouse
        {
            // Converte a posição do mouse na tela para uma posição no mundo
            Vector3 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            // Converte a posição no mundo para uma célula do Tilemap
            Vector3Int gridPosition = tilemap.WorldToCell(worldPoint);
            gridPosition.z = 0;

            TileBase clickedTile = tilemap.GetTile(gridPosition);

            // Verifica se o tile clicado é um PlantTile e está completamente crescido
            if (clickedTile is PlantTile plantTile && plantTile.isFullyGrown)
            {
                // Coleta a planta e restaura o tile original
                plantTile.Collect(tilemap, gridPosition, playerInventory);
            }
            else if (inventoryManager.HasSelectedSeed())
            {
                // Verifica se o tile na posição é plantável
                TileInfo tileInfo = tilemapManager.GetTileInfo(gridPosition);
                if (tileInfo != null && tileInfo.isPlantable)
                {
                    // Se o tile for plantável, realiza o plantio
                    tilemapPlant.PlantSeedAt(gridPosition, inventoryManager.GetSelectedSeedID());
                    inventoryManager.PlantSeedAt(gridPosition);
                }
                else
                {
                    Debug.Log("O tile na posição " + gridPosition + " não é plantável.");
                }
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
            // Exibe o tipo do tile para depuração
            Debug.Log("Tipo de tile clicado: " + clickedTile.GetType().Name);

            // Obtém o TileInfo do tile clicado
            TileInfo tileInfo = tilemapManager.GetTileInfo(gridPosition);

            if (tileInfo != null && clickedTile is CustomTileBase tileData)  // Substitua "CustomTileBase" pelo tipo real que você está usando
            {
                // Atualiza o UIManager com as informações do tile
                uiManager.UpdateTileInfo(tileData.sprite, tileInfo.nitrogen, tileInfo.phosphorus, tileInfo.potassium, tileInfo.humidity);
            }
            else if (tileInfo != null && clickedTile is PlantTile tileData2)
            {
                uiManager.UpdateTileInfo(tileData2.sprite, tileInfo.nitrogen, tileInfo.phosphorus, tileInfo.potassium, tileInfo.humidity);
            }
            else
            {
                Debug.Log("Nenhum dado do tile encontrado ou tipo de tile incompatível.");
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
