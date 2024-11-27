using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Runtime.CompilerServices;
using System.Xml.Schema;
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
            //file.data1 = new List<AnalyticsData<int>>();
            //file.data2 = new List<AnalyticsData<bool>>();

            file.tutorialTime_Seconds = new List<AnalyticsData<float>>();
            file.tutorialTime_Formated = new List<AnalyticsData<string>>();
            file.inativeTime_Seconds = new List<AnalyticsData<float>>();
            file.inativeTime_Formated = new List<AnalyticsData<string>>();
            file.plants = new List<AnalyticsData<int>>();
            file.lands = new List<AnalyticsData<Vector3>>();
            file.info = new List<AnalyticsData<int[]>>();
            file.devastation = new List<AnalyticsData<Vector3>>();
        }
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            //AddAnalyticData2("Player", "Jump", true);
            //AddAnalyticData2("Enemy", "Alive", false);
            Debug.Log("FOii");
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            SaveAnalyticsFile("Assets/Scripts/ScriptAnalytics/AnalyticsData.json");
            Debug.Log("Salvo");
        }
    }

    #region // Add Analytics

    #region // Tutorial Time
    public void AddAnalyticTutorialTime_Seconds(string sender, string track, float value)
    {
        AnalyticsData<float> data = new AnalyticsData<float>(sender, track, value);
        file.tutorialTime_Seconds.Add(data);
    }

    public void AddAnalyticTutorialTime_Formated(string sender, string track, float value)
    {
        AnalyticsData<string> data = new AnalyticsData<string>(sender, track, FormatTimeFromSeconds(value));
        file.tutorialTime_Formated.Add(data);
    }
    #endregion

    #region // Inative Time
    public void AddAnalyticInativeTime_Seconds(string sender, string track, float value)
    {
        AnalyticsData<float> data = new AnalyticsData<float>(sender, track, value);
        file.inativeTime_Seconds.Add(data);
    }

    public void AddAnalyticInativeTime_Formated(string sender, string track, float value)
    {
        AnalyticsData<string> data = new AnalyticsData<string>(sender, track, FormatTimeFromSeconds(value));
        file.inativeTime_Formated.Add(data);
    }
    #endregion

    #region // Plants
    public void AddAnalyticPlants_Bought(string sender, string plant, int value)
    {
        file.totalPlantsBought += value;
        AnalyticsData<int> data = new AnalyticsData<int>(sender, plant + " bought", value);
        file.plants.Add(data);
    }

    public void AddAnalyticPlants_Planted(string sender, string plant, int value)
    {
        file.totalPlantsPlanted += value;
        AnalyticsData<int> data = new AnalyticsData<int>(sender, plant + " planted", value);
        file.plants.Add(data);
    }

    public void AddAnalyticPlants_Harvested(string sender, string plant, int value)
    {
        file.totalPlantsHarvested += value;
        AnalyticsData<int> data = new AnalyticsData<int>(sender, plant + " harvested", value);
        file.plants.Add(data);
    }

    public void AddAnalyticPlants_Sold(string sender, string plant, int value)
    {
        file.totalPlantsSold += value;
        AnalyticsData<int> data = new AnalyticsData<int>(sender, plant + " sold", value);
        file.plants.Add(data);
    }
    #endregion

    #region // Lands
    public void AddAnalyticLands_Plowed(string sender, Vector3 position)
    {
        file.totalLandsPlowed += 1;
        file.currentLandsPlowed += 1;
        AnalyticsData<Vector3> data = new AnalyticsData<Vector3>(sender, "Tile plowed", position);
        file.lands.Add(data);
    }

    public void AddAnalyticLands_Flated(string sender, Vector3 position)
    {
        file.totalLandsFlated += 1;
        file.currentLandsPlowed -= 1;
        AnalyticsData<Vector3> data = new AnalyticsData<Vector3>(sender, "Tile flated", position);
        file.lands.Add(data);
    }

    public void AddAnalyticLands_Hectare(string sender, Vector3 position)
    {
        AnalyticsData<Vector3> data = new AnalyticsData<Vector3>(sender, "Hectare bought", position);
        file.lands.Add(data);
    }
    #endregion

    public void AddAnalyticInfo(string sender, string track, int[] value)
    {
        file.infosConsulted += 1;
        AnalyticsData<int[]> data = new AnalyticsData<int[]>(sender, track, value);
        file.info.Add(data);
    }

    #region // Devastation
    public void AddAnalyticDevastation_Tree(string sender, Vector3 position)
    {
        file.totalTreesCuted += 1;
        AnalyticsData<Vector3> data = new AnalyticsData<Vector3>(sender, "Tree cuted", position);
        file.devastation.Add(data);
    }

    public void AddAnalyticDevastation_River(string sender, Vector3 position)
    {
        file.timesRiverPolluted += 1;
        AnalyticsData<Vector3> data = new AnalyticsData<Vector3>(sender, "River polluted", position);
        file.devastation.Add(data);
    }
    #endregion

    #endregion

    #region // Report

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

    #endregion

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



    [Serializable]
    public class AnalyticsFile
    {
        public float gameTime_Seconds;
        public string gameTime_Formated;

        public List<AnalyticsData<float>> tutorialTime_Seconds;
        public List<AnalyticsData<string>> tutorialTime_Formated;

        public List<AnalyticsData<float>> inativeTime_Seconds;
        public List<AnalyticsData<string>> inativeTime_Formated;

        public int totalPlantsBought;
        public int totalPlantsPlanted;
        public int totalPlantsHarvested;
        public int totalPlantsSold;
        public List<AnalyticsData<int>> plants;

        public int totalLandsPlowed;
        public int totalLandsFlated;
        public int currentLandsPlowed;
        public int hectaresBought;
        public List<AnalyticsData<Vector3>> lands;

        public int infosConsulted;
        public List<AnalyticsData<int[]>> info;

        public int totalTreesCuted;
        public int timesRiverPolluted;
        public List<AnalyticsData<Vector3>> devastation;

        //public List<AnalyticsData<???>> quests;
        //public List<AnalyticsData<???>> events;

        public SaveSystem.SaveData saveData;

        // Construtor
        /*public AnalyticsFile()
        {
            this.saveData = SaveSystem.Load("Assets/Scripts/ScriptSave/SaveData.json");
        }*/
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
}
