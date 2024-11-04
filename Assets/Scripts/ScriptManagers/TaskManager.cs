using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // Certifique-se de incluir este namespace para usar TextMeshPro

public class TaskManager : MonoBehaviour
{
    public List<Quest> quests;
    public MoneyManager moneyManager;
    public Canvas questCanvas;
    public GameObject questUIPrefab;
    public GameObject spawnTask;

    void Start()
    {
        // Cria e adiciona uma nova Quest simples
        Quest simpleQuest = new Quest
        {
            title = "Primeira Missão",
            description = "Colete 5 recursos para completar esta missão.",
            isCompleted = false,
            objectives = new List<Objective>
            {
                new Objective
                {
                    description = "Coletar 5 recursos",
                    isCompleted = false,
                    type = ObjectiveType.Collect,
                    targetAmount = 5,
                    currentAmount = 0
                }
            },
            reward = new Reward(moneyAmountFun: 100, ecologicoValorFun: 10, economicoValorFun: 5, socialValorFun: 8)
        };

        AddQuest(simpleQuest);
    }

    public void ShowQuestCanvas()
    {
        questCanvas.gameObject.SetActive(true); // Ativa o Canvas

        // Instancia o prefab no spawnTask
        GameObject questUIInstance = Instantiate(questUIPrefab, spawnTask.transform);

        // Atribui o título da tarefa ao texto de EventName do prefab de UI
        if (quests.Count > 0)
        {
            Quest currentQuest = quests[0]; // Seleciona a primeira quest ou altere conforme necessário

            // Referencia o componente TextMeshProUGUI do prefab instanciado
            TextMeshProUGUI questTitleText = questUIInstance.transform.Find("EventName").GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI questDescriText = questUIInstance.transform.Find("EventDescription").GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI questRewardText = questUIInstance.transform.Find("EventReward").GetComponent<TextMeshProUGUI>();

            // Atribui o título da quest
            questTitleText.text = currentQuest.title;
            questDescriText.text = currentQuest.description;

        }
    }

    public void HideQuestCanvas()
    {
        questCanvas.gameObject.SetActive(false); // Desativa o Canvas
    }

    public void AddQuest(Quest newQuest)
    {
        quests.Add(newQuest);
    }

    public void UpdateQuestProgress(Objective objective)
    {
        // Atualize o progresso de objetivos, se necessário
    }

    public void CompleteQuest(Quest quest)
    {
        quest.isCompleted = true;

        // Concede a recompensa ao jogador
        if (quest.reward != null && moneyManager != null)
        {
            quest.reward.GiveReward(moneyManager);
        }
    }
}
