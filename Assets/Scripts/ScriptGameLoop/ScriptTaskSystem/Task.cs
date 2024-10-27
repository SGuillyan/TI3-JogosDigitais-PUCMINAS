using System.Collections.Generic;

[System.Serializable]
public class Quest
{
    public string title;
    public string description;
    public bool isCompleted;
    public List<Objective> objectives;
    public Reward reward;
}
