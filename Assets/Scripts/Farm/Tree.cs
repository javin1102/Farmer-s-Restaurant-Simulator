using UnityEngine;

[RequireComponent( typeof( Hoverable ) )]
[RequireComponent( typeof( FarmObject ) )]
public class Tree : MonoBehaviour, IActionTime
{
    public float ActionTime { get; set; }
    public float DefaultActionTime => 3.5f;
    private PlayerAction m_PlayerAction;
    private Hoverable m_Hoverable;

    private void Start()
    {
        m_Hoverable = GetComponent<Hoverable>();
        ActionTime = DefaultActionTime;
        m_Hoverable.OnHoverExit += OnHoverExit;
    }

    public void OnHoldMainAction( PlayerAction playerAction )
    {
        m_PlayerAction = playerAction;
        float multiplier = 1;
        if ( playerAction.CurrEquippedItem != null && playerAction.CurrEquippedItem.Data.id.Equals( "Axe" ) ) multiplier = 3;
        ActionTime -= Time.deltaTime * multiplier;
        playerAction.DefaultActionTime = DefaultActionTime;
        playerAction.ActionTime = ActionTime;
    }

    public void OnReleaseMainAction( PlayerAction playerAction )
    {
        ActionTime = DefaultActionTime;
        playerAction.DefaultActionTime = 0;
        playerAction.ActionTime = 0;
    }

    private void OnHoverExit()
    {
        ActionTime = DefaultActionTime;
    }

    private void Update()
    {
        if ( ActionTime <= 0 )
        {
            Destroy( gameObject );
        }
    }

    private void OnDestroy()
    {
        m_Hoverable.OnHoverExit -= OnHoverExit;
        if ( m_PlayerAction == null ) return;
        m_PlayerAction.DefaultActionTime = 0;
        m_PlayerAction.ActionTime = 0;
    }
}