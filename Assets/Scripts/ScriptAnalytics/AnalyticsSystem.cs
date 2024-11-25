using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Runtime.CompilerServices;
using UnityEngine;

public class AnalyticsSystem : MonoBehaviour
{
    [HideInInspector]
    public AnalyticsFile file;
    
    public static AnalyticsSystem instance { get; private set; }

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        if (instance == this)
        {
            file = new AnalyticsFile();
            file.data1 = new List<AnalyticsData<int>>();
            file.data2 = new List<AnalyticsData<bool>>();
        }

        AddAnalyticData1("Level", "N° of obstacles", 12);
        AddAnalyticData2("Player", "Avalable to walk", true);
        AddAnalyticData2("Enemy", "Alive", true);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            AddAnalyticData2("Player", "Jump", true);
            AddAnalyticData2("Enemy", "Alive", false);
            Debug.Log("FOii");
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            SaveAnalyticsFile("Assets/Scripts/ScriptAnalytics/AnalyticsData.json");
            Debug.Log("Salvo");
        }

    }

    // Métodos Públicos
    public void AddAnalyticData1(string sender, string track, int value)
    {
        AnalyticsData<int> data = new AnalyticsData<int>(sender, track, value);
        file.data1.Add(data);
    }
    public void AddAnalyticData2(string sender, string track, bool value)
    {
        AnalyticsData<bool> data = new AnalyticsData<bool>(sender, track, value);
        file.data2.Add(data);
    }

    public string GenerateAnalyticsJsonReport()
    {
        file.saveData = SaveSystem.GenerateSaveData();
        return JsonUtility.ToJson(file, true);
    }

    public void SaveAnalyticsFile(string path)
    {
        string json = GenerateAnalyticsJsonReport();
        File.WriteAllText(path, json);
    }


    [Serializable]
    public class AnalyticsData<T>
    {
        public string sender;
        public string time;
        public string track;
        public T value;

        // Construtor
        public AnalyticsData(string sender, string track, T value)
        {
            this.sender = sender;
            this.time = FormatTimeFromSeconds(Time.realtimeSinceStartup);
            this.track = track;
            this.value = value;
        }

        // Métodos Privados
        private string FormatTimeFromSeconds(float seconds)
        {
            int minutes = (int)seconds / 60;
            int intseconds = (int)seconds % 60;

            seconds = (seconds - (int)seconds) * 100;
            int miliseconds = ((int)seconds * 6) / 10;

            int hours = 0;
            if (minutes >= 60)
            {
                hours = minutes / 60;
                minutes %= 60;
            }

            return hours + ":" + minutes + ":" + intseconds + ":" + miliseconds;
        }
    }

    [Serializable]
    public class AnalyticsFile
    {
        public List<AnalyticsData<int>> data1;
        public List<AnalyticsData<bool>> data2;

        public SaveSystem.SaveData saveData;

        // Construtor
        /*public AnalyticsFile()
        {
            data = new List<AnalyticsData>();
        }*/
    }
}
