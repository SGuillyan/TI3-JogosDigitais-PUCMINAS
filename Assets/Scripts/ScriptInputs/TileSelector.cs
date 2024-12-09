using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class TileSelector : MonoBehaviour
{
    public Tilemap tilemap;
    public GameObject debugObjectPrefab;
    public InventoryManager inventoryManager;
    public MoneyManager moneyManager;
    public TilemapPlant tilemapPlant;
    public TilemapFertilize tilemapFertilize;
    public InventoryUI inventoryUI;
    public TilemapManager tilemapManager;
    public RewardManager rewardManager;
    public UIManager uiManager;
    [SerializeField] private List<GameObject> uiPanels;
    public Inventory playerInventory;
    public Animator tileInfoAnimator;  // Animator para a janela de informações
    public CustomTileBase soilTile;
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
            //Debug.Log("UI ABERTA TOQUE INTERROMPIDO");
            return;
        }

        if (Input.touchCount > 0)
        {
            if (MenuManager.openedMenu)
            {
                //Debug.Log("UI ABERTA TOQUE INTERROMPIDO 2");
                continuoMenuTimer = continuott;
                return;
            }
            if (continuoMenuTimer > 0f)
            {
                //Debug.Log("UI ABERTA TOQUE INTERROMPIDO 3");
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
                        UseFlattenTool(clickedTile, gridPosition);
                        break;
                    case ToolsManager.Tools.Harvest:
                        UseHarvestTool(clickedTile, gridPosition);
                        break;
                    case ToolsManager.Tools.Info:
                        UseInfoTool(gridPosition);
                        //Debug.Log("USING INFO");
                        break;
                    case ToolsManager.Tools.CutDown:
                        UseCutDownTool(gridPosition);
                        //Debug.Log("USING INFO");
                        break;
                    case ToolsManager.Tools.Plant:
                        UsePlantTool(gridPosition);
                        break;
                    case ToolsManager.Tools.Fertilize:
                        UseFertilizeTool(gridPosition);
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
            //Debug.Log($"Solo arado na posição: {gridPosition}");

            AudioManager.PlaySound(SoundType.HOE);

            // Analytics
            AnalyticsSystem.AddAnalyticLands_Plowed(this.name, gridPosition);
        }
    }

    void UseInfoTool(Vector3Int gridPosition)
    {
        if (!tileInfoAnimator.GetBool("OpenInfo") && !isTouchProcessed)
        {
            DisplayTileInfo(gridPosition);
            AudioManager.PlaySound(SoundType.SCREENCLICK);
        }       
    }

    void UseCutDownTool(Vector3Int gridPosition)
    {
        // Obtém o tile na posição
        TileBase tile = tilemap.GetTile(gridPosition);

        if (tile is TreeTile treeTile)
        {
            // Deduz dinheiro do jogador antes de qualquer transformação
            int costToCutDown = 50; // Quantidade de dinheiro a ser deduzida
            if (!moneyManager.SpendMoney(costToCutDown))
            {
                //Debug.Log("Dinheiro insuficiente para cortar a árvore.");
                return; // Interrompe se o jogador não tiver dinheiro suficiente
            }

            // Remove a árvore atual e transforma o tile em CustomTileBase
            TilemapManager tilemapManager = Object.FindObjectOfType<TilemapManager>();
            
            if (tilemapManager != null)
            {
                treeTile.CutDown(gridPosition);

                IDS.ReduceEcologico(1);


                /*tilemap.SetTile(gridPosition, soilTile);

                // Atualiza o TileInfo no TilemapManager
                TileInfo tileInfo = new TileInfo(
                    soilTile.isPlantable,
                    soilTile.nitrogen,
                    soilTile.phosphorus,
                    soilTile.potassium,
                    soilTile.humidity
                );
                tilemapManager.SetTileInfo(gridPosition, tileInfo);*/

                //Debug.Log($"Árvore cortada e tile transformado na posição: {gridPosition}");
            }

        }
        else
        {
           // Debug.Log("O tile selecionado não é uma árvore.");
        }
    }


    void UseFlattenTool(TileBase tile, Vector3Int gridPosition)
    {
        if (tile is CustomTileBase customTile)
        {
            customTile.RevertToCustomTileState(gridPosition);
            //Debug.Log($"Solo alisado na posição: {gridPosition}");

            // Analytics
            // AnalyticsSystem.AddAnalyticLands_Plowed(this.name, gridPosition);
        }    
    }

    void UseFertilizeTool(Vector3Int gridPosition)
    {
        if (inventoryManager.HasSelectedItem())
        {
            int itemID = inventoryManager.GetSelectedItemID();
            {
                if(itemID>= 200 && itemID <= 299){
                    TileInfo tileInfo = tilemapManager.GetTileInfo(gridPosition);

                    // Verifica se o tile é plantável
                    if (tileInfo != null && tileInfo.isPlantable)
                    {
                        tilemapFertilize.FertilizeAt(gridPosition, itemID);
                        inventoryManager.UseItemAt(gridPosition);

                        AudioManager.PlaySound(SoundType.HARVESTABLEPLANT);
                    }

                    else
                    {
                        //Debug.Log("O tile na posição " + gridPosition + " não é plantável.");
                    } 
                }          
            }
        }     
    }

    void UseHarvestTool(TileBase tile, Vector3Int gridPosition)
    {
        if (tile is PlantTile plantTile && plantTile.isFullyGrown)
        {
            plantTile.Collect(tilemap, gridPosition, playerInventory);

            AudioManager.PlaySound(SoundType.HARVEST);

            // Analytics
            AnalyticsSystem.AddAnalyticPlants_Harvested(this.name, plantTile.name, 1);
        }
        else
        {
            //Debug.Log("Tile Não Harvestable");
        }
    }

    void UsePlantTool(Vector3Int gridPosition)
    {
        if (inventoryManager.HasSelectedItem())
        {
            int itemID = inventoryManager.GetSelectedItemID();
            {
                if(itemID>= 0 && itemID <= 99){
                    TileInfo tileInfo = tilemapManager.GetTileInfo(gridPosition);

                    // Verifica se o tile é plantável
                    if (tileInfo != null && tileInfo.isPlantable)
                    {
                        tilemapPlant.PlantSeedAt(gridPosition, itemID);
                        inventoryManager.UseItemAt(gridPosition);

                        AudioManager.PlaySound(SoundType.PLANT);
                    }
                    else
                    {
                       // Debug.Log("O tile na posição " + gridPosition + " não é plantável.");
                    } 
                }          
            }
        }
    }



    void DisplayTileInfo(Vector3Int gridPosition)
    {
        TileBase clickedTile = tilemap.GetTile(gridPosition);

        if (clickedTile != null)
        {
           // Debug.Log("Tipo de tile clicado: " + clickedTile.GetType().FullName);

            TileInfo tileInfo = tilemapManager.GetTileInfo(gridPosition);

            if (tileInfo != null || clickedTile is TreeTile)
            {
                if (clickedTile is CustomTileBase tileData)
                {
                    uiManager.UpdateTileInfo(tileData.sprite, tileInfo.nitrogen, tileInfo.phosphorus, tileInfo.potassium, tileInfo.humidity, tileInfo.isPlantable);
                }
                else if (clickedTile is PlantTile tileData2)
                {
                    uiManager.UpdateTileInfo(tileData2.sprite, tileInfo.nitrogen, tileInfo.phosphorus, tileInfo.potassium, tileInfo.humidity, tileInfo.isPlantable);
                }
                else if (clickedTile is TreeTile tileData3)
                {
                    uiManager.UpdateTileInfo(tileData3.sprite, tileData3.nitrogen, tileData3.phosphorus, tileData3.potassium, tileData3.humidity, tileData3.isPlantable);
                }

                tileInfoAnimator.SetBool("OpenInfo", true);  // Ativa a animação de abertura
                //Debug.Log("Setting openinfo true");
                //ToolsManager.SetActiveTool(ToolsManager.Tools.None);

                // Analytics
                if(tileInfo != null){
                    AnalyticsSystem.AddAnalyticInfo(this.name, "Consult INFO", new int[4] { tileInfo.nitrogen, tileInfo.phosphorus, tileInfo.potassium, tileInfo.humidity });
                }
            }
        }
        else
        {
            Debug.Log("Nenhum tile na posição: " + gridPosition);
        }
    }

    public void HideTileInfo()
    {
        //Debug.Log("Setting openinfo false");
        tileInfoAnimator.SetBool("OpenInfo", false);  // Ativa a animação de fechamento
        //ToolsManager.SetActiveTool(ToolsManager.Tools.Info);
        uiManager.HideTileInfo();

        AudioManager.PlaySound(SoundType.SCREENCLICK);
    }

    /*private void Update()
    {
        SelectTile();
    }*/
}
