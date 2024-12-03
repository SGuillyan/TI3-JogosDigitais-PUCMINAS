using UnityEngine;

[System.Serializable]
public class Item
{
    public string itemName;  // Nome do item (ex: "Semente de Milho", "Colheita de Trigo")
    public Sprite itemIcon;  // Ícone do item para exibição na UI
    public int maxStackSize = 99;  // Quantidade máxima que pode ser empilhada (ex: 99 sementes por slot)
    public ItemType itemType;  // Tipo de item (ex: Coletado, Semente, Fertilizante)
    public int itemID;  // ID único do item
    public int price;  // Preço do item

    // Atributos para sementes, só devem ser utilizados se o item for do tipo Seed
    public float reqNitro;  // Requerimento de Nitrogênio
    public float reqPhosf;  // Requerimento de Fósforo
    public float reqK;      // Requerimento de Potássio

    // Construtor para inicializar os itens com suas propriedades, incluindo o preço
    public Item(string name, Sprite icon, int maxStack, ItemType type, int id, int price)
    {
        itemName = name;
        itemIcon = icon;
        maxStackSize = maxStack;
        itemType = type;
        itemID = id;
        this.price = price;  // Atribui o preço ao item
    }

    // Método para configurar os atributos adicionais, mas só se for uma Semente
    public void SetSeedRequirements(float nitro, float phosf, float k)
    {
        if (itemType == ItemType.Seed)
        {
            reqNitro = nitro;
            reqPhosf = phosf;
            reqK = k;
        }
        else
        {
            Debug.LogWarning("Este item não é do tipo Seed e não possui requisitos.");
        }
    }
}
