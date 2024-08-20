using System.Collections.Generic;
using UnityEngine;
using System;

public class Inventory : MonoBehaviour
{
    public List<InventoryItem> items = new List<InventoryItem>();  // Lista de itens no inventário

    // Evento para notificar mudanças no inventário
    public event Action onInventoryChanged;

    // Método para adicionar um item ao inventário
    public void AddItem(Item item, int quantity)
    {
        InventoryItem existingItem = items.Find(i => i.item.itemName == item.itemName);

        if (existingItem != null)
        {
            // Se o item já existe no inventário, adiciona a quantidade
            existingItem.AddQuantity(quantity);
        }
        else
        {
            // Se o item não existe, cria um novo InventoryItem e adiciona à lista
            InventoryItem newItem = new InventoryItem(item, quantity);
            items.Add(newItem);
        }

        // Dispara o evento de mudança no inventário
        onInventoryChanged?.Invoke();
    }

    // Método para remover um item do inventário
    public bool RemoveItem(Item item, int quantity)
    {
        InventoryItem existingItem = items.Find(i => i.item.itemName == item.itemName);

        if (existingItem != null)
        {
            // Tenta remover a quantidade solicitada
            if (existingItem.RemoveQuantity(quantity))
            {
                // Se a quantidade for zero, remove o item do inventário
                if (existingItem.quantity == 0)
                {
                    items.Remove(existingItem);
                }

                // Dispara o evento de mudança no inventário
                onInventoryChanged?.Invoke();
                return true;
            }
        }

        // Retorna falso se não foi possível remover a quantidade desejada
        return false;
    }

    // Método para obter a quantidade de um item específico
    public int GetItemQuantity(Item item)
    {
        InventoryItem existingItem = items.Find(i => i.item.itemName == item.itemName);

        if (existingItem != null)
        {
            return existingItem.quantity;
        }

        return 0;  // Retorna 0 se o item não estiver no inventário
    }

    // Método para filtrar itens por tipo
    public List<InventoryItem> GetItemsByType(ItemType itemType)
    {
        return items.FindAll(i => i.item.itemType == itemType);
    }

    // Método para obter todos os itens coletados
    public List<InventoryItem> GetCollectedItems()
    {
        return GetItemsByType(ItemType.CollectedItem);
    }

    // Método para obter todas as sementes
    public List<InventoryItem> GetSeeds()
    {
        return GetItemsByType(ItemType.Seed);
    }

    // Método para obter todos os fertilizantes
    public List<InventoryItem> GetFertilizers()
    {
        return GetItemsByType(ItemType.Fertilizer);
    }
}
