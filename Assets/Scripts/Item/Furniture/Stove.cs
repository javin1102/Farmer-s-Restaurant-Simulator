using NPC.Chef;
using UnityEngine;

public class Stove : Furniture, IInteractable
{
    public Transform ChefStandTf { get => m_ChefStandTf; set => m_ChefStandTf = value; }
    [SerializeField] private Transform m_ChefStandTf;
    private Chef m_Chef;
    private void Start()
    {
        m_Hoverable.OnHoverEnter += ShowHelper;
    }

    private void OnDestroy()
    {
        m_Hoverable.OnHoverEnter -= ShowHelper;
    }

    private void ShowHelper()
    {
        m_UIManager.ShowActionHelperPrimary( "Left", "Pasang Chef" );
        m_UIManager.ShowActionHelperSecondary( "Left", "Simpan" );
    }

    public override void MainAction()
    {
        if ( !gameObject.activeInHierarchy || !m_IsInstantiable ) return;
        base.MainAction();
    }
    public void Interact( PlayerAction playerAction )
    {
        if ( m_Chef == null ) SetChef();
        else UnsetChef();
    }

    private void SetChef()
    {
        m_Restaurant = RestaurantManager.Instance;
        if ( !m_Restaurant.FindNoStoveChef( out m_Chef ) ) return;
        m_Chef.Stove = this;
        m_Chef.ChangeState( new GoToStoveState() );
    }

    private void UnsetChef()
    {
        m_Chef.Stove = null;
        m_Chef = null;
    }
}
