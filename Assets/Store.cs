using System.Collections.Generic;
using UnityEngine;

public class Store : MonoBehaviour
{
    // Lista de itens da loja configurados no Inspector
    [Header("Itens Disponíveis na Loja")]
    public List<ShopItem> defaultShopItems;  // Lista de itens à venda, preenchida no Inspector

    private List<ShopItem> shopItems = new List<ShopItem>();

    void Start()
    {
        InitializeStore();  // Inicializa a loja com os itens configurados
    }

    // Inicializa a loja com os itens configurados no Inspector
    private void InitializeStore()
    {
        foreach (ShopItem item in defaultShopItems)
        {
            shopItems.Add(new ShopItem(item.item, item.price));
        }

        Debug.Log("Loja inicializada com " + shopItems.Count + " itens.");
    }

    // Método para obter todos os itens da loja
    public List<ShopItem> GetAllShopItems()
    {
        return shopItems;
    }

    // Método para obter um item da loja por ID
    public ShopItem GetShopItemByID(int itemID)
    {
        return shopItems.Find(i => i.item.itemID == itemID);
    }

    // Método para comprar um item
    public bool BuyItem(int itemID, int quantity, Inventory playerInventory, ref int playerCoins)
    {
        ShopItem shopItem = GetShopItemByID(itemID);
        if (shopItem != null && playerCoins >= (shopItem.price * quantity))
        {
            playerCoins -= shopItem.price * quantity;  // Deduz o valor dos itens das moedas do jogador
            playerInventory.AddItem(shopItem.item, quantity);  // Adiciona o item ao inventário do jogador
            Debug.Log("Item comprado: " + shopItem.item.itemName + " x" + quantity);
            return true;
        }
        else
        {
            Debug.Log("Moedas insuficientes ou item não encontrado.");
            return false;
        }
    }
}
