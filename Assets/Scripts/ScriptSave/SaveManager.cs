using System;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    [SerializeField, Tooltip("Tempo para o salvamento periódico (segundos)"), Min(15)]
    private int periodicSavingTime = 60;
    private float _periodicSavingTime;
    private float periodicSaving_tt;

    private void Start()
    {
        _periodicSavingTime = periodicSavingTime;
        periodicSaving_tt = _periodicSavingTime;

        SaveSystem.Save();
        Debug.Log(Application.persistentDataPath + "/SaveData.json");
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
        AnalyticsSystem.SendEmail(AnalyticsSystem.GenerateAnalyticsJsonReport());
    }

    // Adiciona um botão no Inspector para chamar Load()
    [ContextMenu("Load Game")]
    public void LoadGame()
    {
        SaveSystem.Load();
        Debug.Log("Jogo carregado com sucesso!");
    }
}
