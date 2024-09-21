using UnityEngine;
using TMPro;
using UnityEngine.UIElements;

public class UIManager : MonoBehaviour
{

/*
    public TMP_Text moneyText;  // Referência ao componente TextMeshPro que exibe o dinheiro

    private MoneyManager moneyManager;
*/

    // Reference to your UXML file
    public VisualTreeAsset visualTreeAsset;

    void OnEnable()
    {
        // Get the root VisualElement
        var root = GetComponent<UIDocument>().rootVisualElement;

        // Find the button by name and register a click event on the button
        Button inventoryButton = root.Q<Button>("btnInventory");

        inventoryButton.clicked += () =>
        {
            Debug.Log("Inventory opened!");
        };
        
        Button shopButton = root.Q<Button>("btnShop");

        shopButton.clicked += () =>
        {
            Debug.Log("Shop opened!");
        };

        Button configButton = root.Q<Button>("btnConfig");

        configButton.clicked += () =>
        {
            Debug.Log("Config Screen opened!");
        };
    }

/*
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
*/
}
