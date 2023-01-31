using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using UnityEngine;

[System.Serializable]
public class GameTimeStamp : ISerializable
{

    public int day;
    public int hour;
    public int minute;

    public GameTimeStamp(JSONNode jsonNode)
    {
        this.day = jsonNode["day"];
        this.hour = jsonNode["hour"];
        this.minute = jsonNode["minute"];

    }
    public GameTimeStamp(int day, int hour, int minute)
    {
        this.day = day;
        this.hour = hour;
        this.minute = minute;
    }

    public GameTimeStamp(GameTimeStamp timeStamp)
    {
        this.day = timeStamp.day;
        this.hour = timeStamp.hour;
        this.minute = timeStamp.minute;
    }

    public GameTimeStamp()
    {
    }

    public void UpdateClock()
    {
        minute++;

        // 1 hour is 60 minute
        if (minute >= 60)
        {
            minute = 0;
            hour++;
        }

        // 1 day is 24 hour
        if (hour >= 24)
        {
            hour = 0;
            day++;
        }

        if (day > 30)
        {
            day = 1;
        }
    }

    public static int HourToMinutes(int hour)
    {
        return hour * 60;
    }

    public static float MinutesToHour(int minute) => (float)minute / 60;

    public static int DayToHour(int days)
    {
        return days * 24;
    }

    public static float CompareTimeStamps(GameTimeStamp timeStamp1, GameTimeStamp timeStamp2)
    {
        float timeStamp1Hour = DayToHour(timeStamp1.day) + timeStamp1.hour + MinutesToHour(timeStamp1.minute);
        float timeStamp2Hour = DayToHour(timeStamp2.day) + timeStamp2.hour + MinutesToHour(timeStamp2.minute);

        return Mathf.Abs(timeStamp2Hour - timeStamp1Hour);
    }

    public JSONObject Serialize()
    {
        JSONObject jsonObject = new();
        jsonObject.Add("minute", minute);
        jsonObject.Add("hour", hour);
        jsonObject.Add("day", day);
        return jsonObject;
    }
}

