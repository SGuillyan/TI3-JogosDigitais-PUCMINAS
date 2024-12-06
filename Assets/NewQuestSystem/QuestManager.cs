using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public List<QuestSO> availableQuests = new List<QuestSO>();
    public List<QuestSO> activeQuests = new List<QuestSO>();
    public List<QuestSO> completedQuests = new List<QuestSO>();

    public MoneyManager moneyManager;

    private const int maxActiveQuests = 3;

    public void ActivateQuest(QuestSO quest)
    {
        if (activeQuests.Count >= maxActiveQuests)
        {
            return;
        }

        if (availableQuests.Contains(quest))
        {
            availableQuests.Remove(quest);
            activeQuests.Add(quest);
        }
    }

    public void CompleteQuest(QuestSO quest)
    {
        if (activeQuests.Contains(quest))
        {
            activeQuests.Remove(quest);
            completedQuests.Add(quest);

            IDS.AddEcologico(quest.ecologicReward);
            IDS.AddEconomico(quest.economicReward);
            IDS.AddSocial(quest.socialReward);
            moneyManager.AddMoney(quest.moneyReward);
        }
    }
}
