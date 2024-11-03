using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.EventSystems;

public class TileSelector : MonoBehaviour
{
    public Tilemap tilemap;
    public GameObject debugObjectPrefab;
    public InventoryManager inventoryManager;
    public TilemapPlant tilemapPlant;
    public InventoryUI inventoryUI;
    public TilemapManager tilemapManager;
    public UIManager uiManager;
    public Inventory playerInventory;
    public Animator tileInfoAnimator;  // Animator para a janela de informações

    public void SelectTile()
    {
        if (inventoryUI.inventoryUI.activeSelf || EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        if(Input.touchCount > 0)
        {
            Vector3 mousePosition = Input.mousePosition;
            Plane plane = new Plane(Vector3.up, Vector3.zero);
            Ray ray = Camera.main.ScreenPointToRay(mousePosition);
            if (plane.Raycast(ray, out float rayDistance))
            {
                Vector3 worldPoint = ray.GetPoint(rayDistance);
                Vector3Int gridPosition = tilemap.WorldToCell(worldPoint);
                gridPosition.z = 0;
                TileBase clickedTile = tilemap.GetTile(gridPosition);

                switch(ToolsManager.activeTool)
                {
                    case ToolsManager.Tools.Plow:
                        UsePlowTool(clickedTile,gridPosition);
                        break;
                    case ToolsManager.Tools.Flatten:
                        //Logica de flatten//
                        break; 
                    case ToolsManager.Tools.Harvest:
                        UseHarvestTool(clickedTile,gridPosition);
                        break;
                    case ToolsManager.Tools.Info:
                        UseInfoTool(gridPosition);
                        break;

                    default:
                        UsePlantTool(gridPosition);
                        break;
                }
            }
        }
    }
    void UsePlowTool(TileBase tile, Vector3Int gridPosition){
        if (tile is CustomTileBase customTile)
            {
                customTile.ChangeToPlowedState(gridPosition);
                Debug.Log($"Solo arado na posição: {gridPosition}");
            }
    }
    void UseInfoTool(Vector3Int gridPosition){
        DisplayTileInfo(gridPosition);
    }

    void UseHarvestTool(TileBase tile, Vector3Int gridPosition){
        if (tile is PlantTile plantTile && plantTile.isFullyGrown)
        {
            plantTile.Collect(tilemap, gridPosition, playerInventory);
        }
        else{
            Debug.Log("Tile Não Harvestable");
        }
    }

    void UsePlantTool(Vector3Int gridPosition){
        if (inventoryManager.HasSelectedSeed())
            {
                TileInfo tileInfo = tilemapManager.GetTileInfo(gridPosition);
                if (tileInfo != null && tileInfo.isPlantable)
                {
                    tilemapPlant.PlantSeedAt(gridPosition, inventoryManager.GetSelectedSeedID());
                    inventoryManager.PlantSeedAt(gridPosition);
                }
                else
                {
                    Debug.Log("O tile na posição " + gridPosition + " não é plantável.");
                }
            }
    }

    /*void ProcessLeftClick()
    {
        Vector3 mousePosition = Input.mousePosition;
        Plane plane = new Plane(Vector3.up, Vector3.zero);
        Ray ray = Camera.main.ScreenPointToRay(mousePosition);

        if (plane.Raycast(ray, out float rayDistance))
        {
            Vector3 worldPoint = ray.GetPoint(rayDistance);
            Vector3Int gridPosition = tilemap.WorldToCell(worldPoint);
            gridPosition.z = 0;

            TileBase clickedTile = tilemap.GetTile(gridPosition);


            if (clickedTile is PlantTile plantTile && plantTile.isFullyGrown)
            {
                plantTile.Collect(tilemap, gridPosition, playerInventory);
            }
            else if (inventoryManager.HasSelectedSeed())
            {
                TileInfo tileInfo = tilemapManager.GetTileInfo(gridPosition);
                if (tileInfo != null && tileInfo.isPlantable)
                {
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
                DisplayTileInfo(gridPosition);
            }
        }
    }

    void ProcessRightClick()
    {
        Vector3 mousePosition = Input.mousePosition;
        Plane plane = new Plane(Vector3.up, Vector3.zero);
        Ray ray = Camera.main.ScreenPointToRay(mousePosition);

        if (plane.Raycast(ray, out float rayDistance))
        {
            Vector3 worldPoint = ray.GetPoint(rayDistance);
            Vector3Int gridPosition = tilemap.WorldToCell(worldPoint);
            gridPosition.z = 0;

            TileBase clickedTile = tilemap.GetTile(gridPosition);

            if (clickedTile is CustomTileBase customTile)
            {
                customTile.ChangeToPlowedState(gridPosition);
                Debug.Log($"Solo arado na posição: {gridPosition}");
            }
            else
            {
                Debug.Log("O tile clicado não é um CustomTileBase.");
            }
        }
    }*/

    void DisplayTileInfo(Vector3Int gridPosition)
    {
        TileBase clickedTile = tilemap.GetTile(gridPosition);

        if (clickedTile != null)
        {
            Debug.Log("Tipo de tile clicado: " + clickedTile.GetType().Name);

            TileInfo tileInfo = tilemapManager.GetTileInfo(gridPosition);

            if (tileInfo != null)
            {
                if (clickedTile is CustomTileBase tileData)
                {
                    uiManager.UpdateTileInfo(tileData.sprite, tileInfo.nitrogen, tileInfo.phosphorus, tileInfo.potassium, tileInfo.humidity, tileInfo.isPlantable);
                }
                else if (clickedTile is PlantTile tileData2)
                {
                    uiManager.UpdateTileInfo(tileData2.sprite, tileInfo.nitrogen, tileInfo.phosphorus, tileInfo.potassium, tileInfo.humidity, tileInfo.isPlantable);
                }

                tileInfoAnimator.SetBool("OpenInfo", true);  // Ativa a animação de abertura
            }
        }
        else
        {
            Debug.Log("Nenhum tile na posição: " + gridPosition);
        }
    }

    public void HideTileInfo()
    {
        tileInfoAnimator.SetBool("OpenInfo", false);  // Ativa a animação de fechamento
    }

    /*private void Update()
    {
        SelectTile();
    }*/
}
