using System;
using UnityEngine;
using TMPro;

public class PlantingFeeback : MonoBehaviour
{
    [Header("Access")]
    [SerializeField] private InventoryManager inventoryManager;
    [SerializeField] private TextMeshProUGUI display;

    private void Update()
    {
        if(inventoryManager.HasSelectedSeed())
        {
            display.text = inventoryManager.playerInventory.items[inventoryManager.GetSelectedSeedID()].quantity.ToString();
        }
    }
}
