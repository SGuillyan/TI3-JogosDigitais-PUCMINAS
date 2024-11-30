using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CarouselLanguageSelector : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private Image image;
    [SerializeField] private string[] languageList;
    [SerializeField] private Sprite[] flagSprits;
    private int page = 0;

    private void Start()
    {
        AtualizarPagina();
    }

    void AtualizarPagina()
    {
        text.text = languageList[page];
        image.sprite = flagSprits[page];
    }

    public void Anterior()
    {
        if (page > 0) page -= 1;
        else page = languageList.Length - 1;

        AtualizarPagina();
    }

    public void Proximo()
    {
        if (page < languageList.Length - 1) page += 1;
        else page = 0;

        AtualizarPagina();
    }
}
