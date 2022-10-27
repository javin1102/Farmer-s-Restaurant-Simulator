using System;
using UnityEngine;
[Serializable]
public struct CustomTransform
{
    public Vector3 position;
    public Vector3 eulerAngles;
    public Vector3 scale;
}

[CreateAssetMenu(fileName = "Spawn Data", menuName = "Custom Data/New Spawn Data")]
public class PlayerSpawnPosData : ScriptableObject
{
    public CustomTransform cityDoorSpawnTf;
    public CustomTransform cityFarmSpawnTf;
    public CustomTransform farmSpawnTf;
    public CustomTransform houseDoorSpawnTf;
    public CustomTransform houseBedSpawnTf;
}
