using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Reward
{
    public int money;
    [Tooltip("[ecologico, economico, social]")] public int[] ids = new int[3];
    public PlantTile plant;
    public int quantity;
}
