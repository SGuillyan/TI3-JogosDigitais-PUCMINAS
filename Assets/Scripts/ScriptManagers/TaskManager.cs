using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    void OnEnable()
    {
        Instantiate(questUIPrefab, spawnTask.transform);
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

    public void ShowQuestCanvas()
    {
        questCanvas.gameObject.SetActive(true); // Ativa o Canvas
    }

    public void HideQuestCanvas()
    {
        questCanvas.gameObject.SetActive(false); // Desativa o Canvas
    }
}
