using UnityEngine;

[CreateAssetMenu(menuName = "Pickups/Add Time Effect")]
public class AddTimeEffect : PickupEffect
{
    [SerializeField] private float seconds = 30f;

    public override bool Apply(GameObject receiver)
    {
        var timeReceiver = receiver.GetComponentInParent<ITimeReceiver>();
        if (timeReceiver == null) return false;

        timeReceiver.AddTime(seconds);
        return true;
    }
}
