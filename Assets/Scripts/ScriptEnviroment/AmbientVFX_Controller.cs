using System;
using System.Data;
using UnityEngine;
using UnityEngine.Rendering;

public class AmbientVFX_Controller : MonoBehaviour
{
    [Header("Access")]
    [SerializeField] private ParticleSystem _particleSystem;
    [SerializeField] private Light _light;
    ParticleSystem.EmissionModule emission;

    [Header("Rain")]
    [SerializeField] private float stormIntensity = 200;
    [SerializeField] private float rainIntensity = 80;
    [SerializeField] private float drizzleIntensity = 5;

    [Header("Light Colors")]
    [SerializeField, Tooltip("Cor de tempo comum")] private Color defaultColor = new Color(1f, 0.9568627450980392f, 0.8392156862745098f, 1f);
    [Space(5)]
    [SerializeField, Tooltip("Cor de tempestade")] private Color stormColor = new Color(0.4117647058823529f, 0.4705882352941176f, 0.607843137254902f, 1f);
    [SerializeField, Tooltip("Cor de garoa")] private Color drizzleColor = new Color(0.7254901960784314f, 0.7450980392156863f, 0.8823529411764706f, 1f);
    [SerializeField, Tooltip("Cor de levemente ensolarado")] private Color lowSunsetColor = new Color(1f, 0.9215686274509804f, 0.7254901960784314f, 1f);
    [SerializeField, Tooltip("Cor de muito ensolarado")] private Color hightSunsetColor = new Color(0.9803921568627451f, 0.9607843137254902f, 0.5294117647058824f, 1f);

    [Header("Day Time")]
    [SerializeField/*, Min(1.5f)*/] private float maxDayLightIntensity = 1.5f;
    private float realTimeNow;
    [Tooltip("hora em que luz começa a aparecer")]
    [SerializeField, Range(0, 6)] private int dawnStartHour = 1;
    [Tooltip("hora em que luz chega no máximo de sua intensidade")]
    [SerializeField, Range(7, 11)] private int solarZenithHour = 11;
    [Tooltip("hora em que luz começa a diminuir")]
    [SerializeField, Range(12, 18)] private int DeclineStartHour = 13;
    [Tooltip("hora em que luz chga no máximo de sua escuridão")]
    [SerializeField, Range(17, 23)] private int lunarZenithHour = 23;

    [Header("Lightning")]
    [SerializeField] private Color lightningColor = new Color(0.9803921568627451f, 0.9803921568627451f, 0.7843137254901961f);
    [SerializeField, Range(0f, 1f)] private float lightningDurationTime;
    private float lightningDuration_tt;
    [SerializeField] private float lightningGapTime;
    private float lightningGap_tt;
    private bool isLightning = false;
    [SerializeField, Min(3)] private int lightningChance = 3;

    [Header("Timer")]
    [SerializeField, Min(10)] private float changeTime = 10;
    private float change_tt;
    private bool isChanging = false;

    // Changing
    private float currentRateOverTime;
    private Color currentLightColor;
    private float newRateOverTime;
    private Color newLightColor;

    /*[Header("Sound Effects")]
    [SerializeField] private AudioClip drizzleSound;
    [SerializeField] private AudioClip stormSound;
    [SerializeField] private AudioClip thunderSound;*/

    private void Start()
    {
        // Access
        emission = _particleSystem.emission;

        // Day Time
        realTimeNow = (((int)DateTime.Now.TimeOfDay.TotalMinutes) / 60f);

        // Lightning
        lightningDuration_tt = lightningDurationTime;
        lightningGap_tt = lightningGapTime;

        // Timer
        change_tt = changeTime;

        // Changing
        currentRateOverTime = 0;
        currentLightColor = defaultColor;
        emission.rateOverTime = currentRateOverTime;
        _light.color = currentLightColor; 

        // Sound Effects
    }

