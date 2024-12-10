using System;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    [SerializeField, Tooltip("Tempo para o salvamento periódico (segundos)"), Min(30)] private int periodicSavingTime = 60;
    private float _periodicSavingTime;
    private float periodicSaving_tt;

    private void Start()
    {
        _periodicSavingTime = periodicSavingTime;
        periodicSaving_tt = _periodicSavingTime;

        if (!SaveSystem.isFirstTime())
        {
            SaveSystem.Load();
            Debug.Log("Deu load corretamente!");
        }

        //Debug.Log(Application.persistentDataPath + "/SaveData.json");
    }

    private void Update()
    {
        if (_periodicSavingTime <= 0)
        {
            SaveSystem.Save();

            _periodicSavingTime = periodicSaving_tt;
        }
        else
        {
            _periodicSavingTime -= Time.deltaTime;
        }
    }

    public void SaveAndSendAnalytics()
    {
        SaveSystem.Save();
        //AnalyticsSystem.SendEmail(AnalyticsSystem.GenerateAnalyticsJsonReport());
    }

    public void ButtomSave()
    {
        SaveSystem.Save();
    }

    [ContextMenu("Save")]
    private void TestSave()
    {
        SaveSystem.Save();
    }
    [ContextMenu("Load")]
    private void TestLoad()
    {
        SaveSystem.Load();
    }
}
