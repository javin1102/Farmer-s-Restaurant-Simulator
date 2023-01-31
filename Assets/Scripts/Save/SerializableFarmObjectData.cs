using System;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using UnityEngine;

[Serializable]
public class SerializableFarmObjectData : ISerializable
{
    public int resourcesIndex;
    public int tileIndex;
    public CustomTransform transform;

    public SerializableFarmObjectData(BaseFarmObject farmObject)
    {
        this.resourcesIndex = farmObject.ResourcesIndex;
        this.tileIndex = farmObject.TileIndex;
        this.transform = new(farmObject.transform);
    }
    public SerializableFarmObjectData(int resourcesIndex, int tileIndex, Transform transform)
    {
        this.resourcesIndex = resourcesIndex;
        this.tileIndex = tileIndex;
        this.transform = new(transform);
    }
    public SerializableFarmObjectData(int resourcesIndex, int tileIndex, Vector3 worldPos, Vector3 eulerAngles, Vector3 localScale)
    {
        this.resourcesIndex = resourcesIndex;
        this.tileIndex = tileIndex;
        this.transform = new(worldPos, eulerAngles, localScale);
    }

    public SerializableFarmObjectData(JSONNode jsonNode)
    {
        this.transform.position = jsonNode["transform"].ToWorldPos();
        this.transform.eulerAngles = jsonNode["transform"].ToEulerAngle();
        this.transform.localScale = jsonNode["transform"].ToLocalScale();
        this.resourcesIndex = jsonNode["resourcesIndex"];
        this.tileIndex = jsonNode["tileIndex"];
    }

    public virtual JSONObject Serialize()
    {
        JSONObject jsonObject = new();
        jsonObject.Add("resourcesIndex", resourcesIndex);
        jsonObject.Add("tileIndex", tileIndex);
        jsonObject.Add("transform", transform.Serialize());
        return jsonObject;
    }
}
