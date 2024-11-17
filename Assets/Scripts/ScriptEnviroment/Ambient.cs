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
        Geada,
        Chuvoso,
 /*min*/Nublado,
        Umido,
 /*---*/Limpo, 
        Seco,
 /*max*/Mormaco, // mormaço
        Semiarido, // semiárido
        Arido, // árido
    }

    public static Temperature currentTemperature;
    public static Climate currentClimate;

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
