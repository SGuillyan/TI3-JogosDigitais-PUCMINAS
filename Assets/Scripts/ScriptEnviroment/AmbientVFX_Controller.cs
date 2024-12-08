using System;
using UnityEngine;

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
    [SerializeField, Tooltip("Cor de tempo comum")] private Color defaultColor = new Color(1f, 0.9568627450980392f, 0.8392156862745098f);
    [Space(5)]
    [SerializeField, Tooltip("Cor de tempestade")] private Color heavyRainColor = new Color(0.4117647058823529f, 0.4705882352941176f, 0.607843137254902f);
    [SerializeField, Tooltip("Cor de garoa")] private Color drizzleColor = new Color(0.7254901960784314f, 0.7450980392156863f, 0.8823529411764706f);
    [SerializeField, Tooltip("Cor de levemente ensolarado")] private Color lowSunsetColor = new Color(1f, 0.9215686274509804f, 0.7254901960784314f);
    [SerializeField, Tooltip("Cor de muito ensolarado")] private Color hightSunsetColor = new Color(0.9803921568627451f, 0.9607843137254902f, 0.5294117647058824f);

    [Header("Timer")]
    [SerializeField, Min(10)] private float changeTime = 10;
    private float change_tt;
    private bool isChanging = false;

    // Changing
    private float currentRateOverTime;
    private Color currentLightColor;
    private float newRateOverTime;
    private Color newLightColor;

    private void Start()
    {
        //ModifyWeather();

        emission = _particleSystem.emission;
        change_tt = changeTime;
    }

    private void Update()
    {
        if (isChanging) 
        { 
            if (changeTime > 0)
            {
                //var emission = _particleSystem.emission;
                emission.rateOverTime = rainIntensity;

                changeTime -= Time.deltaTime;
            }
            else
            {
                currentRateOverTime = newRateOverTime;
                currentLightColor = newLightColor;

                isChanging = false;
                changeTime = change_tt;
            }
        }
    }

    public void ChangeWeather()
    {
        switch (Ambient.GetCurrentClimate())
        {
            case Ambient.Climate.Tempestuoso:
                ModifyWeather(heavyRainColor, stormIntensity);
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
    }

    private void ModifyWeather(Color lightColor, float rateOverTime = 0)
    {
        /*var emission = _particleSystem.emission;
        emission.rateOverTime = rainIntensity;*/

        newLightColor = lightColor;
        newRateOverTime = rateOverTime;
    }

}
