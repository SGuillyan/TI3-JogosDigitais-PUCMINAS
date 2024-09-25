using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.EventSystems;

public class TileSelector : MonoBehaviour
{
    public Tilemap tilemap;  // Referência ao Tilemap
    public GameObject debugObjectPrefab;  // Prefab do GameObject de debug
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
            return; // Se o inventário estiver aberto ou o clique for na UI, não processa o clique
        }

        if (Input.GetMouseButtonDown(0))  // Detecta clique do mouse
        {
            // Captura a posição do mouse na tela (Screen Space)
            Vector3 mousePosition = Input.mousePosition;

            // Cria um plano no eixo XZ, com Y = 0 (plano horizontal)
            Plane plane = new Plane(Vector3.up, Vector3.zero);

            // Cria um raio da câmera para o ponto onde o mouse está clicando
            Ray ray = Camera.main.ScreenPointToRay(mousePosition);

            float rayDistance;

            // Verifica onde o raio intersecta o plano (plano XZ)
            if (plane.Raycast(ray, out rayDistance))
            {
                // Converte o ponto de interseção para o ponto no mundo
                Vector3 worldPoint = ray.GetPoint(rayDistance);

                // Converte a posição do mundo para uma célula do Tilemap
                Vector3Int gridPosition = tilemap.WorldToCell(worldPoint);
                gridPosition.z = 0; // Garante que o Z da célula seja 0, já que estamos no plano XZ

                // Verifica se há um tile na posição clicada
                TileBase clickedTile = tilemap.GetTile(gridPosition);

                // Se o tile for uma planta completamente crescida, faça a colheita
                if (clickedTile is PlantTile plantTile && plantTile.isFullyGrown)
                {
                    plantTile.Collect(tilemap, gridPosition, playerInventory); // Coleta a planta
                }
                // Verifica se o jogador tem uma semente selecionada e se o tile é plantável
                else if (inventoryManager.HasSelectedSeed())
                {
                    TileInfo tileInfo = tilemapManager.GetTileInfo(gridPosition);
                    if (tileInfo != null && tileInfo.isPlantable)
                    {
                        tilemapPlant.PlantSeedAt(gridPosition, inventoryManager.GetSelectedSeedID());
                        inventoryManager.PlantSeedAt(gridPosition); // Atualiza o inventário após o plantio
                    }
                    else
                    {
                        Debug.Log("O tile na posição " + gridPosition + " não é plantável.");
                    }
                }
                // Caso contrário, exibe as informações do tile
                else
                {
                    DisplayTileInfo(gridPosition);
                    CreateDebugTile(gridPosition);
                }
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

            if (tileInfo != null && clickedTile is CustomTileBase tileData)  // Verifica se o tile é um CustomTileBase
            {
                // Atualiza o UIManager com as informações do tile
                uiManager.UpdateTileInfo(tileData.sprite, tileInfo.nitrogen, tileInfo.phosphorus, tileInfo.potassium, tileInfo.humidity);
            }
            else if (tileInfo != null && clickedTile is PlantTile tileData2)  // Verifica se o tile é uma PlantTile
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

    // Função para instanciar um GameObject de debug na posição do tile
    void CreateDebugTile(Vector3Int gridPosition)
    {
        // Converte a posição da célula do grid para uma posição no mundo
        Vector3 worldPosition = tilemap.CellToWorld(gridPosition);

        // Instancia o GameObject de debug na posição do mundo
        if (debugObjectPrefab != null)
        {
            Instantiate(debugObjectPrefab, worldPosition, Quaternion.identity);
            Debug.Log("GameObject de debug instanciado na posição: " + worldPosition);
        }
        else
        {
            Debug.LogWarning("Prefab de debugObject não atribuído!");
        }
    }

    private void Update()
    {
        SelectTile();
    }
}
