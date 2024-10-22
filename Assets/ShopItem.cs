using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ShopItem
{
    public Item item;  // Referência ao tipo de item
    public int price;  // Preço do item na loja

    public ShopItem(Item newItem, int itemPrice)
    {
        item = newItem;
        price = itemPrice;
    }
}
