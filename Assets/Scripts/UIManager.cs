using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public TMP_Text moneyText;  // Referência ao componente TextMeshPro que exibe o dinheiro

    private MoneyManager moneyManager;

    public GameObject tileInfoPanel;  // Painel lateral que exibe as informações do tile
    public Image tileSpriteImage;  // Imagem para exibir o sprite do tile
    public TMP_Text nitrogenText;  // Texto para exibir Nitrogênio (N)
    public TMP_Text phosphorusText;  // Texto para exibir Fósforo (P)
    public TMP_Text potassiumText;  // Texto para exibir Potássio (K)
    public TMP_Text humidityText;  // Texto para exibir Umidade
    public TMP_Text plantableText;  // Texto para exibir se o tile é plantável ou não

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

    public void UpdateTileInfo(Sprite tileSprite, int nitrogen, int phosphorus, int potassium, int humidity, bool isPlantable)
    {
        // Exibe o painel de informações
        tileInfoPanel.SetActive(true);

        // Atualiza o sprite do tile
        if (tileSpriteImage != null)
        {
            tileSpriteImage.sprite = tileSprite;
        }

        // Atualiza os textos dos nutrientes NPK e umidade
        nitrogenText.text = "Nitrogênio (N): " + nitrogen.ToString();
        phosphorusText.text = "Fósforo (P): " + phosphorus.ToString();
        potassiumText.text = "Potássio (K): " + potassium.ToString();
        humidityText.text = "Umidade: " + humidity.ToString();

        // Atualiza o status de plantabilidade
        if (isPlantable)
        {
            plantableText.text = "Plantável";
            plantableText.color = Color.green;  // Cor verde se o tile for plantável
        }
        else
        {
            plantableText.text = "Não Plantável";
            plantableText.color = Color.red;  // Cor vermelha se o tile não for plantável
        }
    }

    public void HideTileInfo()
    {
        tileInfoPanel.SetActive(false);
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
