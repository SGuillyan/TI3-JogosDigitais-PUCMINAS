using UnityEngine;

public class InventoryTester : MonoBehaviour
{
    public Inventory playerInventory;  // Referência ao inventário do jogador
    public Item cornSeed;
    public Item compostFertilizer;
    public Item apple;

    // Testa a adição de itens
    public void TestAddCornSeed()
    {
        playerInventory.AddItem(cornSeed, 10);
        Debug.Log("Adicionou 10 Sementes de Milho ao inventário.");
    }

    public void TestAddFertilizer()
    {
        playerInventory.AddItem(compostFertilizer, 5);
        Debug.Log("Adicionou 5 Compostos de Fertilizante ao inventário.");
    }

    public void TestAddApple()
    {
        playerInventory.AddItem(apple, 20);
        Debug.Log("Adicionou 20 Maçãs ao inventário.");
    }

    // Testa a remoção de itens
    public void TestRemoveCornSeed()
    {
        if (playerInventory.RemoveItem(cornSeed, 5))
        {
            Debug.Log("Removeu 5 Sementes de Milho do inventário.");
        }
        else
        {
            Debug.Log("Falha ao remover 5 Sementes de Milho - quantidade insuficiente.");
        }
    }

    public void TestRemoveFertilizer()
    {
        if (playerInventory.RemoveItem(compostFertilizer, 3))
        {
            Debug.Log("Removeu 3 Compostos de Fertilizante do inventário.");
        }
        else
        {
            Debug.Log("Falha ao remover 3 Compostos de Fertilizante - quantidade insuficiente.");
        }
    }

    public void TestRemoveApple()
    {
        if (playerInventory.RemoveItem(apple, 10))
        {
            Debug.Log("Removeu 10 Maçãs do inventário.");
        }
        else
        {
            Debug.Log("Falha ao remover 10 Maçãs - quantidade insuficiente.");
        }
    }

    // Testa a exibição de quantidades
    public void TestGetCornSeedQuantity()
    {
        int quantity = playerInventory.GetItemQuantity(cornSeed);
        Debug.Log("Quantidade de Sementes de Milho no inventário: " + quantity);
    }

    public void TestGetFertilizerQuantity()
    {
        int quantity = playerInventory.GetItemQuantity(compostFertilizer);
        Debug.Log("Quantidade de Compostos de Fertilizante no inventário: " + quantity);
    }

    public void TestGetAppleQuantity()
    {
        int quantity = playerInventory.GetItemQuantity(apple);
        Debug.Log("Quantidade de Maçãs no inventário: " + quantity);
    }
}
