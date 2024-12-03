using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameSpeedController : MonoBehaviour
{
    public TextMeshProUGUI speedText;  // Referência para o texto de velocidade (opcional)
    public GameObject warningPanel;  // Painel de aviso para quando atingir 16x
    public GameObject configPanel;  // Painel de configurações (novo)
    public Button continueButton;  // Botão para continuar com a aceleração
    public Button cancelButton;    // Botão para desistir e voltar à velocidade anterior

    private int currentSpeedIndex = 0;  // Indica o índice da velocidade (0 = 1x, 1 = 2x, 2 = 4x, 3 = 16x)
    private int previousSpeedIndex = 0;  // Armazena a velocidade anterior (para voltar atrás se desistir)

    private float[] speeds = { 1f, 2f, 4f, 16f };  // Array de velocidades do jogo

    void Start()
    {
        Time.timeScale = speeds[currentSpeedIndex];  // Define a velocidade inicial
        warningPanel.SetActive(false);  // Esconde o aviso no início
        configPanel.SetActive(true);  // Garante que o painel de configurações esteja visível

        // Configura os botões para suas ações
        continueButton.onClick.AddListener(ContinueAcceleration);
        cancelButton.onClick.AddListener(CancelAcceleration);
    }

    public void OnSpeedButtonClick()
    {
        // Aumenta a velocidade
        currentSpeedIndex++;
        
        // Reseta para 1x se chegar a 16x
        if (currentSpeedIndex > 3)
        {
            currentSpeedIndex = 0;  // Retorna para 1x
        }

        // Se a velocidade atingir 16x, exibe o aviso
        if (currentSpeedIndex == 3)
        {
            // Fecha o painel de configurações e exibe o painel de aviso
            configPanel.SetActive(false);
            warningPanel.SetActive(true);  // Exibe o aviso
        }
        else
        {
            // Se não for 16x, aplica a nova velocidade diretamente
            Time.timeScale = speeds[currentSpeedIndex];
            // Atualiza o texto da velocidade no botão (opcional)
            if (speedText != null)
                speedText.text = speeds[currentSpeedIndex] + "x";

            warningPanel.SetActive(false);  // Esconde o aviso
            configPanel.SetActive(true);  // Garante que o painel de configurações esteja visível
        }
    }

    private void ContinueAcceleration()
    {
        // Continua com a aceleração para 16x
        Time.timeScale = speeds[currentSpeedIndex];
        warningPanel.SetActive(false);  // Esconde o aviso
        configPanel.SetActive(true);  // Volta o painel de configurações
        // Atualiza o texto da velocidade no botão (opcional)
        if (speedText != null)
            speedText.text = speeds[currentSpeedIndex] + "x";
    }

    private void CancelAcceleration()
    {
        // Desiste da aceleração e volta para a velocidade anterior (4x)
        currentSpeedIndex = previousSpeedIndex;
        Time.timeScale = speeds[currentSpeedIndex];
        warningPanel.SetActive(false);  // Esconde o aviso
        configPanel.SetActive(true);  // Volta o painel de configurações
        // Atualiza o texto da velocidade no botão (opcional)
        if (speedText != null)
            speedText.text = speeds[currentSpeedIndex] + "x";
    }
}
