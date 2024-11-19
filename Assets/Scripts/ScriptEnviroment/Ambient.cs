using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Ambient
{
    public enum Temperature
    {
        None,
        Algido, // �lgido
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
        Tempestuoso,
        Chuvoso,
 /*min*/Nublado,
        Umido,
 /*---*/Limpo, 
        Seco,
 /*max*/Mormaco, // morma�o
        Semiarido, // semi�rido
        Arido, // �rido
    }

    private static Temperature currentTemperature;
    private static Climate currentClimate;

    public static Temperature GetCurrentTemperature()
    {
        return currentTemperature;
    }

    public static Climate GetCurrentClimate()
    {
        return currentClimate;
    }

    public static void ChangeTemperature(int modify)
    {
        currentTemperature = currentTemperature + modify;
        if (currentTemperature == 0)
        {
            currentTemperature = Temperature.Algido;
        }
    }

    public static void ChangeClimate(int modify)
    {
        currentClimate = currentClimate + modify;
        if (currentClimate == 0)
        {
            currentClimate = Climate.Limpo;
        }
    }

    /*[SerializeField] private int modifyTemperature;
    [SerializeField] private int modifyClimate;

    public void ModifyAmbient(Ambient mainAmbient)
    {
        currentTemperature = mainAmbient.currentTemperature + modifyTemperature;
        if (currentTemperature == 0)
        {
            currentTemperature = Temperature.Algido;
        }

        currentClimate = mainAmbient.currentClimate + modifyClimate;
        if (currentClimate == 0)
        {
            currentClimate = Climate.Limpo;
        }
    }*/
}
