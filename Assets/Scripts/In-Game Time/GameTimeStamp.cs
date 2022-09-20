using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameTimeStamp
{

    public int day;
    public int hour;
    public int minute;

    public GameTimeStamp(int day,int hour,int minute)
    {
        this.day = day;
        this.hour = hour;
        this.minute = minute;
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
        if(hour >= 24)
        {
            hour = 0;
            day++;
        }

        if(day >= 30)
        {
            day = 1;
        }
    }

    public static int HourToMinutes(int hour)
    {
        return hour * 60;
    }


}

