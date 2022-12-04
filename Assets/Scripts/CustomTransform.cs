using System;
using SimpleJSON;
using UnityEngine;
[Serializable]
public struct CustomTransform : ISerializable
{
    public Vector3 position;
    public Vector3 eulerAngles;
    public Vector3 localScale;

    public CustomTransform(JSONNode jsonNode)
    {
        position = jsonNode.ToWorldPos();
        eulerAngles = jsonNode.ToEulerAngle();
        localScale = jsonNode.ToLocalScale();
    }
    public CustomTransform(Transform transform)
    {
        this.position = transform.position;
        this.eulerAngles = transform.eulerAngles;
        this.localScale = transform.localScale;
    }

    public CustomTransform(Vector3 position, Vector3 eulerAngles, Vector3 localScale)
    {
        this.position = position;
        this.eulerAngles = eulerAngles;
        this.localScale = localScale;
    }


    public JSONObject Serialize()
    {
        JSONObject jsonObject = new();
        jsonObject.Add("position", position.ToJsonObject());
        jsonObject.Add("eulerAngles", eulerAngles.ToJsonObject());
        jsonObject.Add("localScale", localScale.ToJsonObject());
        return jsonObject;
    }

    public void Set(Vector3 position, Vector3 eulerAngles, Vector3 localScale)
    {
        this.position = position;
        this.eulerAngles = eulerAngles;
        this.localScale = localScale;
    }

    public void Set(Transform transform)
    {
        Set(transform.position, transform.eulerAngles, transform.localScale);
    }
}
