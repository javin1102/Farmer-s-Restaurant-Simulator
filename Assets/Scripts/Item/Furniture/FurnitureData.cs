using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FurnitureType
{
    TABLE, CHAIR, STOVE
}
[CreateAssetMenu( fileName = "Furniture", menuName = "Custom Data/New Furniture" )]
public class FurnitureData : ItemData
{
    public FurnitureType type;
}
