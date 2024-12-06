using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardManager : MonoBehaviour
{
    [SerializeField] private InventoryManager inventoryManager;

    public void GiveMoney(int value)
    {
        if (value != 0) MoneyManager.AddMoney_Static(value);
    }

    public void GiveIDS(int ecologico, int economico, int social)
    {
        if (ecologico != 0) IDS.AddEcologico(ecologico);

        if (economico != 0) IDS.AddEconomico(economico);

        if (social != 0) IDS.AddSocial(social);
    }

    public void GivePlant(Item item, int quantity)
    {
        if (item != null) inventoryManager.playerInventory.AddItem(item, quantity);
    }
}
