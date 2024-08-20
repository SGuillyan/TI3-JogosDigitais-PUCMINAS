using UnityEngine;

public class MoneyManager : MonoBehaviour
{
    private int currentMoney;  // Armazena o dinheiro atual do jogador

    // Evento que pode ser usado para notificar mudanças no dinheiro (ex: para atualizar a UI)
    public delegate void OnMoneyChanged(int newAmount);
    public event OnMoneyChanged onMoneyChanged;

    // Inicializa o dinheiro do jogador
    public void InitializeMoney(int startingMoney)
    {
        currentMoney = startingMoney;
        NotifyMoneyChanged();
    }

    // Adiciona dinheiro ao saldo atual
    public void AddMoney(int amount)
    {
        currentMoney += amount;
        NotifyMoneyChanged();
    }

    // Remove dinheiro do saldo atual (se possível)
    public bool SpendMoney(int amount)
    {
        if (currentMoney >= amount)
        {
            currentMoney -= amount;
            NotifyMoneyChanged();
            return true;
        }
        else
        {
            Debug.LogWarning("Dinheiro insuficiente!");
            return false;
        }
    }

    // Retorna o saldo atual do jogador
    public int GetCurrentMoney()
    {
        return currentMoney;
    }

    // Método para notificar que o dinheiro mudou (para atualizar a UI, por exemplo)
    private void NotifyMoneyChanged()
    {
        if (onMoneyChanged != null)
        {
            onMoneyChanged(currentMoney);
        }
    }
}
