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

    public void CompostingRequire()
    {
        InventoryItem[] aux = manager.inventoryManager.playerInventory.items.ToArray();
        int quantity = 0;

        for (int i = 0; i < aux.Length; i++)
        {
            quantity += aux[i].quantity;

            if (quantity >= quantityRequire)
            {
                quantity = quantityRequire;
                for (int j = 0; j < aux.Length; j++)
                {
                    /*if (quantity - aux[j].quantity >= 0)
                    {
                        manager.inventoryManager.playerInventory.items.Remove(aux[j]);
                        quantity -= aux[j].quantity;
                    }
                    else
                    {
                        aux[j].quantity -= quantity;
                        break;
                    }*/

                    if (quantity >= aux[j].quantity)
                    {
                        quantity -= aux[j].quantity;
                        manager.inventoryManager.playerInventory.items.Remove(aux[j]);

                        Debug.Log("(1) " + quantity.ToString() + " " + aux[j].quantity.ToString());
                    }
                    else if (quantity < aux[j].quantity)
                    {
                        aux[j].quantity -= quantity;
                        quantity = 0;
                        Debug.Log("(2) " + quantity.ToString() + " " + aux[j].quantity.ToString());
                        break;
                    }
                }

                GiveReward();

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

    private void GiveReward()
    {
        RewardManager.GiveReward(reward);
    }
}
