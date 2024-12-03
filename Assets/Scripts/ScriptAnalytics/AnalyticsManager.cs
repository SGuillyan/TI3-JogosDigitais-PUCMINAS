using System;
using System.Collections;
//using UnityEditor.Localization.Editor;
using UnityEngine;
//using UnityEngine.Localization.Settings;

public class AnalyticsManager : MonoBehaviour
{
    // Tutorial
    private static bool startTutorial = false;
    private static float tutorialTime;

    // Input
    private static float inativeTime;
    

    private void Start()
    {
        
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            Debug.Log("Input verificado!");
            AnalyticsSystem.SendEmail(AnalyticsSystem.GenerateAnalyticsJsonReport());
        }

        // Tutorial
        if (startTutorial)
        {
            tutorialTime += Time.deltaTime;
        }
    }

    #region // Métodos Públicos

    // Tutorial
    public static void SetStartTutorial(bool value)
    {
        startTutorial = value;
    }

    public static float GetTutorialTime()
    {
        return tutorialTime;
    }

    // Input
    public static void AddInativeTime()
    {
        inativeTime = Time.deltaTime;
    }

    public static void ResetInativeTime()
    {
        inativeTime = 0;
    }

    public static float GetInativeTime()
    {
        return inativeTime;
    }

    #endregion
}
