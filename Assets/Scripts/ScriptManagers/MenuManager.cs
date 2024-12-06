using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    private enum Menu
    {
        None,
        ToolBox,
        QuestList,
        Inventory,
        Shop,
        IDS_Menu,
        Config,
        Credit,
    }

    [Header("Scripts")]

    [SerializeField] private ToolsManager toolsManager;
    [SerializeField] private TaskManager taskManager;
    [SerializeField] private InventoryUI inventoryUI;
    [SerializeField] private StoreUI storeUI;

    [Header("UI GameObjects")]

    [SerializeField] private GameObject questCanvas;
    [SerializeField] private GameObject inventory;
    [SerializeField] private GameObject shop;

    [SerializeField] private GameObject idsMenu;
    [SerializeField] private GameObject config;
    [SerializeField] private GameObject credit;


    public static bool openedMenu = false;

    #region // Tab

    public void TabToolBox()
    {
        if (ToolsManager.isToolBoxOpen)
        {
            CloseToolBox();
            //isToolBoxOpen = false;
        }
        else
        {
            OpenToolBox();
            //isToolBoxOpen = true;
        }
    }

    public void TabQuestList()
    {
        if (questCanvas.activeSelf) CloseQuestList(); 
        else OpenQuestList(); 
    }

    public void TabInventory()
    {
        if (inventory.activeSelf) CloseInventory(); 
        else OpenInventory(); 
    }

    public void TabShop()
    {
        if (shop.activeSelf) CloseShop(); 
        else OpenShop(); 
    }

    public void TabIDS_Menu()
    {
        if (idsMenu.activeSelf) CloseIDS_Menu();
        else OpenIDS_Menu();
    }

    public void TabConfig()
    {
        if (config.activeSelf) CloseConfig();
        else OpenConfig();
    }

    public void TabCredit()
    {
        if (credit.activeSelf) CloseCredit();
        else OpenCredit();
    }

    #endregion

    #region // Open

    private void OpenToolBox()
    {
        VerifyTabs(Menu.ToolBox);
        ToolsManager.ToolBoxAnim();

        openedMenu = true;
        CameraController.lockCamera = true;
    }

    private void OpenQuestList()
    {
        VerifyTabs(Menu.QuestList);
        taskManager.ShowQuestCanvas();

        openedMenu = true;
        CameraController.lockCamera = true;
    }

    public void OpenInventory()
    {
        VerifyTabs(Menu.Inventory);
        inventoryUI.OpenInventory();

        openedMenu = true;
        CameraController.lockCamera = true;
    }

    private void OpenShop()
    {
        VerifyTabs(Menu.Shop);
        storeUI.OpenStore();

        openedMenu = true;
        CameraController.lockCamera = true;
    }

    private void OpenIDS_Menu()
    {
        VerifyTabs(Menu.IDS_Menu);
        idsMenu.SetActive(true);

        openedMenu = true;
        CameraController.lockCamera = true;
    }

    private void OpenConfig()
    {
        VerifyTabs(Menu.Config);
        config.SetActive(true);

        openedMenu = true;
        CameraController.lockCamera = true;
    }

    private void OpenCredit()
    {
        VerifyTabs(Menu.Config);
        credit.SetActive(true);

        openedMenu = true;
        CameraController.lockCamera = true;
    }

    #endregion

    #region // Close

    public void CloseToolBox()
    {
        ToolsManager.ToolBoxAnim();

        openedMenu = false;
        CameraController.lockCamera = false;
    }

    private void CloseQuestList()
    {
        taskManager.HideQuestCanvas();

        openedMenu = false;
        CameraController.lockCamera = false;
    }

    private void CloseInventory()
    {
        inventoryUI.CloseInventory();

        openedMenu = false;
        CameraController.lockCamera = false;
    }

    public void CloseInventoryToPlant()
    {
        inventoryUI.CloseInventory();

        openedMenu = false;
        CameraController.lockCamera = true;
    }

    private void CloseShop()
    {
        storeUI.CloseStore();

        openedMenu = false;
        CameraController.lockCamera = false;
    }

    private void CloseIDS_Menu()
    {
        idsMenu.SetActive(false);

        openedMenu = false;
        CameraController.lockCamera = false;
    }

    private void CloseConfig()
    {
        VerifyTabs(Menu.Config);
        config.SetActive(false);

        openedMenu = false;
        CameraController.lockCamera = false;
    }

    private void CloseCredit()
    {
        credit.SetActive(false);

        openedMenu = false;
        CameraController.lockCamera = false;
    }

    #endregion

    private void VerifyTabs(Menu tab)
    {
        if (tab != Menu.ToolBox)
        {
            if (ToolsManager.isToolBoxOpen)
            {
                CloseToolBox();
            }
        }
        if (tab != Menu.QuestList)
        {
            CloseQuestList();
        }
        if (tab != Menu.Inventory)
        {
            if (inventory.activeSelf)
            {
                CloseInventory();
            }
        }
        if (tab != Menu.Shop)
        {
            if (shop.activeSelf)
            {
                CloseShop();
            }
        }

        if (tab != Menu.IDS_Menu)
        {
            if (idsMenu.activeSelf)
            {
                CloseIDS_Menu();
            }
        }
        if (tab != Menu.Config)
        {
            if (config.activeSelf)
            {
                CloseConfig();
            }
        }
        if (tab != Menu.Credit)
        {
            if (credit.activeSelf)
            {
                CloseCredit();
            }
        }
    }
}
