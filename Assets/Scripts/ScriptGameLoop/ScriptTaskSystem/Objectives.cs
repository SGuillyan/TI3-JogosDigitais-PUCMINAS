[System.Serializable]
public class Objective
{
    public string description;
    public bool isCompleted;
    public ObjectiveType type;
    public int targetAmount;
    public int currentAmount;
}

public enum ObjectiveType
{
    Collect,
    Build
}
