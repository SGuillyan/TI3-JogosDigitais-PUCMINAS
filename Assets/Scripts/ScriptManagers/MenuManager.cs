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

    //private ToolsManager toolBoxScript;
    //private GameObject questListScript;
    //private GameObject inventoryScript;
    //private GameObject shopScript;

    private void Start()
    {
        /*if (toolBoxUI != null)
        {
            toolBoxScript = toolBoxUI.GetComponent<ToolsManager>();
        }
        else
        {
            Debug.LogError("ToolBox ausente!");
        }*/


    }

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
    }

    private void OpenQuestList()
    {
        VerifyTabs(Menu.QuestList);
        taskManager.ShowQuestCanvas();
    }

    private void OpenInventory()
    {
        VerifyTabs(Menu.Inventory);
        inventoryUI.OpenInventory();
    }

    private void OpenShop()
    {
        VerifyTabs(Menu.Shop);
        storeUI.OpenStore();
    }

    #endregion

    #region // Close

    private void CloseToolBox()
    {
        ToolsManager.ToolBoxAnim();
    }

    private void CloseQuestList()
    {
        taskManager.HideQuestCanvas();
    }

    private void CloseInventory()
    {
        inventoryUI.CloseInventory();
    }

    private void CloseShop()
    {
        storeUI.CloseStore();
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
