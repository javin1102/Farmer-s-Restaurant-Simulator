using UnityEngine;
using UnityEngine.UI;

public class UIChef : MonoBehaviour
{
    [SerializeField] private Image m_FillImage;
    [SerializeField] private Image m_IconImage;

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag(Utils.PLAYER_TAG))
        {
            transform.LookAt(other.transform);
        }
    }
    public void SetIcon(Sprite icon)
    {
        m_IconImage.sprite = icon;
    }

    public void SetFillAmount(float currDur, float maxDur)
    {
        m_FillImage.fillAmount = Mathf.InverseLerp(0, maxDur, currDur);
    }

    public void DisableTimerUI()
    {
        transform.GetChild(0).gameObject.SetActive(false);
    }
    public void EnableTimerUI()
    {
        transform.GetChild(0).gameObject.SetActive(true);
    }
}
