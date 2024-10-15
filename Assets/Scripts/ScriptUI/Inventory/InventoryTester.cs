using UnityEngine;

public class InventoryTester : MonoBehaviour
{
    public Inventory playerInventory;  // Referência ao inventário do jogador
    public Item[] seeds;
    public Item strawberrySeed;
    public Item apple;

    // Testa a adição de itens
    
    public void TestAddSeeds()
    {
        for(int i = 0;i<seeds.Length; i++){
            playerInventory.AddItem(seeds[i], 5);
            }
    }
    public void TestAddCornSeed()
    {
        playerInventory.AddItem(seeds[0], 10);
        Debug.Log("Adicionou 10 Sementes de Milho ao inventário.");
    }

    public void TestAddStrawberry()
    {
        playerInventory.AddItem(strawberrySeed, 5);
        Debug.Log("Adicionou 5 Sementes de Morango ao inventário.");
    }

    public void TestAddApple()
    {
        playerInventory.AddItem(apple, 20);
        Debug.Log("Adicionou 20 Maçãs ao inventário.");
    }

    // Testa a remoção de itens
    public void TestRemoveCornSeed()
    {
        if (playerInventory.RemoveItem(seeds[0], 5))
        {
            Debug.Log("Removeu 5 Sementes de Milho do inventário.");
        }
        else
        {
            Debug.Log("Falha ao remover 5 Sementes de Milho - quantidade insuficiente.");
        }
    }

    public void TestRemoveStrawberry()
    {
        if (playerInventory.RemoveItem(strawberrySeed, 3))
        {
            Debug.Log("Removeu 3 Sementes de Morango do inventário.");
        }
        else
        {
            Debug.Log("Falha ao remover 3 Sementes de Morango - quantidade insuficiente.");
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
    public void TestGetSeedsQuantity()
    {
        int quantity = playerInventory.GetItemQuantity(seeds[0]);
        Debug.Log("Quantidade de Sementes de Milho no inventário: " + quantity);
    }

    public void TestGetStrawberryQuantity()
    {
        int quantity = playerInventory.GetItemQuantity(strawberrySeed);
        Debug.Log("Quantidade de Sementes de Morango no inventário: " + quantity);
    }

    public void TestGetAppleQuantity()
    {
        int quantity = playerInventory.GetItemQuantity(apple);
        Debug.Log("Quantidade de Maçãs no inventário: " + quantity);
    }
}
