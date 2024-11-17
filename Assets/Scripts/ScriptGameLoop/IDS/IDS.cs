using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEngine;

public static class IDS
{
    [Header("�ndicie de Desenvolvimento Sustent�vel")]
    [SerializeField] private static int ids;

    [Space(10)]

    [Header("�ndicies")]
    [Range(0, 100)]
    [SerializeField] private static int ecologico = 3;
    [Range(0, 100)]
    [SerializeField] private static int economico = 4;
    [Range(0, 100)]
    [SerializeReference] private static int social = 5;

    [Space(5)]

    [Header("Pesos")]
    [Range(0, 10)]
    [SerializeField] private static int pesoEcologico;
    [Range(0, 10)]
    [SerializeField] private static int pesoEconomico;
    [Range(0, 10)]
    [SerializeField] private static int pesoSocial;

    #region // Get & Set

    public static int GetEcologico()
    {
        return ecologico;
    }

    public static int GetEconomico()
    {
        return economico;
    }

    public static int GetSocial()
    {
        return social;
    }

    #endregion

    #region // Add & Reduce

    public static void AddEcologico(int value)
    {
        if (ecologico < 100)
        {
            ecologico += value;
        }
        CalcularIDS();
    }
    public static void ReduceEcologico(int value)
    {
        if (ecologico > 0)
        {
            ecologico -= value;
        }
        CalcularIDS();
    }

    public static void AddEconomico(int value)
    {
        if (economico < 100)
        {
            economico += value;
        }
        CalcularIDS();
    }
    public static void ReduceEconomico(int value)
    {
        if (economico > 0)
        {
            economico -= value;
        }
        CalcularIDS();
    }

    public static void AddSocial(int value)
    {
        if (social < 100)
        {
            social += value;
        }
        CalcularIDS();
    }
    public static void ReduceSocial(int value)
    {
        if (social > 0)
        {
            social -= value;
        }
        CalcularIDS();
    }

    #endregion

    public static void CalcularIDS()
    {
        // Soma Ponderada: soma considerando o peso de cada termo
        int pesoTotal = pesoEcologico + pesoEconomico + pesoSocial;
        float pe = pesoEcologico / (float)pesoTotal;
        float pc = pesoEconomico / (float)pesoTotal;
        float ps = pesoSocial / (float)pesoTotal;
        float somaPonderada = (pe * ecologico) + (pc * economico) + (ps * social);

        // Desvio padr�o: m�dia da disper��o termos
        float media = (ecologico + economico + social) / 3f;
        float de = Mathf.Pow(pesoEcologico - media, 2);
        float dc = Mathf.Pow(pesoEconomico - media, 2);
        float ds = Mathf.Pow(pesoSocial - media, 2);
        float desvioPadrao = Mathf.Sqrt((de + dc + ds) / 3);

        // Equil�brio: usando o desvio padr�o dividido pelo valor m�ximo poss�vel de um termo pode-se determinar o qu�o pr�ximos, equilibrados, eles est�o
        float penalizacao = 1 - (desvioPadrao / 100);

        // a soma ponderada indica o quanto o jogador consegue deixar os �ndicies altos e a penaliza��o o quanto ele deixou os �ndicies equilibrados, multiplicando os dois n�s temos o IDS
        ids = (int)(somaPonderada * penalizacao);
    }
}