    private void Update()
    {
        if (isChanging) 
        { 
            if (changeTime > 0)
            {
                emission.rateOverTime = Mathf.Lerp(currentRateOverTime, newRateOverTime, 1f - (changeTime/ change_tt));
                _light.color = Color.Lerp(currentLightColor, newLightColor, 1f - (changeTime / change_tt));
                _light.color = new Color(_light.color.r, _light.color.g, _light.color.b, 1f);

                changeTime -= Time.deltaTime;
            }
            else
            {
                emission.rateOverTime = newRateOverTime;
                _light.color = newLightColor;

                currentRateOverTime = newRateOverTime;
                currentLightColor = newLightColor;

                isChanging = false;
            }
        }

        #region // Lightning

        if (Ambient.GetCurrentClimate() == Ambient.Climate.Tempestuoso && changeTime <= 0)
        {
            if (lightningGapTime <= 0)
            {
                if (!isLightning)
                {
                    if (UnityEngine.Random.Range(0, lightningChance) == 0)
                    {
                        _light.color = lightningColor;
                        isLightning = true;
                    }
                    else
                    {
                        isLightning = true;
                        lightningGapTime = lightningGap_tt;
                    }
                }
                else
                {
                    if (lightningDurationTime <= 0)
                    {
                        _light.color = currentLightColor;
                        isLightning = false;

                        lightningDurationTime = lightningDuration_tt;
                        lightningGapTime = lightningGap_tt;
                    }
                    else lightningDurationTime -= Time.deltaTime;
                }
            }
            else lightningGapTime -= Time.deltaTime;
        }

        #endregion

        #region // Real Time Light

        realTimeNow = (((int)DateTime.Now.TimeOfDay.TotalMinutes) / 60f);
        //Debug.Log(realTimeNow);

        // Changing light intensity
        _light.transform.rotation = Quaternion.Euler(_light.transform.rotation.eulerAngles.x, realTimeNow * 30f, _light.transform.rotation.eulerAngles.z);

        // Changing light intensity

        if (realTimeNow > dawnStartHour && realTimeNow < solarZenithHour)
        {
            // AM
            _light.intensity = Mathf.Lerp(0f, maxDayLightIntensity, (realTimeNow - dawnStartHour) / 10);
        }
        else if (realTimeNow > DeclineStartHour && realTimeNow < lunarZenithHour)
        {
            // PM
            _light.intensity = maxDayLightIntensity - (Mathf.Lerp(0f, maxDayLightIntensity,(realTimeNow - DeclineStartHour) / 10));
        }
        else if (realTimeNow < dawnStartHour || realTimeNow > lunarZenithHour) _light.intensity = 0;
        else if (realTimeNow > solarZenithHour && realTimeNow < DeclineStartHour) _light.intensity = maxDayLightIntensity;

        #endregion
    }

    public void ChangeWeather()
    {
        switch (Ambient.GetCurrentClimate())
        {
            case Ambient.Climate.Tempestuoso:
                ModifyWeather(stormColor, stormIntensity);
                break;
            case Ambient.Climate.Chuvoso:
                ModifyWeather(drizzleColor, rainIntensity);
                break;
            case Ambient.Climate.Nublado:
                ModifyWeather(drizzleColor, drizzleIntensity);
                break;
            case Ambient.Climate.Umido:
                ModifyWeather(defaultColor, drizzleIntensity);
                break;
            case Ambient.Climate.Limpo:
                ModifyWeather(defaultColor);
                break;
            case Ambient.Climate.Seco:
                ModifyWeather(defaultColor);
                break;
            case Ambient.Climate.Mormaco:
                ModifyWeather(lowSunsetColor);
                break;
            case Ambient.Climate.Semiarido:
                ModifyWeather(lowSunsetColor);
                break;
            case Ambient.Climate.Arido:
                ModifyWeather(hightSunsetColor);
                break;
            default:
                ModifyWeather(defaultColor);
                break;
        }

        changeTime = change_tt;
        isChanging = true;
    }

    private void ModifyWeather(Color lightColor, float rateOverTime = 0)
    {
        newLightColor = lightColor;
        newRateOverTime = rateOverTime;
    }

}
