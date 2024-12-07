using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestManager : MonoBehaviour
{
    public List<Quest> availableQuests = new List<Quest>();
    public List<Quest> activeQuests = new List<Quest>();
    public List<Quest> completedQuests = new List<Quest>();

    private const int maxActiveQuests = 3;

    [Header("Access")]
    public InventoryManager inventoryManager;
    [SerializeField] private GameObject questButtomContent;

    // event quest
    private bool countEventQuest = false;
    private float eventQuestTime;
    private Reward eventQuestReward;

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

    // Get & Set
    public bool GetCountEventQuest()
    {
        return countEventQuest;
    }


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

    public void AddQuest(Quest quest)
    {
        // conferir a necessidade de se usar o prefeb básico de quest
        Instantiate(quest.gameObject, questButtomContent.transform);
        activeQuests.Add(quest);
        StartEventQuest(quest.gapTime, quest.reward);
    }


    public void ResetEventQuest()
    {
        countEventQuest = false;
    }

    public void StartEventQuest(float time, Reward reward)
    {
        countEventQuest = true;
        eventQuestTime = time;
        eventQuestReward = reward;

    }
}
