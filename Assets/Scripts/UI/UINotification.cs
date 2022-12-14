using UnityEngine;
using TMPro;
public class UINotification : MonoBehaviour
{
    private UIManager m_UIManager;
    private float[] m_Timers = new float[3];
    private const float m_DefaultNotificationTimer = 5f;
    int currentIndex;
    void Start()
    {
        m_UIManager = UIManager.Instance;
    }

    void Update()
    {
        for (int i = 0; i < 3; i++)
        {
            if (m_Timers[i] <= 0)
            {
                transform.GetChild(i).gameObject.SetActive(false);
                m_Timers[i] = m_DefaultNotificationTimer;
            }

            m_Timers[i] -= Time.deltaTime;
        }

        if (!m_UIManager.NotificationQueue.TryPeek(out string res)) return;

        Transform child = transform.GetChild(currentIndex);
        child.gameObject.SetActive(true);
        child.SetSiblingIndex(transform.childCount - 1);
        TMP_Text notif = child.GetChild(0).GetComponent<TMP_Text>();
        notif.text = res;
        currentIndex = ++currentIndex % (transform.childCount - 1);
        m_UIManager.NotificationQueue.Dequeue();
        m_Timers[currentIndex] = m_DefaultNotificationTimer;


    }

}
