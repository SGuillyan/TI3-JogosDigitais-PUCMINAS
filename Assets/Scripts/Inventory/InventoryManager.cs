using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public Inventory playerInventory;  // Referência ao inventário do jogador
    public InventoryUI inventoryUI;  // Referência ao script de UI do inventário
    private int selectedSeedID = -1;  // ID da semente selecionada
    private bool isPlanting = false;  // Flag para verificar se o jogador está no modo de plantio

    // Método para selecionar uma semente
    public void SelectSeed(int seedID)
    {
        selectedSeedID = seedID;
        inventoryUI.CloseInventory();  // Fecha o inventário ao selecionar uma semente
        isPlanting = true;  // Ativa o modo de plantio
    }

    // Método para deselecionar a semente
    public void DeselectSeed()
    {
        selectedSeedID = -1;
        isPlanting = false;  // Desativa o modo de plantio
    }

    // Verifica se uma semente está selecionada
    public bool HasSelectedSeed()
    {
        return selectedSeedID != -1;
    }

    // Retorna o ID da semente selecionada
    public int GetSelectedSeedID()
    {
        return selectedSeedID;
    }

    // Método para plantar uma semente e remover do inventário
    public void PlantSeedAt(Vector3Int gridPosition)
    {
        if (isPlanting && selectedSeedID != -1)
        {
            InventoryItem selectedItem = playerInventory.items.Find(item => item.item.itemID == selectedSeedID);
            if (selectedItem != null)
            {
                bool quantityRemoved = selectedItem.RemoveQuantity(1);  // Remove 1 unidade da semente

                if (quantityRemoved)
                {
                    // Plantar no gridPosition aqui (adapte isso ao seu jogo)
                    Debug.Log("Planted at: " + gridPosition);

                    if (selectedItem.quantity <= 0)
                    {
                        // Remove o item do inventário se a quantidade for 0
                        playerInventory.items.Remove(selectedItem);
                        DeselectSeed();
                        inventoryUI.OpenInventory();  // Reabre o inventário se o item foi removido
                    }

                    inventoryUI.UpdateInventoryUI();  // Atualiza o inventário para refletir a nova quantidade
                }
                else
                {
                    Debug.LogError("Não foi possível remover a quantidade desejada.");
                }
            }
        }
    }
}
