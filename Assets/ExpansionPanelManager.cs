using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ExpansionPanelManager : MonoBehaviour
{
    public GameObject expansionPanel; // Painel que contém os botões
    public Button[] directionButtons; // Botões para cada direção (3x3 grid)
    public TilemapManager tilemapManager; // Referência ao TilemapManager
    public Vector3Int initialCenter = new Vector3Int(10, -10, 0); // Centro inicial

    private HashSet<Vector3Int> expandedAreas = new HashSet<Vector3Int>();

    void Start()
    {
        // Desativa o painel no início
        expansionPanel.SetActive(false);

        // Configura os botões
        for (int i = 0; i < directionButtons.Length; i++)
        {
            int index = i; // Necessário para capturar o índice corretamente no lambda
            directionButtons[index].onClick.AddListener(() => OnExpand(index));
        }

        // Marca a área inicial como já expandida
        expandedAreas.Add(initialCenter);
    }

    // Mostra o painel de expansão
    public void ShowExpansionPanel()
    {
        expansionPanel.SetActive(true);

        // Ativa ou desativa os botões com base nas áreas já expandidas
        for (int i = 0; i < directionButtons.Length; i++)
        {
            Vector3Int expansionArea = GetExpansionArea(i);
            directionButtons[i].interactable = !expandedAreas.Contains(expansionArea);
        }
    }

    // Expande a área correspondente ao botão clicado
    void OnExpand(int directionIndex)
    {
        Vector3Int expansionArea = GetExpansionArea(directionIndex);

        // Adiciona a área expandida ao conjunto
        expandedAreas.Add(expansionArea);

        // Define as coordenadas da expansão
        Vector3Int topLeft, bottomRight;
        GetExpansionCoordinates(directionIndex, out topLeft, out bottomRight);

        // Chama a função de expansão com os limites corretos
        tilemapManager.ExpandTerrain(topLeft, bottomRight);

        // Atualiza os botões e fecha o painel
        ShowExpansionPanel();
        expansionPanel.SetActive(false);
    }

    // Calcula as coordenadas centrais da área a ser expandida com base no botão clicado
    Vector3Int GetExpansionArea(int directionIndex)
    {
        switch (directionIndex)
        {
            case 0: return new Vector3Int(-4, -16, 0); // Norte
            case 1: return new Vector3Int(-4, -1, 0);  // Nordeste
            case 2: return new Vector3Int(3, -1, 0);   // Leste
            case 3: return new Vector3Int(-4, -23, 0); // Noroeste
            case 4: return initialCenter;             // Centro
            case 5: return new Vector3Int(17, -1, 0); // Sudeste
            case 6: return new Vector3Int(17, -16, 0); // Sul
            case 7: return new Vector3Int(17, -23, 0); // Sudoeste
            case 8: return new Vector3Int(3, -23, 0);  // Oeste
            default: return initialCenter;
        }
    }


    // Define as coordenadas de expansão para os limites inferiores e superiores do quadrilátero
    void GetExpansionCoordinates(int directionIndex, out Vector3Int topLeft, out Vector3Int bottomRight)
    {
        switch (directionIndex)
        {
            case 0: // Norte
                topLeft = new Vector3Int(-4, -16, 0);
                bottomRight = new Vector3Int(2, -2, 0);
                break;
            case 1: // Nordeste
                topLeft = new Vector3Int(-4, -1, 0);
                bottomRight = new Vector3Int(2, 5, 0);
                break;
            case 2: // Leste
                topLeft = new Vector3Int(3, -1, 0);
                bottomRight = new Vector3Int(16, 5, 0);
                break;
            case 3: // Noroeste
                topLeft = new Vector3Int(-4, -23, 0);
                bottomRight = new Vector3Int(2, -17, 0);
                break;
            case 4: // Centro
                topLeft = initialCenter - new Vector3Int(8, 8, 0);
                bottomRight = initialCenter + new Vector3Int(8, 8, 0);
                break;
            case 5: // Sudeste
                topLeft = new Vector3Int(17, -1, 0);
                bottomRight = new Vector3Int(23, 5, 0);
                break;
            case 6: // Sul
                topLeft = new Vector3Int(17, -16, 0);
                bottomRight = new Vector3Int(23, -2, 0);
                break;
            case 7: // Sudoeste
                topLeft = new Vector3Int(17, -23, 0);
                bottomRight = new Vector3Int(23, -17, 0);
                break;
            case 8: // Oeste
                topLeft = new Vector3Int(3, -23, 0);
                bottomRight = new Vector3Int(16, -17, 0);
                break;
            default:
                topLeft = Vector3Int.zero;
                bottomRight = Vector3Int.zero;
                break;
        }
    }
}
