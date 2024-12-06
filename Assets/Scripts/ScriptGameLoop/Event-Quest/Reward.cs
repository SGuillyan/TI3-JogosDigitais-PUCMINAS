using UnityEngine;

[System.Serializable]
public class Reward
{
    public int moneyAmount;
    public int ecologicoValor;
    public int economicoValor;
    public int socialValor;

    public Reward(int moneyAmountFun = 0, int ecologicoValorFun = 0, int economicoValorFun = 0, int socialValorFun = 0)
    {
        this.moneyAmount = moneyAmountFun;
        this.ecologicoValor = ecologicoValorFun;
        this.economicoValor = economicoValorFun;
        this.socialValor = socialValorFun;
    }

    public void GiveReward(MoneyManager moneyManager)
    {
        // Adiciona dinheiro ao jogador
        if (moneyAmount > 0 && moneyManager != null)
        {
            moneyManager.AddMoney(moneyAmount);
        }

        // Ajusta os índices do IDS
        IDS.AddEcologico(ecologicoValor);
        IDS.AddEconomico(economicoValor);
        IDS.AddSocial(socialValor);
    }
}
