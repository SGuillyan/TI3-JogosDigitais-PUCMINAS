using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.EventSystems;

public class TileSelector : MonoBehaviour
{
    public Tilemap tilemap;  // Referência ao Tilemap
    public Sprite debugSprite;  // Sprite de debug para colorir o tile
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
            // Converte a posição do mouse na tela para uma posição no mundo
            Vector3 screenPoint = Input.mousePosition;
            Vector3 worldPoint = Camera.main.ScreenToWorldPoint(screenPoint);
            
            // Depuração detalhada da posição
            Debug.Log("ScreenPoint: " + screenPoint);
            Debug.Log("WorldPoint: " + " X " + worldPoint.x + " Y " + worldPoint.y + " Z " + worldPoint.z);

            // Se o Tilemap está no plano XZ, definimos o Y para 0 (plano horizontal) e mantemos o X e Z
            worldPoint.y = 0f;  // Como o Tilemap está no plano XZ, y deve ser ajustado para 0

            // Converte a posição do mundo ajustada para uma célula do Tilemap
            Vector3Int gridPosition = tilemap.WorldToCell(worldPoint);
            Debug.Log("gridPosition" + " X " + gridPosition.x + " Y " + gridPosition.y + " Z " + gridPosition.z);

            // Verifica se há um tile na posição clicada
            TileBase clickedTile = tilemap.GetTile(gridPosition);

            if (clickedTile != null)
            {
                DisplayTileInfo(gridPosition);
            }
            else
            {
                Debug.Log("Nenhum tile encontrado na posição. Criando tile de debug.");
                CreateDebugTile(gridPosition);
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

    // Função para criar um tile temporário de debug
    void CreateDebugTile(Vector3Int gridPosition)
    {
        // Cria um novo Tile para substituir o tile original com o sprite de debug
        Tile tempTile = ScriptableObject.CreateInstance<Tile>();
        tempTile.sprite = debugSprite;  // Usa o sprite de debug fornecido

        Vector3Int NewGridPosition = gridPosition;
        NewGridPosition.x = 0;
        NewGridPosition.y = 0;
        NewGridPosition.z = 0;

        // Colore o tile de debug
        tilemap.SetTile(gridPosition, tempTile);
        tilemap.SetTile(NewGridPosition, tempTile);

        Debug.Log("Tile de debug criado na posição: " + gridPosition);
    }

    private void Update()
    {
        SelectTile();
    }
}
