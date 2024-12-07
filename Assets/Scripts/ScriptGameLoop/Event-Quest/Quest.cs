using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Quest : MonoBehaviour
{
    public string questName;
    public bool daily = true;

    //[Header("Access")]
    //[SerializeField] private RewardManager rewardManager;
    private QuestManager manager;

    [Header("Components")]
    //public string title;
    //[TextArea] public string description;
    //public Sprite image;
    public PlantTile require;
    public int quantityRequire;
    public Reward reward;

    [Header("Event")]
    [Tooltip("Tempo habil para finalizar a tarefa (segundos)"), Min(10)] public float gapTime = 10;

    private void Start()
    {
        manager = transform.GetComponentInParent<QuestManager>();
    }

    public void CheckRequire()
    {
        InventoryItem[] aux = manager.inventoryManager.playerInventory.items.ToArray();

        for (int i = 0; i < aux.Length; i++)
        {
            if (aux[i].item == require.harvestedItem)
            {
                if (aux[i].quantity >= quantityRequire)
                {
                    if (daily) GiveReward();
                    else manager.ResetEventQuest();

                    foreach (Quest q in manager.activeQuests)
                    {
                        if (q.questName == this.questName)
                        {
                            manager.activeQuests.Remove(q);
                            manager.availableQuests.Add(q);
                            break;
                        }
                    }
                    
                    Destroy(gameObject);
                }
            }
        }
    }

    private void GiveReward()
    {
        RewardManager.GiveReward(reward);
    }
}
