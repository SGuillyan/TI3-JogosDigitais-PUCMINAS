using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public TMP_Text moneyText;  // Referência ao componente TextMeshPro que exibe o dinheiro

    private MoneyManager moneyManager;

    void Start()
    {
        // Encontra o MoneyManager no jogo
        moneyManager = FindObjectOfType<MoneyManager>();

        if (moneyManager != null)
        {
            // Conecta a função UpdateMoneyUI ao evento onMoneyChanged do MoneyManager
            moneyManager.onMoneyChanged += UpdateMoneyUI;

            // Atualiza o texto da UI imediatamente para refletir o saldo inicial
            UpdateMoneyUI(moneyManager.GetCurrentMoney());
        }
        else
        {
            Debug.LogError("MoneyManager não encontrado na cena.");
        }
    }

    // Atualiza o texto do dinheiro na UI
    void UpdateMoneyUI(int newAmount)
    {
        if (moneyText != null)
        {
            moneyText.text = "Cash: " + newAmount.ToString();
        }
        else
        {
            Debug.LogError("moneyText não está atribuído no UIManager.");
        }
    }
}
