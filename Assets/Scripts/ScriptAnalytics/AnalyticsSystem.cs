using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class AnalyticsSystem : MonoBehaviour
{
    [HideInInspector]
    public static AnalyticsFile file;
    
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
        /*if(Input.GetKeyDown(KeyCode.Space))
        {
            //AddAnalyticData2("Player", "Jump", true);
            //AddAnalyticData2("Enemy", "Alive", false);
            Debug.Log("FOii");
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            SaveAnalyticsFile("Assets/Scripts/ScriptAnalytics/AnalyticsData.json");
            Debug.Log("Salvo");
        }*/
    }

    #region // Add Analytics

    #region // Tutorial Time
    public static void AddAnalyticTutorialTime_Seconds(string sender, string track, float value)
    {
        AnalyticsData<float> data = new AnalyticsData<float>(sender, track, value);
        file.tutorialTime_Seconds.Add(data);
    }

    public static void AddAnalyticTutorialTime_Formated(string sender, string track, float value)
    {
        AnalyticsData<string> data = new AnalyticsData<string>(sender, track, FormatTimeFromSeconds(value));
        file.tutorialTime_Formated.Add(data);
    }
    #endregion

    #region // Inative Time
    public static void AddAnalyticInativeTime_Seconds(string sender, string track, float value)
    {
        AnalyticsData<float> data = new AnalyticsData<float>(sender, track, value);
        file.inativeTime_Seconds.Add(data);
    }

    public static void AddAnalyticInativeTime_Formated(string sender, string track, float value)
    {
        AnalyticsData<string> data = new AnalyticsData<string>(sender, track, FormatTimeFromSeconds(value));
        file.inativeTime_Formated.Add(data);
    }
    #endregion

    #region // Plants
    public static void AddAnalyticPlants_Bought(string sender, string plant, int value)
    {
        file.totalPlantsBought += value;
        AnalyticsData<int> data = new AnalyticsData<int>(sender, plant + " bought", value);
        file.plants.Add(data);
    }

    public static void AddAnalyticPlants_Planted(string sender, string plant, int value)
    {
        file.totalPlantsPlanted += value;
        AnalyticsData<int> data = new AnalyticsData<int>(sender, plant + " planted", value);
        file.plants.Add(data);
    }

    public static void AddAnalyticPlants_Harvested(string sender, string plant, int value)
    {
        file.totalPlantsHarvested += value;
        AnalyticsData<int> data = new AnalyticsData<int>(sender, plant + " harvested", value);
        file.plants.Add(data);
    }

    public static void AddAnalyticPlants_Sold(string sender, string plant, int value)
    {
        file.totalPlantsSold += value;
        AnalyticsData<int> data = new AnalyticsData<int>(sender, plant + " sold", value);
        file.plants.Add(data);
    }
    #endregion

    #region // Lands
    public static void AddAnalyticLands_Plowed(string sender, Vector3 position)
    {
        file.totalLandsPlowed += 1;
        file.currentLandsPlowed += 1;
        AnalyticsData<Vector3> data = new AnalyticsData<Vector3>(sender, "Tile plowed", position);
        file.lands.Add(data);
    }

    // não chamado ainda
    public static void AddAnalyticLands_Flated(string sender, Vector3 position)
    {
        file.totalLandsFlated += 1;
        file.currentLandsPlowed -= 1;
        AnalyticsData<Vector3> data = new AnalyticsData<Vector3>(sender, "Tile flated", position);
        file.lands.Add(data);
    }

    public static void AddAnalyticLands_Hectare(string sender, Vector3 position)
    {
        file.hectaresBought += 1;
        AnalyticsData<Vector3> data = new AnalyticsData<Vector3>(sender, "Hectare bought", position);
        file.lands.Add(data);
    }
    #endregion

    public static void AddAnalyticInfo(string sender, string track, int[] value)
    {
        file.infosConsulted += 1;
        AnalyticsData<int[]> data = new AnalyticsData<int[]>(sender, track, value);
        file.info.Add(data);
    }

    #region // Devastation
    // não chamado ainda
    public static void AddAnalyticDevastation_Tree(string sender, Vector3 position)
    {
        file.totalTreesCuted += 1;
        AnalyticsData<Vector3> data = new AnalyticsData<Vector3>(sender, "Tree cuted", position);
        file.devastation.Add(data);
    }
    // não chamado ainda
    public static void AddAnalyticDevastation_River(string sender, Vector3 position)
    {
        file.timesRiverPolluted += 1;
        AnalyticsData<Vector3> data = new AnalyticsData<Vector3>(sender, "River polluted", position);
        file.devastation.Add(data);
    }
    #endregion

    #endregion

    #region // Report

    public static string GenerateAnalyticsJsonReport()
    {
        file.gameTime_Seconds = Time.realtimeSinceStartup;
        file.gameTime_Formated = FormatTimeFromSeconds(file.gameTime_Seconds);

        file.saveData = SaveSystem.GenerateSaveData();

        Debug.Log("Relatório json gerado!");
        return JsonUtility.ToJson(file, true);
    }

    public static void SaveAnalyticsFile(string path)
    {
        string json = GenerateAnalyticsJsonReport();
        File.WriteAllText(path, json);
    }

    #endregion

    // Métodos Públicos
    public static void SendEmail(string message)
    {
        /*var client = new SmtpClient("smtp.gmail.com", 587)
        {
            Credentials = new NetworkCredential("bazonsamuel@gmail", "bubo dcqi uxcx gsiv"),
            EnableSsl = true
        };
        client.Send("bazonsamuel@gmail", "jordandlyon@gmail.com", "Analytics " + DateTime.Now.ToString("cc/MM/yyyy HH:mm"), mensage);
        Debug.Log("Email de analytics enviado!");*/

        var client = new SmtpClient("smtp.gmail.com", 587);
        MailMessage mail = new MailMessage();

        client.Timeout = 10000;
        client.DeliveryMethod = SmtpDeliveryMethod.Network;
        client.UseDefaultCredentials = false;
        client.Port = 587;

        mail.From = new MailAddress("bazonsamuel@gmail");
        mail.To.Add(new MailAddress("jordandlyon@gmail.com"));
        mail.Subject = "Analytics " + DateTime.Now.ToString("cc/MM/yyyy HH:mm");
        mail.Body = message;

        // bubo dcqi uxcx gsiv
        // ulmy gxsk sgbl qcpf
        client.Credentials = new NetworkCredential("bazonsamuel@gmail", "ulmy gxsk sgbl qcpf");
        client.EnableSsl = true;

        ServicePointManager.ServerCertificateValidationCallback = 
            delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
            { return true; };

        mail.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;

        client.Send(mail);
        Debug.Log("Email de analytics enviado!");

        

        /*mail.From = new MailAddress(sender);
        mail.To.Add(receiver);
        mail.Subject = "R4UL " + title;
        mail.Body = textBody;

        Debug.Log("Connecting to SMTP server");
        SmtpClient smtpServer = new SmtpClient("smtp.gmail.com");
        smtpServer.Port = 587;
        smtpServer.Credentials = new System.Net.NetworkCredential(sender, password) as ICredentialsByHost;

        smtpServer.EnableSsl = true;
        ServicePointManager.ServerCertificateValidationCallback =
                delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
                { return true; };
        Debug.Log("Sending message");

        smtpServer.Send(mail);*/
    }

    // Métodos Privados
    private static string FormatTimeFromSeconds(float seconds)
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
