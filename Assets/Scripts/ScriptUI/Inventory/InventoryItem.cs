[System.Serializable]
public class InventoryItem
{
    public Item item;  // Referência ao tipo de item
    public int quantity;  // Quantidade do item no inventário

    public InventoryItem(Item newItem, int initialQuantity)
    {
        item = newItem;
        quantity = initialQuantity;
    }

    // Método para adicionar uma quantidade ao item
    public void AddQuantity(int amount)
    {
        quantity += amount;
        if (quantity > item.maxStackSize)
        {
            quantity = item.maxStackSize;  // Limita a quantidade ao máximo permitido
        }
    }

    // Método para remover uma quantidade do item
    public bool RemoveQuantity(int amount)
    {
        if (quantity >= amount)
        {
            quantity -= amount;
            return true;
        }
        else
        {
            return false;  // Não foi possível remover a quantidade desejada
        }
    }
}
