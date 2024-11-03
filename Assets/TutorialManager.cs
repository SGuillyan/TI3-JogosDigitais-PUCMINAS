using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TutorialManager : MonoBehaviour
{
    [System.Serializable]
    public class TutorialStep
    {
        public string dialogueText;           // Texto que será exibido para o jogador
        public RectTransform targetUIElement; // Elemento de UI que a seta deve apontar
    }

    [SerializeField] public TutorialStep[] tutorialSteps;       // Lista dos passos do tutorial
    public GameObject tutorialGameObject;
    public TextMeshProUGUI dialogueTextUI;     // UI do texto do tutorial (pode ser um Text ou TextMeshProUGUI)
    public RectTransform arrowImage;           // Referência para a imagem da seta
    public Button nextButton;                  // Botão para avançar no tutorial

    private int currentStepIndex = 0;          // Índice do passo atual
    public float arrowOffsetY = 50f;           // Offset vertical da seta (quantos pixels acima do elemento alvo)

    void Start()
    {
        // Inicializa o tutorial mostrando o primeiro passo
        if (tutorialSteps.Length > 0)
        {
            ShowStep(currentStepIndex);
            tutorialGameObject.SetActive(true);
        }

        // Adiciona listener ao botão "Próximo"
        nextButton.onClick.AddListener(NextStep);
    }

    // Método para mostrar um passo específico do tutorial
    void ShowStep(int index)
    {
        if (index >= 0 && index < tutorialSteps.Length)
        {
            // Atualiza o texto do tutorial
            dialogueTextUI.text = tutorialSteps[index].dialogueText;

            // Atualiza a posição da seta para apontar para o elemento desejado
            if (tutorialSteps[index].targetUIElement != null)
            {
                arrowImage.gameObject.SetActive(true);

                // Calcula a posição da seta alguns pixels acima do elemento alvo
                Vector3 targetPosition = tutorialSteps[index].targetUIElement.position;
                targetPosition.y += arrowOffsetY; // Adiciona o offset vertical para posicionar a seta acima

                arrowImage.position = targetPosition;
            }
            else
            {
                // Se não houver um elemento para apontar, esconda a seta
                arrowImage.gameObject.SetActive(false);
            }
        }
    }

    // Método chamado ao clicar no botão "Próximo"
    void NextStep()
    {
        currentStepIndex++;

        // Se houver mais passos, exiba o próximo passo
        if (currentStepIndex < tutorialSteps.Length)
        {
            ShowStep(currentStepIndex);
        }
        else
        {
            // Se não houver mais passos, finalize o tutorial
            EndTutorial();
        }
    }

    // Método para finalizar o tutorial
    void EndTutorial()
    {
        dialogueTextUI.gameObject.SetActive(false);
        arrowImage.gameObject.SetActive(false);
        nextButton.gameObject.SetActive(false);
        tutorialGameObject.SetActive(false);
    }
}
