using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Material Configurations")]
    [SerializeField] private Material materialToChange;

    void Start()
    {
        MoneyManager moneyManager = FindObjectOfType<MoneyManager>();
        moneyManager.InitializeMoney(5000);
    }

    void Update()
    {
        //UpdateMaterialColor();
    }

    public void UpdateMaterialColor()
    {
        int ecologico = IDS.GetEcologico();
        float normalizedValue = Mathf.Clamp01((20f - ecologico) / 20f);
        Color initialColor = new Color(0f, 0.51f, 1f);
        Color targetColor = Color.Lerp(initialColor, Color.green, normalizedValue);
        materialToChange.color = targetColor;
    }
}
