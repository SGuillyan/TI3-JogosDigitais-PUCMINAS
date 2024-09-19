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
    public Inventory playerInventory;  // Referência ao inventário do jogador

    void SelectTile()
    {
        // Verifica se o inventário está aberto ou se o clique é na UI
        if (inventoryUI.inventoryUI.activeSelf || EventSystem.current.IsPointerOverGameObject())
        {
            return; // Se o inventário estiver aberto ou o clique for na UI, não processa o clique
        }

        if (Input.GetMouseButtonDown(0))  // Detecta clique do mouse
        {
            Vector3 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int gridPosition = tilemap.WorldToCell(worldPoint);
            gridPosition.z = 0;

            TileBase clickedTile = tilemap.GetTile(gridPosition);

            // Verifica se o tile clicado é um PlantTile e está completamente crescido
            if (clickedTile is PlantTile plantTile && plantTile.isFullyGrown)
            {
                plantTile.Collect(tilemap, gridPosition, playerInventory); // Coleta a planta e restaura o tile original
            }
            else if (inventoryManager.HasSelectedSeed())
            {
                // Verifica se o tile na posição é plantável
                TileInfo tileInfo = tilemapManager.GetTileInfo(gridPosition);
                if (tileInfo != null && tileInfo.isPlantable)
                {
                    // Obtém o índice da semente selecionada
                    int seedIndex = inventoryManager.GetSelectedSeedID();

                    // Verifica se o índice da semente é válido e obtém o PlantTile correspondente
                    if (seedIndex >= 0 && seedIndex < tilemapPlant.plantTilePrefabs.Length)
                    {
                        PlantTile selectedPlantTile = tilemapPlant.plantTilePrefabs[seedIndex];

                        // Verifica se há nutrientes suficientes para plantar
                        if (HasEnoughNutrients(tileInfo, selectedPlantTile))
                        {
                            // Realiza o plantio
                            tilemapPlant.PlantSeedAt(gridPosition, seedIndex);
                            inventoryManager.PlantSeedAt(gridPosition);
                        }
                        else
                        {
                            Debug.LogWarning("Nutrientes insuficientes para plantar na posição " + gridPosition);
                        }
                    }
                    else
                    {
                        Debug.LogError("Índice de semente inválido: " + seedIndex);
                    }
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

// Função para verificar se o solo tem nutrientes suficientes
    private bool HasEnoughNutrients(TileInfo tileInfo, PlantTile plantTile)
    {
        return tileInfo.nitrogen >= plantTile.requiredNitrogen &&
            tileInfo.phosphorus >= plantTile.requiredPhosphorus &&
            tileInfo.potassium >= plantTile.requiredPotassium;
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
