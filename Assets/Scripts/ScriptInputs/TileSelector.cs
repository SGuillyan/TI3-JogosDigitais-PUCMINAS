using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class TileSelector : MonoBehaviour
{
    public Tilemap tilemap;
    public GameObject debugObjectPrefab;
    public InventoryManager inventoryManager;
    public TilemapPlant tilemapPlant;
    public InventoryUI inventoryUI;
    public TilemapManager tilemapManager;
    public UIManager uiManager;
    [SerializeField] private List<GameObject> uiPanels;
    public Inventory playerInventory;
    public Animator tileInfoAnimator;  // Animator para a janela de informações

    private bool isTouchProcessed = false;  // Variável para controlar o processamento do toque
    private float continuoMenuTimer = 0.3f; // Verifica se o toque processado está ativo desde o memonto em que algum menu estava ativo
    private float continuott;

    private void Start()
    {
        continuott = continuoMenuTimer;
    }

    public void SelectTile()
    {
        // Verifica se algum painel de UI está ativo ou se o painel de informações está aberto
        if (IsAnyUIPanelActive() )//|| tileInfoAnimator.GetBool("OpenInfo"))
        {
            return;
        }

        if (Input.touchCount > 0)
        {
            if (MenuManager.openedMenu)
            {
                continuoMenuTimer = continuott;
                return;
            }
            if (continuoMenuTimer > 0f)
            {
                continuoMenuTimer -= Time.deltaTime;
                return;
            }


            Vector3 mousePosition = Input.mousePosition;
            Plane plane = new Plane(Vector3.up, Vector3.zero);
            Ray ray = Camera.main.ScreenPointToRay(mousePosition);
            if (plane.Raycast(ray, out float rayDistance))
            {
                Vector3 worldPoint = ray.GetPoint(rayDistance);
                Vector3Int gridPosition = tilemap.WorldToCell(worldPoint);
                gridPosition.z = 0;
                TileBase clickedTile = tilemap.GetTile(gridPosition);

                switch (ToolsManager.activeTool)
                {
                    case ToolsManager.Tools.Plow:
                        UsePlowTool(clickedTile, gridPosition);
                        break;
                    case ToolsManager.Tools.Flatten:
                        // Logica de flatten //
                        break;
                    case ToolsManager.Tools.Harvest:
                        UseHarvestTool(clickedTile, gridPosition);
                        break;
                    case ToolsManager.Tools.Info:
                        UseInfoTool(gridPosition);
                        //Debug.Log("USING INFO");
                        break;
                    case ToolsManager.Tools.None:
                        UsePlantTool(gridPosition);
                        break;
                }
            }

            isTouchProcessed = true;  // Marca o toque como processado
        }
        else
        {
            isTouchProcessed = false;  // Reseta quando o toque é liberado
        }
    }

    private bool IsAnyUIPanelActive()
    {
        foreach (GameObject panel in uiPanels)
        {
            if (panel.activeSelf)
            {
                //Debug.Log("Painel " + panel.name + " aberto");
                return true; // Se algum painel estiver ativo, retorna true
            }
        }
        //Debug.Log("Nenhum painel aberto");
        return false; // Caso nenhum painel esteja ativo, retorna false
    }

    void UsePlowTool(TileBase tile, Vector3Int gridPosition)
    {
        if (tile is CustomTileBase customTile)
        {
            // Verifica se há WaterTile adjacente
            if (customTile.HasNearbyWaterTile(gridPosition))
            {
                Debug.Log($"Há um rio próximo à posição {gridPosition}. Solo não pode ser arado.");
                return; 
                // Aqui precisa de um feedback pro jogador //
            }

            customTile.ChangeToPlowedState(gridPosition);
            Debug.Log($"Solo arado na posição: {gridPosition}");

            // Analytics
            AnalyticsSystem.AddAnalyticLands_Plowed(this.name, gridPosition);
        }
    }

    void UseInfoTool(Vector3Int gridPosition)
    {
        if (!tileInfoAnimator.GetBool("OpenInfo") && !isTouchProcessed)
        {
            DisplayTileInfo(gridPosition);
        }       
    }

    void UseHarvestTool(TileBase tile, Vector3Int gridPosition)
    {
        if (tile is PlantTile plantTile && plantTile.isFullyGrown)
        {
            plantTile.Collect(tilemap, gridPosition, playerInventory);

            // Analytics
            AnalyticsSystem.AddAnalyticPlants_Harvested(this.name, plantTile.name, 1);
        }
        else
        {
            Debug.Log("Tile Não Harvestable");
        }
    }

    void UsePlantTool(Vector3Int gridPosition)
    {
        if (inventoryManager.HasSelectedSeed())
        {
            TileInfo tileInfo = tilemapManager.GetTileInfo(gridPosition);

            // Verifica se o tile é plantável
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
                Debug.Log("Setting openinfo true");
                //ToolsManager.SetActiveTool(ToolsManager.Tools.None);

                // Analytics
                AnalyticsSystem.AddAnalyticInfo(this.name, "Consult INFO", new int[4] { tileInfo.nitrogen, tileInfo.phosphorus, tileInfo.potassium, tileInfo.humidity });
            }
        }
        else
        {
            Debug.Log("Nenhum tile na posição: " + gridPosition);
        }
    }

    public void HideTileInfo()
    {
        Debug.Log("Setting openinfo false");
        tileInfoAnimator.SetBool("OpenInfo", false);  // Ativa a animação de fechamento
        //ToolsManager.SetActiveTool(ToolsManager.Tools.Info);
        uiManager.HideTileInfo();
    }

    /*private void Update()
    {
        SelectTile();
    }*/
}
