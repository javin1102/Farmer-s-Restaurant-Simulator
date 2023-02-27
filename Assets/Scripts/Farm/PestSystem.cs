using UnityEngine;
using System.Collections;

public class PestSystem : MonoBehaviour, ITimeTracker
{
    public Transform tileParent;
    public GameObject pestDayUICanvas;

    private TimeManager m_timeManager;
    private bool IsAlreadyRandom;
    private bool IsAlreadyTriggerDay1, IsAlreadyTriggerDay2;


    private void Start()
    {
        m_timeManager = TimeManager.Instance;
        m_timeManager.RegisterListener(this);

        Debug.Log("Pest Day : " + PlayerPrefs.GetInt("day1") + " , " + PlayerPrefs.GetInt("day2"));
    }

    public void ClockUpdate(GameTimeStamp timeStamp)
    {
        //Debug.Log("PestSystem Time : " + timeStamp.day + " day , " + timeStamp.hour + " hour " + timeStamp.minute + " minute , ");

        // every new month generate 2 days to be pest day
        if (timeStamp.day == 1 && !IsAlreadyRandom)
        {
            PlayerPrefs.SetInt("day1", Random.Range(2, 15));
            PlayerPrefs.SetInt("day2", Random.Range(16, 30));

            Debug.Log("Pest Day1 : " + PlayerPrefs.GetInt("day1") + " , Pest Day 2 : " + PlayerPrefs.GetInt("day2"));
            IsAlreadyRandom = true;
            IsAlreadyTriggerDay1 = false;
            IsAlreadyTriggerDay2 = false;
        }
        else if (timeStamp.day == PlayerPrefs.GetInt("day1") && IsAlreadyTriggerDay1 == false) PestDay(true);
        else if (timeStamp.day == PlayerPrefs.GetInt("day2") && IsAlreadyTriggerDay2 == false) PestDay(false);

        /*  else if( (timeStamp.day == PlayerPrefs.GetInt("day1") || timeStamp.day == PlayerPrefs.GetInt("day2")) && IsAlreadyTrigger == false )
          {
              // Pest Day
              foreach(Transform childTf in tileParent)
              {
                  childTf.GetComponent<PlantTile>().TimeManager.UnRegisterListener(childTf.GetComponent<ITimeTracker>());
                  Destroy(childTf.gameObject);
              }
              IsAlreadyRandom = false;
              StartCoroutine("SetPestDayUI");
          }*/
    }

    private void PestDay(bool IsDay1)
    {
        if (PesticideSystem.Instance.isAlreadyPesticide != true)
        {
            // IsDay1 : True = Day1 , False = Day2
            foreach (Transform childTf in tileParent)
            {
                childTf.GetComponent<PlantTile>().TimeManager.UnRegisterListener(childTf.GetComponent<ITimeTracker>());
                Destroy(childTf.gameObject);

            }

            StartCoroutine(SetPestDayUI());

            if (IsDay1) IsAlreadyTriggerDay1 = true;
            else IsAlreadyTriggerDay2 = true;

            IsAlreadyRandom = false;
        }
        else
        {
            if (IsDay1) IsAlreadyTriggerDay1 = true;
            else IsAlreadyTriggerDay2 = true;

            Debug.Log("Survive from PestDay :)");
        }
    }

    IEnumerator SetPestDayUI()
    {
        pestDayUICanvas.SetActive(true);
        yield return new WaitForSeconds(5f);
        pestDayUICanvas.SetActive(false);
    }
}