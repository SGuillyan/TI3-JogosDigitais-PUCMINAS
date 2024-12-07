using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_AddPlants : MonoBehaviour
{
    [SerializeField] private Inventory playerInventory;
    [SerializeField] private Item item;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            playerInventory.AddItem(item, 5);
        }
    }
}
