using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

public class InventoryUI : MonoBehaviour
{
    public Inventory playerInventory;  // Referência ao inventário do jogador
    public GameObject inventoryItemPrefab;  // Prefab que será instanciado para cada item
    public Transform contentParent;  // O transform onde os itens serão instanciados (Content do Scroll View)

    public GameObject panel;  // Referência ao painel que deseja ativar/desativar
    public GameObject testButtons;  // Referência ao grupo de botões de teste que deseja ativar/desativar

    private List<GameObject> inventoryItemInstances = new List<GameObject>();

    void Start()
    {
        // Inscreve no evento de mudança no inventário
        playerInventory.onInventoryChanged += UpdateInventoryUI;

        // Inicializa a UI do inventário
        UpdateInventoryUI();
    }

    // Método para atualizar a UI do inventário (mostrando todos os itens)
    public void UpdateInventoryUI()
    {
        DisplayItems(playerInventory.items);
    }

    // Método para exibir itens filtrados na UI
    public void DisplayItems(List<InventoryItem> itemsToDisplay)
    {
        // Limpa os itens antigos
        foreach (var item in inventoryItemInstances)
        {
            Destroy(item);
        }
        inventoryItemInstances.Clear();

        // Para cada item no inventário, cria uma entrada na UI
        foreach (InventoryItem inventoryItem in itemsToDisplay)
        {
            GameObject itemInstance = Instantiate(inventoryItemPrefab, contentParent);
            inventoryItemInstances.Add(itemInstance);

            // Configura os dados do item na UI
            Image itemIcon = itemInstance.transform.Find("ItemIcon").GetComponent<Image>();
            TextMeshProUGUI itemName = itemInstance.transform.Find("ItemName").GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI itemQuantity = itemInstance.transform.Find("ItemQuantity").GetComponent<TextMeshProUGUI>();

            itemIcon.sprite = inventoryItem.item.itemIcon;
            itemName.text = inventoryItem.item.itemName;
            itemQuantity.text = "x" + inventoryItem.quantity.ToString();
        }
    }

    // Método para exibir todos os itens
    public void ShowAllItems()
    {
        DisplayItems(playerInventory.items);
    }

    // Método para exibir apenas itens coletados
    public void ShowCollectedItems()
    {
        DisplayItems(playerInventory.GetCollectedItems());
    }

    // Método para exibir apenas sementes
    public void ShowSeeds()
    {
        DisplayItems(playerInventory.GetSeeds());
    }

    // Método para exibir apenas fertilizantes
    public void ShowFertilizers()
    {
        DisplayItems(playerInventory.GetFertilizers());
    }

    // Método para ativar o Panel e os TestButtons
    public void ActivateUIElements()
    {
        panel.SetActive(true);
        testButtons.SetActive(true);
    }

    // Método para desativar o Panel e os TestButtons
    public void DeactivateUIElements()
    {
        panel.SetActive(false);
        testButtons.SetActive(false);
    }
}
