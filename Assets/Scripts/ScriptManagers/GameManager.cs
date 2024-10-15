using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        MoneyManager moneyManager = FindObjectOfType<MoneyManager>();
        moneyManager.InitializeMoney(1000);  // Começa com 1000 unidades de dinheiro
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
