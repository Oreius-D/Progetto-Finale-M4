using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPickupReceiver : MonoBehaviour, ITimeReceiver, ICoinReceiver
{
    [Header("References")]
    [SerializeField] private Timer timer;
    [SerializeField] private CoinCollector coinCollector;

    private void Awake()
    {
        if (timer == null)
            timer = FindObjectOfType<Timer>(); // fallback, solo se ti dimentichi di assegnarlo
        if (coinCollector == null) 
            coinCollector = GetComponentInChildren<CoinCollector>();
    }

    public void AddTime(float amount)
    {
        if (timer == null) return;
        timer.AddTime(amount);
    }

    public void AddCoins(int amount)
    {
        if (coinCollector == null) return;
        coinCollector.AddCoins(amount);
    }
}
