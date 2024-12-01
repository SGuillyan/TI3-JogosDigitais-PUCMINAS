using UnityEngine;
using UnityEngine.UI;

public class IDSUI : MonoBehaviour
{
    [Header("Imagens de Preenchimento")]
    public Image ecologicoImage;
    public Image economicoImage;
    public Image socialImage;

    private void Start()
    {
        AtualizarPreenchimento();
    }

    private void Update()
    {
        AtualizarPreenchimento();
    }

    private void AtualizarPreenchimento()
    {
        ecologicoImage.fillAmount = IDS.GetEcologico() / 100f;
        economicoImage.fillAmount = IDS.GetEconomico() / 100f;
        socialImage.fillAmount = IDS.GetSocial() / 100f;
    }
}
