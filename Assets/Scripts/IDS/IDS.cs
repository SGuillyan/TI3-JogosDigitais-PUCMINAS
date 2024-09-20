using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEngine;

public class IDS : MonoBehaviour
{
    [Header("�ndicie de Desenvolvimento Sustent�vel")] 
    [SerializeField] private int ids;

    [Space(10)]

    [Header("�ndicies")]
    [Range(0, 100)]
    [SerializeField] private int ecologico;
    [Range(0, 100)]
    [SerializeField] private int economico;
    [Range(0, 100)]
    [SerializeReference] private int social;

    [Space(5)]

    [Header("Pesos")]
    [Range(0, 10)]
    [SerializeField] private int pesoEcologico;
    [Range(0, 10)]
    [SerializeField] private int pesoEconomico;
    [Range(0, 10)]
    [SerializeField] private int pesoSocial;

    

    #region // Get & Set

    public int GetEcologico()
    {
        return this.ecologico;
    }

    public int GetEconomico()
    {
        return this.economico;
    }

    public int GetSocial()
    {
        return this.social;
    }

    #endregion

    #region // Add & Reduce

    public void AddEcologico(int value)
    {
        if (this.ecologico < 100)
        {
            this.ecologico += value;
        }
        CalcularIDS();
    }
    public void ReduceEcologico(int value)
    {
        if (this.ecologico > 0)
        {
            this.ecologico -= value;
        }
        CalcularIDS();
    }

    public void AddEconomico(int value)
    {
        if (this.economico < 100)
        {
            this.economico += value;
        }
        CalcularIDS();
    }
    public void ReduceEconomico(int value)
    {
        if (this.economico > 0)
        {
            this.economico -= value;
        }
        CalcularIDS();
    }

    public void AddSocial(int value)
    {
        if (this.social < 100)
        {
            this.social += value;
        }
        CalcularIDS();
    }
    public void ReduceSocial(int value)
    {
        if (this.social > 0)
        {
            this.social -= value;
        }
        CalcularIDS();
    }

    #endregion

    private void CalcularIDS()
    {
        // Soma Ponderada: soma considerando o peso de cada termo
        int pesoTotal = pesoEcologico + pesoEconomico + pesoSocial;
        float   pe = pesoEcologico / pesoTotal, 
                pc = pesoEconomico / pesoTotal, 
                ps = pesoSocial / pesoTotal;
        float somaPonderada = (pe * ecologico) + (pc * economico) + (ps * social);

        // Desvio padr�o: m�dia da disper��o termos
        float media = (ecologico + economico + social) / 3;
        float   de = Mathf.Pow(pesoEcologico - media, 2),
                dc = Mathf.Pow(pesoEconomico - media, 2),
                ds = Mathf.Pow(pesoSocial - media, 2);
        float desvioPadrao = Mathf.Sqrt((de + dc + ds) / 3);

        // Equil�brio: usando o desvio padr�o dividido pelo valor m�ximo poss�vel de um termo pode-se determinar o qu�o pr�ximos, equilibrados, eles est�o
        float penalizacao = 1 - (desvioPadrao / 100);
        
        // a soma ponderada indica o quanto o jogador consegue deixar os �ndicies altos e a penaliza��o o quanto ele deixou os �ndicies equilibrados, multiplicando os dois n�s temos o IDS
        ids = (int) (somaPonderada * penalizacao);
    }
}
