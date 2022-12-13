using UnityEngine;
using TMPro;
public class UINotification : MonoBehaviour
{
    private UIManager m_UIManager;
    private TMP_Text m_NotificationText;
    private float m_NotificationTimer;
    private const float m_DefaultNotificationTimer = 3f;
    private CanvasGroup m_CanvasGroup;
    void Start()
    {
        m_CanvasGroup = GetComponent<CanvasGroup>();
        m_NotificationText = transform.GetChild(0).GetComponent<TMP_Text>();
        m_UIManager = UIManager.Instance;
    }

    void Update()
    {
        if (m_NotificationTimer <= 0)
        {
            UpdateNotification();
        }

        m_NotificationTimer -= Time.deltaTime;
    }

    private void UpdateNotification()
    {

        if (m_UIManager.NotificationQueue.TryPeek(out string res))
        {

            m_CanvasGroup.alpha = 1;
            transform.SetSiblingIndex(transform.childCount - 1);
            m_NotificationText.text = res;
            m_UIManager.NotificationQueue.Dequeue();
            m_NotificationTimer = m_DefaultNotificationTimer;
            return;
        }

        m_CanvasGroup.alpha = 0;



    }
}
