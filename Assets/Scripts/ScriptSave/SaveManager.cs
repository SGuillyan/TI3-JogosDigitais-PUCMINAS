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

        //SaveSystem.Load();
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
}
