using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MainScreenUI : MonoBehaviour
{

    // Reference to your UXML file
    public VisualTreeAsset visualTreeAsset;

    void OnEnable()
    {
        // Get the root VisualElement
        var root = GetComponent<UIDocument>().rootVisualElement;

        // Find the button by name and register a click event on the button
        Button inventoryButton = root.Q<Button>("btnInventory");

        inventoryButton.clicked += () =>
        {
            Debug.Log("Inventory opened!");
        };
        
        Button shopButton = root.Q<Button>("btnShop");

        shopButton.clicked += () =>
        {
            Debug.Log("Shop opened!");
        };

        Button configButton = root.Q<Button>("btnConfig");

        configButton.clicked += () =>
        {
            Debug.Log("Config Screen opened!");
        };
    }
}
