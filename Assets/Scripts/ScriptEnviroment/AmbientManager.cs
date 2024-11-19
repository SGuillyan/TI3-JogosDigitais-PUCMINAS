using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AmbientManager : MonoBehaviour
{
    public enum Season
    {
        Summer,
        Autumn,
        Winter,
        Spring,
    }

    private static Season currentSeason;
    private static float seasonalFactor;

    [Tooltip("A cada X segundos a temperatura tem a possibilidade de ser atualizada, X = temperatureUpdateTime")]
    [Min(1)]
    [SerializeField] private float temperatureUpdateTime = 60;
    [Tooltip("Quanto maior o valor menor a chance de atualizar a temperatura, chance = 100 / updateChance")]
    [SerializeField] private int updateChange = 1;

    private void Start()
    {
        currentSeason = Season.Summer;
        seasonalFactor = 0f;
    }

    private void Update()
    {
        if (Time.realtimeSinceStartup % temperatureUpdateTime == 0)
        {
            if (Random.Range(0, updateChange) == 0)
            {
                UpdateTemperature();
            }

            UpdateClimate();
        }
        else if (Time.realtimeSinceStartup % Mathf.RoundToInt(temperatureUpdateTime / 2) == 0)
        {
            UpdateClimate();
        }
    }

    #region // Get & Set

    public static Season GetCurrentSeason()
    {
        return currentSeason;
    }

    public static float GetSeasonalFactor()
    {
        return seasonalFactor;
    }

    #endregion

    private void UpdateTemperature()
    {
        UpdateSeasonalFactor();

        float variableFactor = 0;
        switch (Ambient.GetCurrentTemperature())
        {
            case Ambient.Temperature.Algido:
                variableFactor += 1;
                break;
            case Ambient.Temperature.Gelado:
                variableFactor += 0.5f;
                break;
            case Ambient.Temperature.Calor:
                variableFactor -= 0.5f;
                break;
            case Ambient.Temperature.Escaldadante:
                variableFactor -= 1;
                break;
        }

        int instabilityFactor = ClimateInstabilityLevel();
        float changeFactor = Random.Range(-5f, 5f) + seasonalFactor + variableFactor + (instabilityFactor / 15);

        if (changeFactor > 0f)
        {
            Ambient.ChangeTemperature(instabilityFactor);
        }
        else
        {
            Ambient.ChangeTemperature(-instabilityFactor);
        }
    }

    private void UpdateClimate()
    {
        float changeFactor = Random.Range(-5f, 5f);

        switch (currentSeason)
        {
            case Season.Summer:
                changeFactor -= 1f;
                break;
            case Season.Autumn:
                changeFactor += 0.5f;
                break;
            case Season.Winter:
                changeFactor += 1f;
                break;
            case Season.Spring:
                changeFactor -= 0.5f;
                break;
        }
        switch (Ambient.GetCurrentClimate())
        {
            case Ambient.Climate.Tempestuoso:
                changeFactor += 1;
                break;
            case Ambient.Climate.Chuvoso:
                changeFactor += 0.5f;
                break;
            case Ambient.Climate.Semiarido:
                changeFactor += 0.5f;
                break;
            case Ambient.Climate.Arido:
                changeFactor -= 1;
                break;
        }

        if (changeFactor > 0f)
        {
            Ambient.ChangeClimate(ClimateInstabilityLevel());
        }
        else
        {
            Ambient.ChangeClimate(-ClimateInstabilityLevel());
        }
    }

    private void UpdateSeasonalFactor()
    {
        switch (currentSeason)
        {
            case Season.Summer:
                
                if (seasonalFactor < 1.5f)
                {
                    seasonalFactor += Random.Range(0.1f, 0.3f);
                }
                else
                {
                    seasonalFactor += Random.Range(-0.1f, 0.4f);
                }

                if (seasonalFactor >= 3f)
                {
                    currentSeason = Season.Autumn;
                }

                break;
            case Season.Autumn:

                if (seasonalFactor > 0f)
                {
                    seasonalFactor -= Random.Range(0.5f, 1f);
                }
                else
                {
                    seasonalFactor -= Random.Range(-0.05f, 0.3f);
                }

                if (seasonalFactor <= -1f)
                {
                    currentSeason = Season.Winter;
                }

                break;
            case Season.Winter:

                if (seasonalFactor > -1.5f)
                {
                    seasonalFactor -= Random.Range(0.1f, 0.3f);
                }
                else
                {
                    seasonalFactor -= Random.Range(-0.1f, 0.4f);
                }

                if (seasonalFactor >= 3f)
                {
                    currentSeason = Season.Spring;
                }

                break;
            case Season.Spring:

                if (seasonalFactor < 0f)
                {
                    seasonalFactor += Random.Range(0.5f, 1f);
                }
                else
                {
                    seasonalFactor += Random.Range(-0.05f, 0.3f);
                }

                if (seasonalFactor >= 1f)
                {
                    currentSeason = Season.Summer;
                }

                break;
        }
    }

    private int ClimateInstabilityLevel()
    {
        switch (IDS.GetEcologico() / GlobalWarmingScale())
        {
            case > 0.75f:
                return Random.Range(1, 3);
            case > 0.5f:
                return 1;
            case > 0.25f:
                return Random.Range(1, 4);
            default: 
                return Random.Range(2, 4);
        }
    }

    private float GlobalWarmingScale()
    {
        float a = IDS.IndiciesSoma();
        float b = (a / 100) * IDS.DesvioPadrao();

        return a - b;
    }
}
