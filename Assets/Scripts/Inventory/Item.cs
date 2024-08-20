using UnityEngine;

[System.Serializable]
public class Item
{
    public string itemName;  // Nome do item (ex: "Semente de Milho", "Colheita de Trigo")
    public Sprite itemIcon;  // Ícone do item para exibição na UI
    public int maxStackSize = 99;  // Quantidade máxima que pode ser empilhada (ex: 99 sementes por slot)
    public ItemType itemType;  // Tipo de item (ex: Coletado, Semente, Fertilizante)

    public Item(string name, Sprite icon, int maxStack, ItemType type)
    {
        itemName = name;
        itemIcon = icon;
        maxStackSize = maxStack;
        itemType = type;
    }
}
