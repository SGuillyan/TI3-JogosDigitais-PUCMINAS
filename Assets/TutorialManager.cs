using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class TutorialManager : MonoBehaviour
{
    [System.Serializable]
    public class TutorialStep
    {
        public string dialogueText;           // Texto que será exibido para o jogador
        public RectTransform targetUIElement; // Elemento de UI que será o alvo do passo
        public List<Button> buttonsToClick = new List<Button>();   // Lista de botões que devem ser clicados antes de prosseguir
        public List<Toggle> togglesToActivate = new List<Toggle>(); // Lista de toggles que devem ser ativados antes de prosseguir
        public Vector2 popupPosition = Vector2.zero;         // Posição na tela para exibir o pop-up do tutorial
        [HideInInspector]
        public int buttonsClicked = 0;        // Contador de botões clicados
        [HideInInspector]
        public int togglesActivated = 0;      // Contador de toggles ativados
    }

    [SerializeField] public TutorialStep[] tutorialSteps;       // Lista dos passos do tutorial
    public GameObject tutorialGameObject;
    public TextMeshProUGUI dialogueTextUI;     // UI do texto do tutorial (pode ser um Text ou TextMeshProUGUI)
    public RectTransform arrowImage;           // Referência para a imagem da seta
    public Button nextButton;                  // Botão para avançar no tutorial

    private int currentStepIndex = 0;          // Índice do passo atual
    public float arrowOffsetY = 50f;           // Offset vertical da seta (quantos pixels acima do elemento alvo)

    public bool tutirialCompleted = false;

    void Start()
    {
        if (SaveSystem.isFirstTime())
        {
            Inicialize(false);
        }
    }

    public void Inicialize(bool tutirialCompleted)
    {
        this.tutirialCompleted = tutirialCompleted;

        // Inicializa o tutorial mostrando o primeiro passo
        if (tutorialSteps.Length > 0 && !tutirialCompleted)
        {
            ShowStep(currentStepIndex);
            tutorialGameObject.SetActive(true);

            // Analytics
            AnalyticsManager.SetStartTutorial(true);

            // MenuManager
            MenuManager.openedMenu = true;
        }

        // Adiciona listener ao botão "Próximo"
        nextButton.onClick.AddListener(NextStep);
    }

    // Método para mostrar um passo específico do tutorial
    void ShowStep(int index)
    {
        if (index >= 0 && index < tutorialSteps.Length)
        {
            TutorialStep currentStep = tutorialSteps[index];

            // Atualiza o texto do tutorial
            dialogueTextUI.text = currentStep.dialogueText;

            // Atualiza a posição do pop-up do tutorial
            if (currentStep.popupPosition == Vector2.zero)
            {
                tutorialGameObject.GetComponent<RectTransform>().anchoredPosition = tutorialGameObject.GetComponent<RectTransform>().anchoredPosition;
            }
            else
            {
                tutorialGameObject.GetComponent<RectTransform>().anchoredPosition = currentStep.popupPosition;
            }

            // Atualiza a posição da seta para apontar para o elemento desejado
            if (currentStep.targetUIElement != null)
            {
                arrowImage.gameObject.SetActive(true);

                // Calcula a posição da seta alguns pixels acima do elemento alvo
                Vector3 targetPosition = currentStep.targetUIElement.position;
                targetPosition.y += arrowOffsetY;

                arrowImage.position = targetPosition;

                // Adiciona evento ao targetUIElement para trocar de passo
                AddTargetListener(currentStep.targetUIElement);
            }
            else
            {
                arrowImage.gameObject.SetActive(false);
            }

            // Adiciona listeners aos botões do passo atual
            foreach (Button button in currentStep.buttonsToClick)
            {
                button.onClick.AddListener(() => OnButtonClicked(currentStep));
            }

            // Adiciona listeners aos toggles do passo atual
            foreach (Toggle toggle in currentStep.togglesToActivate)
            {
                toggle.onValueChanged.AddListener((isOn) => OnToggleActivated(currentStep, isOn));
            }

            // Ativa o nextButton apenas se não houver outras condições
            if (currentStep.buttonsToClick.Count == 0 && currentStep.togglesToActivate.Count == 0 && currentStep.targetUIElement == null)
            {
                nextButton.interactable = true;
            }
            else
            {
                nextButton.interactable = false;
            }
        }
    }

    // Método para adicionar listener ao targetUIElement
    void AddTargetListener(RectTransform targetElement)
    {
        Button targetButton = targetElement.GetComponent<Button>();
        if (targetButton != null)
        {
            targetButton.onClick.AddListener(NextStep);
            return;
        }

        Toggle targetToggle = targetElement.GetComponent<Toggle>();
        if (targetToggle != null)
        {
            targetToggle.onValueChanged.AddListener((isOn) =>
            {
                if (isOn)
                {
                    NextStep();
                }
            });
        }
    }

    // Método chamado ao clicar em um dos botões do passo
    void OnButtonClicked(TutorialStep step)
    {
        step.buttonsClicked++;
        UpdateNextButtonState(step);

        // Alalytics
        AnalyticsSystem.AddAnalyticTutorialTime_Seconds(this.name, "Next step tutorial", AnalyticsManager.GetTutorialTime());
        AnalyticsSystem.AddAnalyticTutorialTime_Formated(this.name, "Next step tutorial", AnalyticsManager.GetTutorialTime());
    }

    // Método chamado ao ativar um dos toggles do passo
    void OnToggleActivated(TutorialStep step, bool isOn)
    {
        if (isOn)
        {
            step.togglesActivated++;
        }
        else
        {
            step.togglesActivated--;
        }
        UpdateNextButtonState(step);
    }

    // Atualiza o estado do botão "Próximo"
    void UpdateNextButtonState(TutorialStep step)
    {
        if (step.buttonsClicked >= step.buttonsToClick.Count && step.togglesActivated >= step.togglesToActivate.Count)
        {
            nextButton.interactable = true; // Ativa o nextButton quando todos os botões e toggles forem acionados
        }


    }

    // Método chamado ao clicar no botão "Próximo" ou no targetUIElement
    void NextStep()
    {
        // Remove o listener do targetUIElement do passo atual
        RemoveTargetListener();

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

    void RemoveTargetListener()
    {
        if (tutorialSteps[currentStepIndex].targetUIElement != null)
        {
            Button targetButton = tutorialSteps[currentStepIndex].targetUIElement.GetComponent<Button>();
            if (targetButton != null)
            {
                targetButton.onClick.RemoveListener(NextStep);
            }

            Toggle targetToggle = tutorialSteps[currentStepIndex].targetUIElement.GetComponent<Toggle>();
            if (targetToggle != null)
            {
                targetToggle.onValueChanged.RemoveAllListeners();
            }
        }
    }

    // Método para finalizar o tutorial
    void EndTutorial()
    {
        dialogueTextUI.gameObject.SetActive(false);
        arrowImage.gameObject.SetActive(false);
        nextButton.gameObject.SetActive(false);
        tutorialGameObject.SetActive(false);

        tutirialCompleted = true;

        // Alalytics
        AnalyticsManager.SetStartTutorial(false);
        AnalyticsSystem.AddAnalyticTutorialTime_Seconds(this.name, "End tutorial", AnalyticsManager.GetTutorialTime());
        AnalyticsSystem.AddAnalyticTutorialTime_Formated(this.name, "End tutorial", AnalyticsManager.GetTutorialTime());

        // MenuManager
        MenuManager.openedMenu = false;
    }

    // Método para encerrar o tutorial completamente, para ser chamado por um botão
    public void ExitTutorial()
    {
        // Desativa o jogo do tutorial e seus elementos visuais
        tutorialGameObject.SetActive(false);
        dialogueTextUI.gameObject.SetActive(false);
        arrowImage.gameObject.SetActive(false);
        nextButton.gameObject.SetActive(false);

        // Remove os listeners de todos os elementos associados ao tutorial
        foreach (var step in tutorialSteps)
        {
            foreach (var button in step.buttonsToClick)
            {
                button.onClick.RemoveAllListeners();
            }

            foreach (var toggle in step.togglesToActivate)
            {
                toggle.onValueChanged.RemoveAllListeners();
            }

            if (step.targetUIElement != null)
            {
                Button targetButton = step.targetUIElement.GetComponent<Button>();
                if (targetButton != null)
                {
                    targetButton.onClick.RemoveAllListeners();
                }

                Toggle targetToggle = step.targetUIElement.GetComponent<Toggle>();
                if (targetToggle != null)
                {
                    targetToggle.onValueChanged.RemoveAllListeners();
                }
            }
        }

        // Redefine o progresso do tutorial
        currentStepIndex = 0;

        tutirialCompleted = true;

        // Alalytics
        AnalyticsManager.SetStartTutorial(false);
        AnalyticsSystem.AddAnalyticTutorialTime_Seconds(this.name, "Tutorial skiped", AnalyticsManager.GetTutorialTime());
        AnalyticsSystem.AddAnalyticTutorialTime_Formated(this.name, "Tutorial skiped", AnalyticsManager.GetTutorialTime());

        // MenuManager
        MenuManager.openedMenu = false;

        Debug.Log("Tutorial encerrado completamente.");
    }

}