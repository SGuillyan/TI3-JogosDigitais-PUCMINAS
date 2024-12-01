using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class StoreUI : MonoBehaviour
{
    public Store storeInventory;  // Referência ao inventário da loja
    public Inventory playerInventory;  // Referência ao inventário do jogador
    public GameObject storeItemPrefab;  // Prefab dos itens da loja
    public Transform contentParent;  // Onde os itens serão exibidos
    public GameObject storeUI;  // Referência à UI da loja
    public Animator storeAnimator;  // Animações para abrir/fechar a loja

    private List<GameObject> storeItemInstances = new List<GameObject>();
    private bool isVisible = true;

    private GameObject selectedItem; // Item atualmente selecionado
    private int selectedQuantity = 1; // Quantidade atualmente selecionada

    void Start()
    {
        ShowBuyItems();  // Inicialmente mostra os itens à venda
    }

    public void OpenStore()
    {
        storeUI.SetActive(true);  // Ativa a UI da loja
        ToggleStore(true);
    }

    public void CloseStore()
    {
        ToggleStore(false);
        StartCoroutine(DisableStoreAfterAnimation());  // Espera o fade-out antes de desativar
    }

    private void ToggleStore(bool show)
    {
        isVisible = show;
        storeAnimator.SetBool("IsVisible", show);
    }

    private IEnumerator DisableStoreAfterAnimation()
    {
        yield return new WaitForSeconds(storeAnimator.GetCurrentAnimatorStateInfo(0).length);

        if (!isVisible)
        {
            storeUI.SetActive(false);
        }
    }

    public void OnBuyItemSelected(int itemID)
    {
        int quantity = 1;  // Exemplo de quantidade; isso pode ser ajustado

        if (storeInventory.BuyItem(itemID, quantity))
        {
            ShowBuyItems();  // Atualiza a interface da loja após a compra
        }
    }

    public void OnSellItemSelected(int itemID)
    {
        int quantity = 1;  // Quantidade padrão para venda, você pode ajustar conforme necessário

        // Agora chamamos a função SellItem do Store
        if (storeInventory.SellItem(itemID, quantity))
        {
            ShowSellItems();  // Atualiza a interface da loja após a venda
        }
        else
        {
            Debug.Log("Não foi possível vender o item.");
        }
    }


    public void ShowBuyItems()
    {
        ClearStoreUI();

        // Agora utiliza diretamente o List<Item> para exibir os itens
        List<Item> shopItems = storeInventory.GetAllShopItems();

        foreach (Item item in shopItems)
        {
            CreateStoreItemUI(item, true);  // Passa true para indicar que estamos na aba de compra
        }
    }

    public void ShowSellItems()
    {
        ClearStoreUI();

        // Aqui pegamos os itens que o jogador tem no inventário e que podem ser vendidos
        List<Item> sellableItems = storeInventory.GetSellableItemsFromInventory();

        foreach (Item item in sellableItems)
        {
            // Usamos a função de criação de UI para mostrar os itens à venda
            CreateStoreItemUI(item, false);  // Passa false para indicar que estamos na aba de venda
        }
    }

    #region // Toggle

    public void ShowBuyItems(Toggle toggle)
    {
        if (toggle.isOn)
        {
            ClearStoreUI();

            // Agora utiliza diretamente o List<Item> para exibir os itens
            List<Item> shopItems = storeInventory.GetAllShopItems();

            foreach (Item item in shopItems)
            {
                CreateStoreItemUI(item, true);  // Passa true para indicar que estamos na aba de compra
            }
        } 
    }

    public void ShowSellItems(Toggle toggle)
    {
        if (toggle.isOn)
        {
            ClearStoreUI();

            // Aqui pegamos os itens que o jogador tem no inventário e que podem ser vendidos
            List<Item> sellableItems = storeInventory.GetSellableItemsFromInventory();

            foreach (Item item in sellableItems)
            {
                // Usamos a função de criação de UI para mostrar os itens à venda
                CreateStoreItemUI(item, false);  // Passa false para indicar que estamos na aba de venda
            }
        }
    }

    #endregion

    private void ClearStoreUI()
    {
        foreach (var item in storeItemInstances)
        {
            Destroy(item);
        }
        storeItemInstances.Clear();
    }

    private void CreateStoreItemUI(Item item, bool isBuyTab)
    {
        GameObject itemInstance = Instantiate(storeItemPrefab, contentParent);
        storeItemInstances.Add(itemInstance);

        Image itemIcon = itemInstance.transform.Find("ItemIcon").GetComponent<Image>();
        TextMeshProUGUI itemName = itemInstance.transform.Find("ItemName").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI itemPrice = itemInstance.transform.Find("ItemPrice").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI itemQuantity = itemInstance.transform.Find("ItemQuantity").GetComponent<TextMeshProUGUI>();
        GameObject quantityControls = itemInstance.transform.Find("QuantityControls").gameObject;

        // Exibe as informações do item
        itemIcon.sprite = item.itemIcon;
        itemName.text = item.itemName;

        if (isBuyTab)
        {
            itemPrice.text = item.price.ToString() + " coins";
            itemQuantity.text = "";
        }
        else
        {
            int quantity = playerInventory.GetItemQuantity(item);
            itemPrice.text = "$" + item.price.ToString();
            itemQuantity.text = "x" + quantity.ToString();
        }

        // Configura os botões
        Button selectButton = itemInstance.GetComponentInChildren<Button>();
        Button leftArrow = quantityControls.transform.Find("LeftArrow").GetComponent<Button>();
        Button rightArrow = quantityControls.transform.Find("RightArrow").GetComponent<Button>();
        Button confirmButton = quantityControls.transform.Find("ConfirmButton").GetComponent<Button>();
        TextMeshProUGUI selectedQuantityText = quantityControls.transform.Find("SelectedQuantity").GetComponent<TextMeshProUGUI>();

        int itemID = item.itemID;

        selectButton.onClick.AddListener(() =>
        {
            SelectItem(itemInstance, itemID, isBuyTab);
        });

        leftArrow.onClick.AddListener(() =>
        {
            AdjustQuantity(-1, selectedQuantityText);
        });

        rightArrow.onClick.AddListener(() =>
        {
            AdjustQuantity(1, selectedQuantityText);
        });

        confirmButton.onClick.AddListener(() =>
        {
            if (isBuyTab)
                OnBuyItemConfirmed(itemID, selectedQuantity);
            else
                OnSellItemConfirmed(itemID, selectedQuantity);
        });

        // Inicializa o controle de quantidade como inativo
        quantityControls.SetActive(false);
    }


    private void SelectItem(GameObject itemInstance, int itemID, bool isBuyTab)
    {
        // Desativa os controles do item anteriormente selecionado
        if (selectedItem != null)
        {
            selectedItem.transform.Find("QuantityControls").gameObject.SetActive(false);
            // Remove o estado visual de seleção do item anterior
            var previousButton = selectedItem.GetComponentInChildren<Button>();
            if (previousButton != null)
            {
                previousButton.image.color = Color.white; // Cor padrão
            }
        }

        // Ativa os controles para o novo item selecionado
        selectedItem = itemInstance;
        selectedQuantity = 1; // Reseta a quantidade selecionada
        selectedItem.transform.Find("QuantityControls").gameObject.SetActive(true);

        // Aplica o estado visual de seleção ao botão do item atual
        var currentButton = selectedItem.GetComponentInChildren<Button>();
        if (currentButton != null)
        {
            currentButton.image.color = Color.red; // Cor para destacar o botão selecionado
        }
    }


    private void AdjustQuantity(int amount, TextMeshProUGUI selectedQuantityText)
    {
        selectedQuantity = Mathf.Max(1, selectedQuantity + amount); // Garante que não seja menor que 1
        selectedQuantityText.text = selectedQuantity.ToString();
    }

    private void OnBuyItemConfirmed(int itemID, int quantity)
    {
        if (storeInventory.BuyItem(itemID, quantity))
        {
            ShowBuyItems();
        }
    }

    private void OnSellItemConfirmed(int itemID, int quantity)
    {
        if (storeInventory.SellItem(itemID, quantity))
        {
            ShowSellItems();
        }
    }


}
