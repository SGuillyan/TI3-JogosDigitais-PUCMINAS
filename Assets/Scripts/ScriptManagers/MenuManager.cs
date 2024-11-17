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

    public static bool openedMenu = false;

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
#pragma warning disable CS0618 // O tipo ou membro é obsoleto
        if (questCanvas.active)
        {
            CloseQuestList();
        }
        else
        {
            OpenQuestList();
        }
    }

    public void TabInventory()
    {
        if (inventory.active)
        {
            CloseInventory();
        }
        else
        {
            OpenInventory();
        }
    }

    public void TabShop()
    {
        if (shop.active)
        {
            CloseShop();
        }
        else
        {
            OpenShop();
        }
    }

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

    private void OpenInventory()
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

    #endregion

    #region // Close

    private void CloseToolBox()
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

    private void CloseShop()
    {
        storeUI.CloseStore();

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
            if (inventory.active)
            {
                CloseInventory();
            }
        }
        if (tab != Menu.Shop)
        {
            if (shop.active)
            {
                CloseShop();
            }
        }
    }
}
