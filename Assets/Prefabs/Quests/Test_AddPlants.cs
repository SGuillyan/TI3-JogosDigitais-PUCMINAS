using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_AddPlants : MonoBehaviour
{
    [SerializeField] private Inventory playerInventory;
    [SerializeField] private PlantTile item;

    private void Update()
    {
        if (Input.touchCount >= 2)
        {
            playerInventory.AddItem(item.harvestedItem, 5);
            Debug.Log(item.harvestedItem.itemName + "ADICIONADO (5)");
        }
    }
}
