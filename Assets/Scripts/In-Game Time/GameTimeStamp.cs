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

    public GameTimeStamp(GameTimeStamp timeStamp)
    {
        this.day = timeStamp.day;
        this.hour = timeStamp.hour;
        this.minute = timeStamp.minute;
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

    public static int DayToHour(int days)
    {
        return days * 24;
    }

    public static int CompareTimeStamps(GameTimeStamp timeStamp1 , GameTimeStamp timeStamp2)
    {
        int timeStamp1Hour = DayToHour(timeStamp1.day) + timeStamp1.hour;
        int timeStamp2Hour = DayToHour(timeStamp2.day) + timeStamp2.hour;

        return Mathf.Abs(timeStamp2Hour - timeStamp1Hour);
    }


}

