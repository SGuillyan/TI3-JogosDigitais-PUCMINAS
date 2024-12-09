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

    private int id;

    private void Update()
    {
        if (id > -1)
        {
            if (inventoryManager.HasSelectedItem())
            {
                // Debug.Log(id);
                display.text = inventoryManager.playerInventory.items[id].quantity.ToString();
                image.sprite = inventoryManager.playerInventory.items[id].item.itemIcon;
                
            }
        }  
    }

    public void ActiveFeedback(int ID)
    {
        id = ID;
        gameObject.SetActive(true);
    }

    public void DisableFeedback()
    {
        id = -2;
        gameObject.SetActive(false);
    }
}
