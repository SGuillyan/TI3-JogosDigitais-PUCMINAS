using UnityEngine;
using UnityEngine.UI;
using TMPro; 

public class PlowNearbyRiver : MonoBehaviour
{
    [Header("Popup UI Elements")]
    public GameObject popup;          // Referência ao popup do aviso
    public TMP_Text[] messages;           // Array de textos para exibir mensagens aleatórias
    public Button closeButton;        // Botão para fechar o popup

    private void Start()
    {
        // Garante que o popup está oculto no início
        popup.SetActive(false);

        // Certifica-se de que todos os textos estão desativados inicialmente
        foreach (TMP_Text message in messages)
        {
            message.gameObject.SetActive(false);
        }

        // Configura o evento do botão de fechar
        closeButton.onClick.AddListener(ClosePopup);
    }

    // Método para mostrar o popup e selecionar um texto aleatório
    public void ShowPopup()
    {
        if (messages.Length == 0)
        {
            Debug.LogWarning("Nenhuma mensagem configurada no PlowNearbyRiver.");
            return;
        }

        // Garante que todos os textos estão desativados
        foreach (TMP_Text message in messages)
        {
            message.gameObject.SetActive(false);
        }

        // Escolhe um texto aleatório para ativar
        int randomIndex = Random.Range(0, messages.Length);
        messages[randomIndex].gameObject.SetActive(true);

        // Exibe o popup
        popup.SetActive(true);
    }

    // Método para fechar o popup
    public void ClosePopup()
    {
        popup.SetActive(false); // Oculta o popup
    }
}
