using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;
public interface ISerializable
{
    JSONObject Serialize();
}
