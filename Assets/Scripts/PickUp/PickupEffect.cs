using UnityEngine;

public abstract class PickupEffect : ScriptableObject
{
    // receiver è l'oggetto che entra nel trigger (di solito un collider del player)
    public abstract bool Apply(GameObject receiver);
}