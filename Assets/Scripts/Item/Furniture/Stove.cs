using NPC.Chef;
using UnityEngine;

public class Stove : Furniture, IInteractable
{
    public Transform ChefStandTf { get => m_ChefStandTf; set => m_ChefStandTf = value; }
    public Chef Chef { get => m_Chef; }

    [SerializeField] private Transform m_ChefStandTf;
    [SerializeField] private Chef m_Chef;
    private string m_HelperText = "";

    protected override void OnInstantiate()
    {
        Stove instantiatedStove = m_InstantiatedGO.GetComponent<Stove>();
        m_Restaurant.Stoves.Add(instantiatedStove);
        instantiatedStove.UpdateHelper();
    }

    private new void OnDestroy()
    {
        base.OnDestroy();
        if (m_IsInstantiated)
        {
            m_Restaurant = RestaurantManager.Instance;
            m_Restaurant.Stoves.Remove(this);
        }
    }


    protected override void ShowHelper()
    {
        m_HelperText = m_Chef == null ? "Tugas Chef" : "<color=red>Cabut Chef</color>";
        m_UIManager.ShowActionHelperPrimary("Left", m_HelperText);
        m_UIManager.ShowActionHelperSecondary("F", "Simpan");
    }

    public void Interact(PlayerAction playerAction)
    {
        if (!m_IsInstantiated) return;
        if (m_Chef == null) SetChef();
        else UnsetChef();
        UpdateHelper();
    }

    private void UpdateHelper()
    {
        m_Hoverable.OnHoverEnter -= ShowHelper;
        m_Hoverable.OnHoverEnter += ShowHelper;
    }

    private void SetChef()
    {
        m_Restaurant = RestaurantManager.Instance;
        if (!m_Restaurant.FindNoStoveChef(out m_Chef)) return;
        m_Chef.Stove = this;
        m_Chef.ChangeState(new GoToStoveState());
    }

    public void SetChef_Warp()
    {
        m_Restaurant = RestaurantManager.Instance;
        if (!m_Restaurant.FindNoStoveChef(out m_Chef)) return;
        m_Chef.Stove = this;
        m_Chef.Agent.Warp(m_ChefStandTf.position);
        m_Chef.transform.forward = m_ChefStandTf.transform.forward;
    }

    private void UnsetChef()
    {
        m_Chef.Stove = null;
        m_Chef = null;
    }


}
