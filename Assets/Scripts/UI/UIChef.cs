using System.Collections;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class UIChef : MonoBehaviour
{
    private CanvasGroup m_CanvasGroup;
    [SerializeField] private Image m_FillImage;
    [SerializeField] private Image m_IconImage;
    private void Start()
    {
        m_CanvasGroup = GetComponent<CanvasGroup>();
        m_CanvasGroup.alpha = 0;
    }
    private void OnTriggerEnter( Collider other )
    {
        if ( other.CompareTag( Utils.PLAYER_TAG ) )
        {
            DOTween.To( x => m_CanvasGroup.alpha = x, 0, 1, .25f );
        }
    }

    private void OnTriggerExit( Collider other )
    {
        if ( other.CompareTag( Utils.PLAYER_TAG ) )
        {
            DOTween.To( x => m_CanvasGroup.alpha = x, 1, 0, .25f );
        }
    }

    private void OnTriggerStay( Collider other )
    {
        if ( other.CompareTag( Utils.PLAYER_TAG ) )
        {
            transform.LookAt( other.transform );
        }
    }
    public void SetIcon( Sprite icon )
    {
        m_IconImage.sprite = icon;
    }

    public void SetFillAmount( float currDur, float maxDur )
    {
        m_FillImage.fillAmount = Mathf.InverseLerp( 0, maxDur, currDur );
    }
}
