using System.Collections.Generic;
using UnityEngine;

public class Store : MonoBehaviour
{
    [Header("Itens Disponíveis na Loja")]
    public List<Item> defaultShopItems;  // Lista de itens à venda, preenchida no Inspector

    public Inventory playerInventory;
    public MoneyManager moneyManager;

    void Start()
    {
        InitializeStore();  // Inicializa a loja com os itens configurados
    }

    // Inicializa a loja com os itens configurados no Inspector
    private void InitializeStore()
    {
        //Debug.Log("Loja inicializada com " + defaultShopItems.Count + " itens.");
    }

    // Método para obter todos os itens da loja
    public List<Item> GetAllShopItems()
    {
        return defaultShopItems;  // Retorna a lista de itens na loja
    }

    // Método para comprar um item
    public bool BuyItem(int itemID, int quantity)
    {
        Item shopItem = defaultShopItems.Find(i => i.itemID == itemID);
        int totalCost = shopItem.price * quantity;

        // Verifica se o item está disponível e se o jogador tem dinheiro suficiente
        if (shopItem != null && moneyManager.SpendMoney(totalCost))  // Usar o MoneyManager como singleton
        {
            playerInventory.AddItem(shopItem, quantity);  // Adiciona o item ao inventário do jogador
            //Debug.Log("Item comprado: " + shopItem.itemName + " x" + quantity);

            AudioManager.PlaySound(SoundType.BUY);

            // Analytics
            AnalyticsSystem.AddAnalyticPlants_Bought(this.name, shopItem.itemName, quantity);

            return true;
        }
        else
        {
            //Debug.Log("Dinheiro insuficiente ou item não encontrado.");
            return false;
        }
    }

    public bool SellItem(int itemID, int quantity)
    {
        // Encontra o item no inventário do jogador
        InventoryItem inventoryItem = playerInventory.items.Find(i => i.item.itemID == itemID);

        if (inventoryItem != null && inventoryItem.quantity >= quantity)
        {
            // Calcula o valor total da venda com base no preço do item e a quantidade
            int totalSaleValue = inventoryItem.item.price * quantity;

            // Remove o item do inventário
            if (playerInventory.RemoveItem(itemID, quantity))
            {
                // Adiciona o dinheiro ao jogador usando o MoneyManager
                moneyManager.AddMoney(totalSaleValue);

                AudioManager.PlaySound(SoundType.SELL);

                // Analytics
                AnalyticsSystem.AddAnalyticPlants_Sold(this.name, inventoryItem.item.itemName, quantity);

                //Debug.Log("Item vendido: " + inventoryItem.item.itemName + " x" + quantity);
                return true;
            }
        }

        //Debug.Log("Não foi possível vender o item ou quantidade insuficiente.");
        return false;
    }

    
    // Função para converter InventoryItems em vendáveis diretamente, usando o preço do item
    public List<Item> GetSellableItemsFromInventory()
    {
        List<Item> sellableItems = new List<Item>();
        
        // Pega todos os itens do inventário do jogador que são do tipo coletável (ou outros tipos relevantes)
        List<InventoryItem> playerItems = playerInventory.GetCollectedItems();
        
        foreach (InventoryItem inventoryItem in playerItems)
        {
            // Não precisa de conversão adicional, apenas usa o preço do próprio item
            sellableItems.Add(inventoryItem.item);
        }

        return sellableItems;
    }
}
