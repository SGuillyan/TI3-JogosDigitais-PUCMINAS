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

    public MoneyManager moneyManager;

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

        AudioManager.PlaySound(SoundType.SCREENCLICK);
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

        AudioManager.PlaySound(SoundType.SCREENCLICK);
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

        Image itemIcon = itemInstance.transform.Find("Icon").Find("ItemIcon").GetComponent<Image>();
        //var itemIcon = itemInstance;
        //Debug.Log(itemIcon);
        TextMeshProUGUI itemName = itemInstance.transform.Find("ItemName").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI itemPrice = itemInstance.transform.Find("Price").Find("text_Price").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI itemQuantity = itemIcon.transform.Find("ItemQuantity").GetComponent<TextMeshProUGUI>();
        GameObject quantityControls = itemInstance.transform.Find("QuantityControls").gameObject;

        // Exibe as informações do item
        itemIcon.sprite = item.itemIcon;
        itemName.text = item.itemName;

        if (isBuyTab)
        {
            itemPrice.text = item.price.ToString();
            itemQuantity.text = "";
        }
        else
        {
            int quantity = playerInventory.GetItemQuantity(item);
            itemPrice.text = item.price.ToString();
            itemQuantity.text = "x" + quantity.ToString();
        }

        // Configura os botões
        Button selectButton = itemInstance.GetComponentInChildren<Button>();
        Button leftArrow = quantityControls.transform.Find("Quantity").Find("LeftArrow").GetComponent<Button>();
        Button rightArrow = quantityControls.transform.Find("Quantity").Find("RightArrow").GetComponent<Button>();
        Button confirmButton = quantityControls.transform.Find("ConfirmButton").GetComponent<Button>();
        TextMeshProUGUI selectedQuantityText = quantityControls.transform.Find("Quantity").Find("SelectedQuantity").GetComponent<TextMeshProUGUI>();

        int itemID = item.itemID;


        // Atribuindo funções para os botões
        selectButton.onClick.AddListener(() =>
        {
            SelectItem(itemInstance, itemID, isBuyTab);

            AudioManager.PlaySound(SoundType.SCREENCLICK);
        });

        leftArrow.onClick.AddListener(() =>
        {
            AdjustQuantity(-1, selectedQuantityText, item, isBuyTab); // Passa o item diretamente
            itemPrice.text = (item.price * selectedQuantity).ToString();

            AudioManager.PlaySound(SoundType.SCREENCLICK);
        });

        rightArrow.onClick.AddListener(() =>
        {
            AdjustQuantity(1, selectedQuantityText, item, isBuyTab); // Passa o item diretamente
            itemPrice.text = (item.price * selectedQuantity).ToString();

            AudioManager.PlaySound(SoundType.SCREENCLICK);
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
        // Verifica se o item selecionado é o mesmo que o item já selecionado
        if (selectedItem != itemInstance)
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
            selectedQuantity = 1; // Reseta a quantidade selecionada para 1
            var quantityControls = selectedItem.transform.Find("QuantityControls").gameObject;
            quantityControls.SetActive(true);

            // Atualiza o texto da quantidade inicial para o controle selecionado
            TextMeshProUGUI selectedQuantityText = itemInstance.transform.Find("Icon").Find("ItemIcon").Find("ItemQuantity").GetComponent<TextMeshProUGUI>();
            if (selectedQuantityText != null)
            {
                selectedQuantityText.text = selectedQuantity.ToString(); // Garante que começa em 1
            }

            // Aplica o estado visual de seleção ao botão do item atual
            var currentButton = selectedItem.GetComponentInChildren<Button>();
            if (currentButton != null)
            {
                currentButton.image.color = Color.red; // Cor para destacar o botão selecionado
            }
        }
        else
        {
            // Caso o item selecionado já esteja selecionado, não altera a quantidade
            var quantityControls = selectedItem.transform.Find("QuantityControls").gameObject;
            if (quantityControls.activeSelf)
            {
                quantityControls.SetActive(false); // Desativa os controles, se necessário
            }
            else
            {
                quantityControls.SetActive(true); // Caso contrário, ativa os controles
            }
        }
    }



    private void AdjustQuantity(int amount, TextMeshProUGUI selectedQuantityText, Item item, bool isBuyTab)
    {
        // Ajusta a quantidade inicial, mas nunca permite que seja menor que 1
        int maxQuantity = int.MaxValue;

        if (item != null)
        {
            if (isBuyTab)
            {
                // Aba de compra: restrição com base no dinheiro disponível
                maxQuantity = Mathf.FloorToInt((float)moneyManager.GetCurrentMoney() / item.price);
            }
            else
            {
                // Aba de venda: restrição com base na quantidade no inventário
                maxQuantity = playerInventory.GetItemQuantity(item);
            }
        }

        // Restringe a quantidade ajustada ao intervalo permitido
        selectedQuantity = Mathf.Clamp(selectedQuantity + amount, 1, maxQuantity);

        // Atualiza o texto no controle de quantidade
        if (selectedQuantityText != null)
        {
            selectedQuantityText.text = selectedQuantity.ToString();
        }
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
