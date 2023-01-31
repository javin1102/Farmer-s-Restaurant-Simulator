using UnityEngine;
using UnityEngine.UI;

public class UICrosshairBar : MonoBehaviour
{
    private Image m_CrosshairBar;
    private PlayerAction m_PlayerAction;
    private void Start()
    {
        m_CrosshairBar = GetComponent<Image>();
        m_PlayerAction = transform.root.GetComponent<PlayerAction>();
    }

    private void Update()
    {
        m_CrosshairBar.fillAmount = Mathf.InverseLerp( m_PlayerAction.DefaultActionTime, 0, m_PlayerAction.ActionTime );
    }
}