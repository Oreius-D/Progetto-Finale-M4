using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

//enum to define number of coins in the game
public enum CoinAmount
{
    TotalCoins = 80,
    StartingCoins = 0
}

public class CoinCollector : MonoBehaviour
{
    // reference to the TextMeshProUGUI component that will display the coin count
    [SerializeField] private TextMeshProUGUI coinText;
    // amount of coins in the game
    [SerializeField] private int amountToWin = 80;

    // set coin amount to 0
    private int startCoinAmount = 0;

    private void OnTriggerEnter(Collider other)
    {
        // if the player collides with a coin
        if (other.CompareTag("Coin"))
        {
            // increase the coin amount by 1
            startCoinAmount++;

            // update the coin text UI
            coinText.text = "" + startCoinAmount + "/" + amountToWin;

            // call win screen when player collects all coins
            if (startCoinAmount >= amountToWin)
            {
                // Show win screen and pause the game
                FindObjectOfType<PauseManager>().SetPaused(true); // Call the SetPaused method from the PauseManager to pause the game
                FindObjectOfType<PauseManager>().win(true); // Call the win method from the PauseManager to show the win screen
            }

            // deactivate the coin object instead of destroying it to improve performance
            other.gameObject.SetActive(false);

            // print the current coin amount to the console
           // Debug.Log("Coins collected: " + coinAmount);
        }
    }
}
