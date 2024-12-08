using System;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class EventManager : MonoBehaviour
{
    [Header("Access")]
    [SerializeField] private QuestManager questManager;

    [Header("Event UI")]
    [SerializeField] private GameObject eventUI;
    [SerializeField] private GameObject titleWarning;
    [SerializeField] private GameObject titleTask;
    [SerializeField] private TextMeshProUGUI description;
    [SerializeField] private TextMeshProUGUI bottomText1;
    [SerializeField] private TextMeshProUGUI bottomText2;

    [Header("Events")]
    [SerializeField] private Event[] eventsWarning;
    [SerializeField] private Event[] eventsTask;
    [SerializeField, Min(600)] private float eventCallTimer = 600;
    private float eventCall_tt;

    // Event Warning
    private float warningTimer;

    private void Start()
    {
        eventCall_tt = eventCallTimer;
    }

    private void Update()
    {
        if (warningTimer <= 0)
        {
            if (!questManager.GetCountEventQuest())
            {
                if (eventCallTimer <= 0)
                {
                    int rand;
                    Event aux;
                    if (UnityEngine.Random.Range(0, 5) == 0)
                    {
                        rand = UnityEngine.Random.Range(0, eventsWarning.Length);
                        aux = eventsWarning[rand];
                        ActiveEventUI(aux);

                        AmbientManager.ambientChange = false;
                        Ambient.SetCurrentTemperature(aux.warningTemperature);
                        Ambient.SetCurrentClimate(aux.warningClimate);
                    }
                    else
                    {
                        rand = UnityEngine.Random.Range(0, eventsTask.Length);
                        aux = eventsTask[rand];
                        ActiveEventUI(aux);

                        questManager.AddQuest(aux.eventQuest);
                    }

                    eventCallTimer = eventCall_tt;
                }
                else eventCallTimer -= Time.deltaTime;
            }

            if (warningTimer > -1)
            {
                AmbientManager.ambientChange = true;
                warningTimer = -2;
            }
        }
        else warningTimer -= Time.deltaTime;
        
    }

    private void ActiveEventUI(Event evento)
    {
        eventUI.SetActive(true);

        if (evento.warning) titleWarning.SetActive(true);
        else titleTask.SetActive(true);

        description.text = evento.description;
        bottomText1.text = evento.text1;
        bottomText2.text = evento.text2;

        AudioManager.PlaySound(SoundType.EVENTSTART);
    }

    public void DesableEventUI()
    {
        eventUI.SetActive(false);

        titleWarning.SetActive(false);
        titleTask.SetActive(false);
    }
}
