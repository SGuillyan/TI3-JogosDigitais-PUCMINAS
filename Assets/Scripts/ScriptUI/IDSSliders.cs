using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class IDSUI : MonoBehaviour
{
    [Header("Imagens de Preenchimento")]
    public Image ecologicoImage;
    public TextMeshProUGUI ecologicoText;
    public Image economicoImage;
    public TextMeshProUGUI economicoText;
    public Image socialImage;
    public TextMeshProUGUI socialText;
    public TextMeshProUGUI IDS_Text;

    private void Start()
    {
        AtualizarPreenchimento();
        IDS.CalcularIDS();
    }

    private void Update()
    {
        AtualizarPreenchimento();
    }

    private void AtualizarPreenchimento()
    {
        ecologicoImage.fillAmount = IDS.GetEcologico() / 100f;
        ecologicoText.text = IDS.GetEcologico().ToString();
        economicoImage.fillAmount = IDS.GetEconomico() / 100f;
        economicoText.text = IDS.GetEconomico().ToString();
        socialImage.fillAmount = IDS.GetSocial() / 100f;
        socialText.text = IDS.GetSocial().ToString();
        IDS_Text.text = IDS.GetIDS().ToString();
    }
}
