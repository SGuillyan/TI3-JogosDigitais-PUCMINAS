using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskManager : MonoBehaviour
{
    public List<Quest> quests;
    public MoneyManager moneyManager; // Referência ao MoneyManager

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
