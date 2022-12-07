using UnityEngine;
[CreateAssetMenu(fileName = "Spawn Data", menuName = "Custom Data/New Spawn Data")]
public class PlayerSpawnPosData : ScriptableObject
{
    public CustomTransform cityDoorSpawnTf;
    public CustomTransform farmCitySpawnTf;
    public CustomTransform cityFarmSpawnTf;
    public CustomTransform houseDoorSpawnTf;
    public CustomTransform houseBedSpawnTf;
}
