using UnityEngine;
using UnityEngine.UI;  // Para lidar com UI

public class InventoryManager : MonoBehaviour
{
    public Inventory playerInventory;  // Referência ao inventário do jogador
    public InventoryUI inventoryUI;  // Referência ao script de UI do inventário
    private int selectedSeedID = -1;  // ID da semente selecionada
    private bool isPlanting = false;  // Flag para verificar se o jogador está no modo de plantio

    public Button shopButton;  // Referência ao botão Shop
    public Button stopPlantingButton;  // Referência ao botão Stop Planting

    void Start()
    {
        // Inicialmente, o botão Stop Planting está desativado
        stopPlantingButton.gameObject.SetActive(false);
    }

    // Método para selecionar uma semente
    public void SelectSeed(int seedID)
    {
        // Verifica se o item selecionado é uma semente (ID entre 0 e 99)
        if (seedID >= 0 && seedID <= 99)
        {
            Debug.Log("Selecionando semente de id " + seedID);
            ToolsManager.SetActiveTool(ToolsManager.Tools.None);
            selectedSeedID = seedID;
            inventoryUI.CloseInventory();  // Fecha o inventário ao selecionar uma semente
            isPlanting = true;  // Ativa o modo de plantio

            // Troca os botões
            // shopButton.gameObject.SetActive(false);
            stopPlantingButton.gameObject.SetActive(true);
        }
        else
        {
            DeselectSeed();  // Deseleciona se o item não for uma semente
        }
    }

    // Método para deselecionar a semente e voltar ao inventário
    public void DeselectSeed()
    {
        selectedSeedID = -1;
        isPlanting = false;  // Desativa o modo de plantio

        // Troca os botões
        // shopButton.gameObject.SetActive(true);
        stopPlantingButton.gameObject.SetActive(false);

        inventoryUI.OpenInventory();  // Reabre o inventário
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

    // Método para parar o plantio (associado ao botão Stop Planting)
    public void StopPlanting()
    {
        DeselectSeed();  // Deseleciona a semente e volta ao inventário
    }
}
