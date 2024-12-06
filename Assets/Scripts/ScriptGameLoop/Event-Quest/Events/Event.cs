using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Event")]
public class Event : ScriptableObject
{
    public bool warning = false;

    [Header("Components")]
    [TextArea] public string body;
    public string text1;
    public string text2;

    [Header("Task")]
    public Quest eventQuest;

    [Header("Warning")]
    public float warningTime;
    public Ambient.Temperature warningTemperature;
    public Ambient.Climate warningClimate;
}
