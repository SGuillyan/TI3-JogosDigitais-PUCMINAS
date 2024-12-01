using UnityEngine;
using UnityEngine.UI;

public class ThermometerController : MonoBehaviour
{
    [SerializeField] private Image thermometerFill;

    void Update()
    {
        UpdateThermometer(Ambient.GetCurrentTemperature());
    }

    private void UpdateThermometer(Ambient.Temperature temperature)
    {
        switch (temperature)
        {
            case Ambient.Temperature.Algido:
                thermometerFill.fillAmount = 0.1f;
                break;
            case Ambient.Temperature.Gelado:
                thermometerFill.fillAmount = 0.2f;
                break;
            case Ambient.Temperature.Frio:
                thermometerFill.fillAmount = 0.3f;
                break;
            case Ambient.Temperature.Fresco:
                thermometerFill.fillAmount = 0.4f;
                break;
            case Ambient.Temperature.Ameno:
                thermometerFill.fillAmount = 0.5f;
                break;
            case Ambient.Temperature.Morno:
                thermometerFill.fillAmount = 0.6f;
                break;
            case Ambient.Temperature.Aquientado:
                thermometerFill.fillAmount = 0.7f;
                break;
            case Ambient.Temperature.Calor:
                thermometerFill.fillAmount = 0.8f;
                break;
            case Ambient.Temperature.Escaldadante:
                thermometerFill.fillAmount = 1f;
                break;
            default:
                thermometerFill.fillAmount = 0f;
                break;
        }
    }
}
