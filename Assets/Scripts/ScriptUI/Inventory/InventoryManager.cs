using UnityEngine;
using UnityEngine.UI;  // Para lidar com UI

public class InventoryManager : MonoBehaviour
{
    public Inventory playerInventory;  // Referência ao inventário do jogador
    public InventoryUI inventoryUI;  // Referência ao script de UI do inventário

    public MenuManager menuManager;
    private int selectedItemID = -1;  // ID do item selecionado (genérico)
    private bool isPlanting = false;  // Flag para verificar se o jogador está no modo de plantio
    private bool isAnotherItemSelected = false;  // Flag para verificar se outro tipo de item foi selecionado

    public Button shopButton;  // Referência ao botão Shop
    public Button stopPlantingButton;  // Referência ao botão Stop Planting

    [Header("Feedback")]
    [SerializeField] private PlantingFeeback plantingFeedback;

    void Start()
    {
        // Inicialmente, o botão Stop Planting está desativado
        stopPlantingButton.gameObject.SetActive(false);
    }

    // Método para selecionar uma semente
    public void SelectItem(int itemID, int feedbackID)
    {
        // Verifica se o item selecionado é uma semente (ID entre 0 e 99)
        if (itemID >= 0 && itemID <= 99)
        {
            Debug.Log("Selecionando semente de id " + itemID);
            ToolsManager.SetActiveTool(ToolsManager.Tools.Plant);
            selectedItemID = itemID;
            menuManager.CloseInventoryToPlant();  // Fecha o inventário ao selecionar uma semente
            plantingFeedback.ActiveFeedback(feedbackID);
            isPlanting = true;  // Ativa o modo de plantio
            isAnotherItemSelected = false;  // Reset para outro item

            // Troca os botões
            stopPlantingButton.gameObject.SetActive(true);
        }
        // Verifica se o item está entre 200 e 299 (exemplo de outro tipo de item)
        else if (itemID >= 200 && itemID <= 299)
        {
            Debug.Log("Selecionando fertilizer de id " + itemID);
            ToolsManager.SetActiveTool(ToolsManager.Tools.Fertilize);  // Define o tipo de ferramenta de acordo
            selectedItemID = itemID;
            menuManager.CloseInventoryToPlant();  // Fecha o inventário ao selecionar uma semente
            isAnotherItemSelected = true;  // Marca como outro tipo de item
            // Lógica adicional para outro tipo de item (exemplo)
            // TODO: Adicionar lógica para o uso do item de id entre 200-299

            stopPlantingButton.gameObject.SetActive(true);  // Desativa o botão de parar plantio
        }
        else
        {
            DeselectItem();  // Deseleciona qualquer item
        }
    }

    // Método para deselecionar o item e voltar ao inventário
    public void DeselectItem()
    {
        selectedItemID = -1;
        ToolsManager.SetActiveTool(ToolsManager.Tools.None);
        isPlanting = false;  // Desativa o modo de plantio
        isAnotherItemSelected = false;  // Reset para outro tipo de item

        // Troca os botões
        stopPlantingButton.gameObject.SetActive(false);

        menuManager.OpenInventory();  // Reabre o inventário
        plantingFeedback.DisableFeedback();
    }

    // Verifica se um item está selecionado
    public bool HasSelectedItem()
    {
        return selectedItemID != -1;
    }

    // Retorna o ID do item selecionado
    public int GetSelectedItemID()
    {
        return selectedItemID;
    }

    // Método para usar o item selecionado (exemplo para sementes ou outro tipo de item)
    public void UseItemAt(Vector3Int gridPosition)
    {
        if (selectedItemID != -1)
        {
            // Se for uma semente (ID entre 0 e 99)
            if (selectedItemID >= 0 && selectedItemID <= 99)
            {
                InventoryItem selectedItem = playerInventory.items.Find(item => item.item.itemID == selectedItemID);
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
                            DeselectItem();  // Deseleciona o item
                        }

                        inventoryUI.UpdateInventoryUI();  // Atualiza o inventário
                    }
                    else
                    {
                        Debug.LogError("Não foi possível remover a quantidade desejada.");
                    }
                }
            }
            // Se for outro tipo de item (ID entre 200 e 299)
            else if (selectedItemID >= 200 && selectedItemID <= 299)
            {
                InventoryItem selectedItem = playerInventory.items.Find(item => item.item.itemID == selectedItemID);
                if (selectedItem != null)
                {
                    bool quantityRemoved = selectedItem.RemoveQuantity(1);  // Remove 1 unidade da semente

                    if (quantityRemoved)
                    {
                        // Plantar no gridPosition aqui (adapte isso ao seu jogo)
                        Debug.Log("Fertilized: " + gridPosition);

                        if (selectedItem.quantity <= 0)
                        {
                            // Remove o item do inventário se a quantidade for 0
                            playerInventory.items.Remove(selectedItem);
                            DeselectItem();  // Deseleciona o item
                        }

                        inventoryUI.UpdateInventoryUI();  // Atualiza o inventário
                    }
                    else
                    {
                        Debug.LogError("Não foi possível remover a quantidade desejada.");
                    }
                }
            }
        }
    }

    // Método para parar o plantio (associado ao botão Stop Planting)
    public void StopPlanting()
    {
        DeselectItem();  // Deseleciona o item e volta ao inventário       
    }
}
