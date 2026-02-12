using UnityEngine;

[CreateAssetMenu(menuName = "Pickups/Add Coins Effect")]
public class AddCoinsEffect : PickupEffect
{
    [SerializeField] private int coins = 1;

    public override bool Apply(GameObject receiver)
    {
        var coinReceiver = receiver.GetComponentInParent<ICoinReceiver>();
        if (coinReceiver == null) return false;

        coinReceiver.AddCoins(coins);
        return true;
    }
}

