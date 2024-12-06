using UnityEngine;

[CreateAssetMenu(fileName = "NewQuest", menuName = "Game Data/Quest")]

public class QuestSO : ScriptableObject 
{
    [Header("Basic Info")]

    public string questName;
    public string description;

    [Header("Requirements")]

    public int currentPlantedCrops;
    public int currentHarvestedCrops;
    public int currentMoneyGot;

    [Header("Rewards")]

    public int ecologicReward;
    public int economicReward;
    public int socialReward;
    public int moneyReward;
}
