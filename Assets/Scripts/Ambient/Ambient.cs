using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Ambient")]
public class Ambient : ScriptableObject
{
    public enum Temperature
    {
        None,
        Frio,
        Fresco,
        Ameno,
        Aquientado,
        Calor,
    }

    public enum Climate
    {
        None,
        Chuvoso,
        Nublado,
        Limpo,
        Semiarido,
        Seco,
    }

    public Temperature currentTemperature;
    public Climate currentClimate;

    [SerializeField] private int modifyTemperature;
    [SerializeField] private int modifyClimate;

    public void ModifyAmbient(Ambient mainAmbient)
    {
        currentTemperature = mainAmbient.currentTemperature + modifyTemperature;
        if (currentTemperature == 0)
        {
            currentTemperature = Temperature.Frio;
        }

        currentClimate = mainAmbient.currentClimate + modifyClimate;
        if (currentClimate == 0)
        {
            currentClimate = Climate.Chuvoso;
        }
    }
}
