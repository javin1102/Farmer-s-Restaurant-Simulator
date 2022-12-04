using NPC.Chef;
using UnityEngine;

public class Stove : Furniture, IInteractable
{
    public Transform ChefStandTf { get => m_ChefStandTf; set => m_ChefStandTf = value; }
    public Chef Chef { get => m_Chef; }

    [SerializeField] private Transform m_ChefStandTf;
    [SerializeField] private Chef m_Chef;
    private string m_HelperText = "Test";

    protected override void OnInstantiate()
    {
        m_Restaurant.Stoves.Add(m_InstantiatedGO.GetComponent<Stove>());
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
        m_HelperText = m_Chef == null ? "Tugas Chef" : "Cabut Chef";
        m_UIManager.ShowActionHelperPrimary("Left", m_HelperText);
        m_UIManager.ShowActionHelperSecondary("Left", "Simpan");
    }

    public void Interact(PlayerAction playerAction)
    {
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

    private void UnsetChef()
    {
        m_Chef.Stove = null;
        m_Chef = null;
    }


}
