using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_AddPlants : MonoBehaviour
{
    [SerializeField] private Inventory playerInventory;
    [SerializeField] private PlantTile item1;
    [SerializeField] private PlantTile item2;
    [SerializeField] private PlantTile item3;

    [Space(20)]

    [SerializeField] private AmbientVFX_Controller controller;
    [SerializeField] private Ambient.Temperature temperature;
    [SerializeField] private Ambient.Climate climate;

    [Space(20)]

    [SerializeField] private Ambient.Temperature currentTemperature;
    [SerializeField] private Ambient.Climate currentClimate;

    private void Update()
    {
        if (Input.touchCount >= 2)
        {
            playerInventory.AddItem(item1.harvestedItem, 1);
            playerInventory.AddItem(item2.harvestedItem, 1);
            playerInventory.AddItem(item3.harvestedItem, 1);
            Debug.Log(item1.harvestedItem.itemName + "ADICIONADO (5)");
        }

        currentTemperature = Ambient.GetCurrentTemperature();
        currentClimate = Ambient.GetCurrentClimate();

    }

    public void Acionar()
    {
        Ambient.SetCurrentTemperature(temperature);
        Ambient.SetCurrentClimate(climate);

        controller.ChangeWeather();
    }
}
