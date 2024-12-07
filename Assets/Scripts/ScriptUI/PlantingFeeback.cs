using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlantingFeeback : MonoBehaviour
{
    [Header("Access")]
    [SerializeField] private InventoryManager inventoryManager;
    [SerializeField] private TextMeshProUGUI display;
    [SerializeField] private Image image;

    private void Update()
    {
        if(inventoryManager.HasSelectedSeed())
        {
            display.text = inventoryManager.playerInventory.items[inventoryManager.GetSelectedSeedID()].quantity.ToString();
            image.sprite = inventoryManager.playerInventory.items[inventoryManager.GetSelectedSeedID()].item.itemIcon;
        }
    }
}
