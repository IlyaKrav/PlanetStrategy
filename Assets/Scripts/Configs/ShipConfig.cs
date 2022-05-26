using UnityEngine;

[CreateAssetMenu(fileName = "Config", menuName = "Configs/ShipConfig")]
public class ShipConfig : ScriptableObject
{
    public int shipHeight;
    public float spawnShipShift;
    public float shipFlyPeriod;
}
