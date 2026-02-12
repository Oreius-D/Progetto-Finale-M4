using TMPro;
using UnityEngine;

public class CoinCollector : MonoBehaviour
{
    [Header("Coin Collection")]
    [SerializeField] private int maxCoins = 1;

    public TextMeshProUGUI coinText;

    private int coinAmount = 0;
    
    public void AddCoins(int amount)
    {
        coinAmount += amount;
        coinText.text = coinAmount + "/" + maxCoins;

        if (coinAmount >= maxCoins)
        {
            FindObjectOfType<PauseManager>().SetPaused(true);
            FindObjectOfType<PauseManager>().ShowWinScreen();
        }
    }
}

