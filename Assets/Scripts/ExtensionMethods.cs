using SimpleJSON;
using UnityEngine;
using UnityEngine.AI;

public static class ExtensionMethods
{
    public static bool HasReachedDestination(this NavMeshAgent agent, float offset = 0) => agent.remainingDistance - offset <= agent.stoppingDistance;
    public static Vector3 Round(this Vector3 vec)
    {
        vec.x = Mathf.Round(vec.x);
        vec.y = Mathf.Round(vec.y);
        vec.z = Mathf.Round(vec.z);
        return vec;
    }

    public static void SetLayer(this Transform trans, int layer)
    {
        if (trans.gameObject.layer == Utils.UILayer) return;
        trans.gameObject.layer = layer;
        foreach (Transform child in trans)
            child.SetLayer(layer);
    }



    #region Tranform Serializer

    public static Vector3 ToWorldPos(this JSONNode jsonNode)
    {
        return new Vector3()
        {
            x = jsonNode["position"]["x"],
            y = jsonNode["position"]["y"],
            z = jsonNode["position"]["z"]
        };
    }

    public static Vector3 ToEulerAngle(this JSONNode jsonNode)
    {
        return new Vector3()
        {
            x = jsonNode["eulerAngles"]["x"],
            y = jsonNode["eulerAngles"]["y"],
            z = jsonNode["eulerAngles"]["z"]
        };
    }

    public static Vector3 ToLocalScale(this JSONNode jsonNode)
    {
        return new Vector3()
        {
            x = jsonNode["localScale"]["x"],
            y = jsonNode["localScale"]["y"],
            z = jsonNode["localScale"]["z"]
        };
    }
    #endregion

    public static JSONObject ToJsonObject(this Vector3 vec)
    {
        JSONObject jsonObject = new();
        jsonObject.Add("x", vec.x);
        jsonObject.Add("y", vec.y);
        jsonObject.Add("z", vec.z);
        return jsonObject;
    }
}

