using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestManager_novo : MonoBehaviour
{
    public List<Quest_novo> availableQuests = new List<Quest_novo>();
    public List<Quest_novo> activeQuests = new List<Quest_novo>();
    public List<Quest_novo> completedQuests = new List<Quest_novo>();

    private const int maxActiveQuests = 3;

    [Header("Access")]
    public InventoryManager inventoryManager;
    [SerializeField] private GameObject questButtomContent;

    // event quest
    private bool countEventQuest = false;
    private float eventQuestTime;
    private Reward_novo eventQuestReward;

    private void Start()
    {
        CompleteActiveQuests();
    }

    private void Update()
    {
        if (countEventQuest)
        {
            if (eventQuestTime <= 0f)
            {
                RewardManager.GiveReward(eventQuestReward);
                countEventQuest = false;
            }
            else eventQuestTime -= Time.deltaTime;
        }
    }

    /*public void ActivateQuest(QuestSO quest)
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
            MoneyManager.AddMoney_Static(quest.moneyReward);
        }
    }*/


    private void RandomInstanceQuests()
    {
        int rand = UnityEngine.Random.Range(0, availableQuests.Count);

        Instantiate(availableQuests[rand].gameObject, questButtomContent.transform);
    }

    private void AtualizeAvailableQuests()
    {
        for (int i = 0; i < activeQuests.Count; i++)
        {
            if (availableQuests.Contains(activeQuests[i]))
            {
                availableQuests.Remove(activeQuests[i]);
            }
        }
    }

    public void CompleteActiveQuests()
    {
        for (int i = activeQuests.Count; i < maxActiveQuests; i++)
        {
            RandomInstanceQuests();
            AtualizeAvailableQuests();
        }
    }


    public void ResetEventQuest()
    {
        countEventQuest = false;
    }

    public void StartEventQuest(float time, Reward_novo reward)
    {
        countEventQuest = true;
        eventQuestTime = time;
        eventQuestReward = reward;

    }
}
