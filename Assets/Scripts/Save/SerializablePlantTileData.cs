using System;
using SimpleJSON;
using UnityEngine;

[Serializable]
public class SerializablePlantTileData : SerializableFarmObjectData
{
    public int plantStatus;
    public string seedID;
    public bool isWet;
    public GameTimeStamp time;


    public SerializablePlantTileData(PlantTile plantTile) : base(plantTile)
    {
        this.plantStatus = (int)plantTile.Status;
        string seedID = plantTile.PlantGrowHandler == null ? "" : plantTile.PlantGrowHandler.SeedData.ID;
        this.seedID = seedID;
        this.isWet = plantTile.CompareTag(Utils.TILE_WET_TAG);
        this.time = new(plantTile.Time);
    }

    public SerializablePlantTileData(JSONNode jsonNode) : base(jsonNode)
    {
        this.plantStatus = jsonNode["plantStatus"];
        this.seedID = jsonNode["seedID"];
        this.isWet = jsonNode["isWet"];
        this.time = new(jsonNode["time"]);
    }

    public override JSONObject Serialize()
    {
        JSONObject jsonObject = base.Serialize();
        jsonObject.Add("plantStatus", plantStatus);
        jsonObject.Add("seedID", seedID);
        jsonObject.Add("isWet", isWet);
        jsonObject.Add("time", time.Serialize());
        return jsonObject;
    }
}
