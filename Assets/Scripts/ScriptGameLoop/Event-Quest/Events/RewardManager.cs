using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardManager : MonoBehaviour
{
    [SerializeField] private InventoryManager _inventoryManager;
    private static InventoryManager inventoryManager;

    private void Start()
    {
        inventoryManager = _inventoryManager;
    }

    private static void GiveMoney(int value)
    {
        if (value != 0) MoneyManager.AddMoney_Static(value);
    }

    private static void GiveIDS_Add(int ecologico, int economico, int social)
    {
        if (ecologico != 0) IDS.AddEcologico(ecologico);

        if (economico != 0) IDS.AddEconomico(economico);

        if (social != 0) IDS.AddSocial(social);
    }

    private static void GiveIDS_Reduce(int ecologico, int economico, int social)
    {
        if (ecologico != 0) IDS.ReduceEcologico(ecologico);

        if (economico != 0) IDS.ReduceEconomico(economico);

        if (social != 0) IDS.ReduceSocial(social);
    }

    private static void GivePlant(Item item, int quantity)
    {
        if (item != null) inventoryManager.playerInventory.AddItem(item, quantity);
    }

    public static void GiveReward(Reward_novo reward)
    {
        if (reward.money != 0) GiveMoney(reward.money);

        if (reward.ids.Length == 3)
        {
            if (reward.ids[0] >= 0 && reward.ids[1] >= 0 && reward.ids[2] >= 0)
            {
                GiveIDS_Add(reward.ids[0], reward.ids[1], reward.ids[2]);
            }
            if (reward.ids[0] <= 0 && reward.ids[1] <= 0 && reward.ids[2] <= 0)
            {
                GiveIDS_Reduce(reward.ids[0], reward.ids[1], reward.ids[2]);
            }
        }

        if (reward.plant != null) GivePlant(reward.plant.harvestedItem, reward.quantity);
    }
}
