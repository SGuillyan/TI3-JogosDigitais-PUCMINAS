using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class ExpansionPanelManager : MonoBehaviour
{
    public GameObject expansionPanel; // Painel que contém os botões
    public Button confirmButton;      // Botão para confirmar a expansão
    public Button closeButton;        // Botão para fechar o painel
    public TMP_Text priceText;        // Texto para exibir o preço da expansão
    public TilemapManager tilemapManager; // Referência ao TilemapManager
    public MoneyManager moneyManager; // Referência ao MoneyManager
    public Vector3Int initialCenter = new Vector3Int(10, -10, 0); // Centro inicial

    public int[] expansionPrices;     // Vetor manual de preços
    private int currentExpansionIndex = 0; // Índice da expansão atual
    private List<Vector3Int> expansionOrder = new List<Vector3Int>(); // Ordem das expansões
    private HashSet<Vector3Int> expandedAreas = new HashSet<Vector3Int>(); // Áreas já expandidas

    void Start()
    {
        // Desativa o painel no início
        expansionPanel.SetActive(false);

        // Configura os botões
        confirmButton.onClick.AddListener(OnConfirmExpansion);
        closeButton.onClick.AddListener(() => expansionPanel.SetActive(false));

        // Inicializa a ordem das expansões (Norte, Leste, Sul, Oeste, NE, SE, SW, NW)
        expansionOrder.Add(GetExpansionArea(0)); // Norte
        expansionOrder.Add(GetExpansionArea(2)); // Leste
        expansionOrder.Add(GetExpansionArea(1)); // Sul
        expansionOrder.Add(GetExpansionArea(3)); // Oeste
        expansionOrder.Add(GetExpansionArea(5)); // Nordeste
        expansionOrder.Add(GetExpansionArea(7)); // Sudeste
        expansionOrder.Add(GetExpansionArea(8)); // Sudoeste
        expansionOrder.Add(GetExpansionArea(6)); // Noroeste

        // Marca a área inicial como já expandida
        expandedAreas.Add(initialCenter);

        UpdatePriceText();
    }

    // Mostra ou oculta o painel de expansão
    public void ShowExpansionPanel()
    {
        if (expansionPanel.activeSelf)
        {
            expansionPanel.SetActive(false);
        }
        else
        {
            expansionPanel.SetActive(true);
            UpdatePriceText();
        }
    }

    // Método chamado ao confirmar a expansão
    void OnConfirmExpansion()
    {
        if (currentExpansionIndex >= expansionOrder.Count)
        {
            Debug.LogWarning("Todas as áreas já foram expandidas!");
            return;
        }

        if (currentExpansionIndex >= expansionPrices.Length)
        {
            Debug.LogError("Preços insuficientes configurados para todas as expansões!");
            return;
        }

        // Obtém o preço da expansão atual
        int expansionCost = expansionPrices[currentExpansionIndex];

        // Verifica se o jogador tem dinheiro suficiente
        if (!moneyManager.SpendMoney(expansionCost))
        {
            return;
        }

        // Obtém a área atual a ser expandida
        Vector3Int expansionArea = expansionOrder[currentExpansionIndex];
        if (expandedAreas.Contains(expansionArea))
        {
            Debug.LogWarning("A área já foi expandida!");
            return;
        }

        // Marca a área como expandida
        expandedAreas.Add(expansionArea);

        // Define as coordenadas da expansão
        Vector3Int topLeft, bottomRight;
        GetExpansionCoordinates(currentExpansionIndex, out topLeft, out bottomRight);

        // DEBUG: Verifique as coordenadas calculadas
        Debug.Log($"Expansão {currentExpansionIndex} - TopLeft: {topLeft}, BottomRight: {bottomRight}");

        // Chama a função de expansão no TilemapManager
        tilemapManager.ExpandTerrain(topLeft, bottomRight);

        // Atualiza o índice da expansão atual
        currentExpansionIndex++;

        // Atualiza o texto do preço ou desativa o painel se não houver mais expansões
        if (currentExpansionIndex < expansionPrices.Length)
        {
            UpdatePriceText();
        }
        else
        {
            expansionPanel.SetActive(false);
            Debug.Log("Todas as expansões foram concluídas!");
        }
    }


    // Oculta o aviso de dinheiro insuficiente

    // Atualiza o texto do preço
    void UpdatePriceText()
    {
        if (currentExpansionIndex < expansionPrices.Length)
        {
            priceText.text = $"{expansionPrices[currentExpansionIndex]}";
        }
        else
        {
            priceText.text = "All done";
        }
    }

    // Calcula as coordenadas centrais da área a ser expandida
    Vector3Int GetExpansionArea(int directionIndex)
    {
        switch (directionIndex)
        {
            case 0: return new Vector3Int(-4, -16, 0); // Norte
            case 1: return new Vector3Int(17, -16, 0); // Sul
            case 2: return new Vector3Int(3, -1, 0);   // Leste
            case 3: return new Vector3Int(3, -23, 0);  // Oeste
            case 4: return initialCenter;             // Centro
            case 5: return new Vector3Int(-4, -1, 0);  // Nordeste
            case 6: return new Vector3Int(-4, -23, 0); // Noroeste
            case 7: return new Vector3Int(17, -1, 0);  // Sudeste
            case 8: return new Vector3Int(17, -23, 0); // Sudoeste
            default: return initialCenter;
        }
    }

    // Define as coordenadas de expansão
    void GetExpansionCoordinates(int expansionIndex, out Vector3Int topLeft, out Vector3Int bottomRight)
    {
        Vector3Int area = expansionOrder[expansionIndex]; // Obtém a área da ordem de expansão

        switch (area)
        {
            case { x: -4, y: -16 }: // Norte
                topLeft = new Vector3Int(-4, -16, 0);
                bottomRight = new Vector3Int(2, -2, 0);
                break;
            case { x: 17, y: -16 }: // Sul
                topLeft = new Vector3Int(17, -16, 0);
                bottomRight = new Vector3Int(23, -2, 0);
                break;
            case { x: 3, y: -1 }: // Leste
                topLeft = new Vector3Int(3, -1, 0);
                bottomRight = new Vector3Int(16, 5, 0);
                break;
            case { x: 3, y: -23 }: // Oeste
                topLeft = new Vector3Int(3, -23, 0);
                bottomRight = new Vector3Int(16, -17, 0);
                break;
            case { x: -4, y: -1 }: // Nordeste
                topLeft = new Vector3Int(-4, -1, 0);
                bottomRight = new Vector3Int(2, 5, 0);
                break;
            case { x: 17, y: -1 }: // Sudeste
                topLeft = new Vector3Int(17, -1, 0);
                bottomRight = new Vector3Int(23, 5, 0);
                break;
            case { x: 17, y: -23 }: // Sudoeste
                topLeft = new Vector3Int(17, -23, 0);
                bottomRight = new Vector3Int(23, -17, 0);
                break;
            case { x: -4, y: -23 }: // Noroeste
                topLeft = new Vector3Int(-4, -23, 0);
                bottomRight = new Vector3Int(2, -17, 0);
                break;
            default:
                topLeft = Vector3Int.zero;
                bottomRight = Vector3Int.zero;
                Debug.LogError($"Coordenadas não mapeadas para índice: {expansionIndex}");
                break;
        }
    }

}
