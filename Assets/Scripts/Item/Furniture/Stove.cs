using NPC.Chef;
using UnityEngine;

public class Stove : Furniture, IInteractable
{
    public Transform ChefStandTf { get => m_ChefStandTf; set => m_ChefStandTf = value; }
    private Chef m_Chef;
    [SerializeField] private Transform m_ChefStandTf;

    public override void MainAction()
    {
        if ( !gameObject.activeInHierarchy || !m_IsInstantiable ) return;
        base.MainAction();
    }
    public void Interact()
    {
        if ( m_Chef == null ) SetChef();
        else UnsetChef();
    }

    private void SetChef()
    {
        m_Restaurant = RestaurantManager.Instance;
        if ( !m_Restaurant.FindNoStoveChef( out m_Chef ) ) return;
        m_Chef.Stove = this;
        m_Chef.Agent.isStopped = false;
        m_Chef.ChangeState( new GoToStoveState() );
    }

    private void UnsetChef()
    {
        m_Chef.Agent.isStopped = true;
        m_Chef.Stove = null;
        m_Chef = null;
    }
}
