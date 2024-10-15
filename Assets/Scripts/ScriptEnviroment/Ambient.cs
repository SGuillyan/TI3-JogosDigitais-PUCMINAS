using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Ambient")]
public class Ambient : ScriptableObject
{
    public enum Temperature
    {
        None,
        algido, // �lgido
        Gelado,
 /*min*/Frio,
        Fresco,
 /*---*/Ameno,
        Morno,
 /*max*/Aquientado,
        Calor,
        Escaldadante,
    }

    public enum Climate
    {
        None,
        Geada,
        Chuvoso,
 /*min*/Nublado,
        Umido,
 /*---*/Limpo, 
        Seco,
 /*max*/Mormaco, // morma�o
        Semiarido, // semi�rido
        Arido, // �rido
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
