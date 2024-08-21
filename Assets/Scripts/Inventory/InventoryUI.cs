using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

public class InventoryUI : MonoBehaviour
{
    public Inventory playerInventory;  // Referência ao inventário do jogador
    public GameObject inventoryItemPrefab;  // Prefab que será instanciado para cada item
    public Transform contentParent;  // O transform onde os itens serão instanciados (Content do Scroll View)
    public GameObject inventoryUI;  // Referência ao painel de UI do inventário
    public InventoryManager inventoryManager;  // Referência ao InventoryManager para manipular a seleção de sementes

    private List<GameObject> inventoryItemInstances = new List<GameObject>();

    void Start()
    {
        // Inscreve o método UpdateInventoryUI no evento onInventoryChanged
        playerInventory.onInventoryChanged += UpdateInventoryUI;

        // Inicializa a UI do inventário
        UpdateInventoryUI();
    }

    void OnDestroy()
    {
        // Desinscreve o evento ao destruir o objeto para evitar memory leaks
        playerInventory.onInventoryChanged -= UpdateInventoryUI;
    }

    // Método para abrir o inventário
    public void OpenInventory()
    {
        inventoryUI.SetActive(true);  // Ativa o painel do inventário
        UpdateInventoryUI();  // Atualiza a UI do inventário ao abrir
    }

    // Método para fechar o inventário
    public void CloseInventory()
    {
        inventoryUI.SetActive(false);  // Desativa o painel do inventário
    }

    // Método para atualizar a UI do inventário
    public void UpdateInventoryUI()
    {
        ClearInventoryUI();

        // Para cada item no inventário, cria uma entrada na UI
        foreach (InventoryItem inventoryItem in playerInventory.items)
        {
            CreateInventoryItemUI(inventoryItem);
        }
    }

    // Método para limpar os itens da UI do inventário
    private void ClearInventoryUI()
    {
        foreach (var item in inventoryItemInstances)
        {
            Destroy(item);
        }
        inventoryItemInstances.Clear();
    }

    // Método para criar uma entrada de item na UI do inventário
    private void CreateInventoryItemUI(InventoryItem inventoryItem)
    {
        GameObject itemInstance = Instantiate(inventoryItemPrefab, contentParent);
        inventoryItemInstances.Add(itemInstance);

        // Configura os dados do item na UI
        Image itemIcon = itemInstance.transform.Find("ItemIcon").GetComponent<Image>();
        TextMeshProUGUI itemName = itemInstance.transform.Find("ItemName").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI itemQuantity = itemInstance.transform.Find("ItemQuantity").GetComponent<TextMeshProUGUI>();

        if (itemIcon != null && itemName != null && itemQuantity != null)
        {
            itemIcon.sprite = inventoryItem.item.itemIcon;
            itemName.text = inventoryItem.item.itemName;
            itemQuantity.text = "x" + inventoryItem.quantity.ToString();
        }
        else
        {
            Debug.LogError("Erro ao encontrar componentes no prefab do item de inventário.");
        }

        // Adiciona um listener ao botão de seleção do item usando o ID do item
        Button selectButton = itemInstance.GetComponentInChildren<Button>();
        if (selectButton != null)
        {
            int itemID = inventoryItem.item.itemID;  // Captura o ID do item
            selectButton.onClick.AddListener(() => inventoryManager.SelectSeed(itemID));
        }
        else
        {
            Debug.LogError("Botão de seleção não encontrado no prefab do item de inventário.");
        }
    }

    // Método para filtrar itens por tipo e atualizar a UI
    public void UpdateInventoryUIByType(ItemType itemType)
    {
        ClearInventoryUI();

        List<InventoryItem> filteredItems = playerInventory.GetItemsByType(itemType);

        foreach (InventoryItem inventoryItem in filteredItems)
        {
            CreateInventoryItemUI(inventoryItem);
        }
    }

    // Métodos para filtrar por tipos específicos
    public void ShowAllItems()
    {
        UpdateInventoryUI();
    }

    public void ShowCollectedItems()
    {
        UpdateInventoryUIByType(ItemType.CollectedItem);
    }

    public void ShowSeeds()
    {
        UpdateInventoryUIByType(ItemType.Seed);
    }

    public void ShowFertilizers()
    {
        UpdateInventoryUIByType(ItemType.Fertilizer);
    }
}
