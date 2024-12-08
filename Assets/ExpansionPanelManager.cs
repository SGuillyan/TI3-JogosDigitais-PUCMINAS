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

        // Atualiza a interatividade dos botões no início
        UpdateButtonInteractability();
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
            UpdateButtonInteractability();
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

        // Atualiza a interatividade dos botões após a expansão
        UpdateButtonInteractability();

        // Fecha o painel
        ShowExpansionPanel();
    }

    // Atualiza a interatividade dos botões
    private void UpdateButtonInteractability()
    {
        for (int i = 0; i < directionButtons.Length; i++)
        {
            Vector3Int expansionArea = GetExpansionArea(i);

            // Verifica se o botão pode ser interagível
            if (i <= 3 || i == 4) // Direções N, S, E, W e Centro
            {
                directionButtons[i].interactable = !expandedAreas.Contains(expansionArea);
            }
            else // NE, NW, SE, SW
            {
                // Só permite interagir se uma área adjacente foi expandida
                bool canExpand = IsAdjacentAreaExpanded(i) && !expandedAreas.Contains(expansionArea);
                directionButtons[i].interactable = canExpand;
            }
        }
    }

    // Calcula as coordenadas centrais da área a ser expandida com base no botão clicado
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

    // Define as coordenadas de expansão para os limites inferiores e superiores do quadrilátero
    void GetExpansionCoordinates(int directionIndex, out Vector3Int topLeft, out Vector3Int bottomRight)
    {
        switch (directionIndex)
        {
            case 0: // Norte
                topLeft = new Vector3Int(-4, -16, 0);
                bottomRight = new Vector3Int(2, -2, 0);
                break;
            case 1: // Sul
                topLeft = new Vector3Int(17, -16, 0);
                bottomRight = new Vector3Int(23, -2, 0);
                break;
            case 2: // Leste
                topLeft = new Vector3Int(3, -1, 0);
                bottomRight = new Vector3Int(16, 5, 0);
                break;
            case 3: // Oeste
                topLeft = new Vector3Int(3, -23, 0);
                bottomRight = new Vector3Int(16, -17, 0);
                break;
            case 4: // Centro
                topLeft = initialCenter - new Vector3Int(8, 8, 0);
                bottomRight = initialCenter + new Vector3Int(8, 8, 0);
                break;
            case 5: // Nordeste
                topLeft = new Vector3Int(-4, -1, 0);
                bottomRight = new Vector3Int(2, 5, 0);
                break;
            case 6: // Noroeste
                topLeft = new Vector3Int(-4, -23, 0);
                bottomRight = new Vector3Int(2, -17, 0);
                break;
            case 7: // Sudeste
                topLeft = new Vector3Int(17, -1, 0);
                bottomRight = new Vector3Int(23, 5, 0);
                break;
            case 8: // Sudoeste
                topLeft = new Vector3Int(17, -23, 0);
                bottomRight = new Vector3Int(23, -17, 0);
                break;
            default:
                topLeft = Vector3Int.zero;
                bottomRight = Vector3Int.zero;
                break;
        }
    }

    // Verifica se alguma área adjacente foi expandida
    bool IsAdjacentAreaExpanded(int directionIndex)
    {
        switch (directionIndex)
        {
            case 5: // Nordeste depende de Norte ou Leste
                return expandedAreas.Contains(GetExpansionArea(0)) || expandedAreas.Contains(GetExpansionArea(2));
            case 6: // Noroeste depende de Norte ou Oeste
                return expandedAreas.Contains(GetExpansionArea(0)) || expandedAreas.Contains(GetExpansionArea(3));
            case 7: // Sudeste depende de Sul ou Leste
                return expandedAreas.Contains(GetExpansionArea(1)) || expandedAreas.Contains(GetExpansionArea(2));
            case 8: // Sudoeste depende de Sul ou Oeste
                return expandedAreas.Contains(GetExpansionArea(1)) || expandedAreas.Contains(GetExpansionArea(3));
            default:
                return false;
        }
    }
}
