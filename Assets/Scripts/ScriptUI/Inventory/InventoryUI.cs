using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class InventoryUI : MonoBehaviour
{
    public Inventory playerInventory;
    public GameObject inventoryItemPrefab;
    public Transform contentParent;
    public GameObject inventoryUI;
    public InventoryManager inventoryManager;
    public Animator inventoryAnimator;

    private List<GameObject> inventoryItemInstances = new List<GameObject>();
    private bool isVisible = true;

    void Start()
    {
        playerInventory.onInventoryChanged += UpdateInventoryUI;
        //ShowSeeds();
        UpdateInventoryUI();
    }

    void OnDestroy()
    {
        playerInventory.onInventoryChanged -= UpdateInventoryUI;
    }

    public void OpenInventory()
    {
        inventoryUI.SetActive(true);  // Ativa o inventário
        ToggleInventory(true);
    }

    public void CloseInventory()
    {
        ToggleInventory(false);
        StartCoroutine(DisableInventoryAfterAnimation());  // Espera o fade-out antes de desativar
    }

    private void ToggleInventory(bool show)
    {
        isVisible = show;
        inventoryAnimator.SetBool("IsVisible", show);
    }

    // Coroutine para desativar o inventário após a animação de fade-out
    private IEnumerator DisableInventoryAfterAnimation()
    {
        // Espera até que a animação atual seja finalizada
        yield return new WaitForSeconds(inventoryAnimator.GetCurrentAnimatorStateInfo(0).length);

        // Desativa o inventário após o fade-out
        if (!isVisible)
        {
            inventoryUI.SetActive(false);
        }
    }

    public void OnSeedSelected(int itemID)
    {
        inventoryManager.SelectSeed(itemID);
        //CloseInventory();
    }

    public void OnAbaClicked()
    {
        if (!isVisible)
        {
            OpenInventory();
        }
    }

    public void UpdateInventoryUI()
    {
        ClearInventoryUI();

        foreach (InventoryItem inventoryItem in playerInventory.items)
        {
            CreateInventoryItemUI(inventoryItem);
        }
    }

    private void ClearInventoryUI()
    {
        foreach (var item in inventoryItemInstances)
        {
            Destroy(item);
        }
        inventoryItemInstances.Clear();
    }

    private void CreateInventoryItemUI(InventoryItem inventoryItem)
    {
        GameObject itemInstance = Instantiate(inventoryItemPrefab, contentParent);
        inventoryItemInstances.Add(itemInstance);

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

        Button selectButton = itemInstance.GetComponentInChildren<Button>();
        if (selectButton != null)
        {
            int itemID = inventoryItem.item.itemID;
            selectButton.onClick.AddListener(() => OnSeedSelected(itemID));
        }
        else
        {
            Debug.LogError("Botão de seleção não encontrado no prefab do item de inventário.");
        }
    }

    public void UpdateInventoryUIByType(ItemType itemType)
    {
        ClearInventoryUI();

        List<InventoryItem> filteredItems = playerInventory.GetItemsByType(itemType);

        foreach (InventoryItem inventoryItem in filteredItems)
        {
            CreateInventoryItemUI(inventoryItem);
        }
    }

    public void ShowAllItems()
    {
        UpdateInventoryUI();
    }

    #region // Private comentado

    /*private void ShowCollectedItems()
    {

        UpdateInventoryUIByType(ItemType.CollectedItem);
    }

    private void ShowSeeds()
    {
        UpdateInventoryUIByType(ItemType.Seed);
    }

    private void ShowFertilizers()
    {
        UpdateInventoryUIByType(ItemType.Fertilizer);
    }*/

    #endregion

    #region // Toggle

    public void ShowCollectedItems(Toggle toggle)
    {
        if (toggle.isOn)
        {
            UpdateInventoryUIByType(ItemType.CollectedItem);
        }
    }

    public void ShowSeeds(Toggle toggle)
    {
        if (toggle.isOn)
        {
            UpdateInventoryUIByType(ItemType.Seed);
        }
    }

    public void ShowFertilizers(Toggle toggle)
    {
        if (toggle.isOn)
        {
            UpdateInventoryUIByType(ItemType.Fertilizer);
        }
    }

    #endregion
}
