using UnityEngine;
using UnityEngine.UI;

public class TreeCutWarning : MonoBehaviour
{
    [Header("Popup UI Elements")]
    public GameObject popup;           // Referência ao popup do aviso
    public Toggle doNotShowAgainToggle; // Toggle para não exibir novamente
    public Button closeButton;         // Botão de fechar o popup

    private const string DO_NOT_SHOW_KEY = "DoNotShowTreeCutWarning"; // Chave para salvar no PlayerPrefs

    void Start()
    {
        // Configura os eventos do botão e do toggle
        closeButton.onClick.AddListener(ClosePopup);
        doNotShowAgainToggle.onValueChanged.AddListener(SetDoNotShowAgainPreference);

        // Certifica-se de que o popup está desativado inicialmente
        popup.SetActive(false);

        // Remova preferências salvas se estiver no editor
        if (!Application.isPlaying)
        {
            PlayerPrefs.DeleteKey(DO_NOT_SHOW_KEY); // Remove a chave do PlayerPrefs
            Debug.Log("Preferência de 'Não mostrar novamente' foi desativada no editor.");
        }
    }

    // Método para exibir o popup
    public void ShowPopup()
    {
       // if (PlayerPrefs.GetInt(DO_NOT_SHOW_KEY, 0) == 0) // Só exibe se o toggle "Não mostrar novamente" não foi marcado
        //{
            popup.SetActive(true);
        //}
    }

    // Método chamado ao clicar no botão de fechar
    public void ClosePopup()
    {
        popup.SetActive(false); // Fecha o popup
    }

    // Método chamado ao alterar o estado do toggle
    public void SetDoNotShowAgainPreference(bool isOn)
    {
        if (!Application.isPlaying)
        {
            Debug.Log("Preferência de 'Não mostrar novamente' não é salva no editor.");
            return; // Não salva preferências no modo de edição
        }

        PlayerPrefs.SetInt(DO_NOT_SHOW_KEY, isOn ? 1 : 0); // Salva a preferência no PlayerPrefs
        PlayerPrefs.Save(); // Garante que os dados sejam salvos
    }
}
