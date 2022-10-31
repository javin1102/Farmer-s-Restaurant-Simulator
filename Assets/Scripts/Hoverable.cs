using UnityEngine;
using UnityEngine.Events;

public class Hoverable : MonoBehaviour
{
    private int m_Layer;
    private bool m_IsHoverable;
    [SerializeField] protected UIManager m_UIManager;
    public bool IsHoverable { get => m_IsHoverable; set => m_IsHoverable = value; }
    public event UnityAction OnHoverEnter;
    private void Awake()
    {
        m_Layer = Utils.RaycastableLayer;
        m_IsHoverable = true;
    }
    private void Start()
    {
        m_UIManager = UIManager.Instance;
    }


    public void HoverEnter()
    {
        if ( !m_IsHoverable ) return;
        gameObject.layer = Utils.OutlineLayer;
        transform.SetLayer( Utils.OutlineLayer );
        OnHoverEnter?.Invoke();
    }
    public void HoverExit()
    {
        gameObject.layer = m_Layer;
        transform.SetLayer( m_Layer );
        m_UIManager.HideActionHelper();
    }


}
