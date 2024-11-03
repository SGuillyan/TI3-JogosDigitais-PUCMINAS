using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskManager : MonoBehaviour
{
    public List<Quest> quests;
    public MoneyManager moneyManager;

    public void AddQuest(Quest newQuest)
    {
        quests.Add(newQuest);
    }

    public void UpdateQuestProgress(Objective objective)
    {

    }

    public void CompleteQuest(Quest quest)
    {
        quest.isCompleted = true;

        if (quest.reward != null && moneyManager != null)
        {
            quest.reward.GiveReward(moneyManager);
        }
    }
}
