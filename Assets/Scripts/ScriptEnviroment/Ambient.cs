using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Ambient
{
    public enum Temperature
    {
        None,
        Algido, // álgido
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
 /*max*/Mormaco, // mormaço
        Semiarido, // semiárido
        Arido, // árido
    }

    private static Temperature currentTemperature = Temperature.Ameno;
    private static Climate currentClimate = Climate.Limpo;

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
        if (currentTemperature <= 0)
        {
            currentTemperature = Temperature.Algido;
        }
        if ((int)currentTemperature >= 10)
        {
            currentTemperature = Temperature.Escaldadante;
        }
    }

    public static void ChangeClimate(int modify)
    {
        currentClimate = currentClimate + modify;
        if (currentClimate <= 0)
        {
            currentClimate = Climate.Limpo;
        }
        if ((int)currentClimate >= 10)
        {
            currentClimate = Climate.Arido;
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
