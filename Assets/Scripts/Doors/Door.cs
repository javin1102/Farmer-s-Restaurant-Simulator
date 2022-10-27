using UnityEngine;

[RequireComponent( typeof( Hoverable ) )]
public abstract class Door : MonoBehaviour, IInteractable
{
    protected SceneLoader m_SceneLoader;
    protected UIManager m_UIManager;
    protected Hoverable m_Hoverable;
    private void Start()
    {
        m_SceneLoader = SceneLoader.Instance;
        m_UIManager = UIManager.Instance;
        m_Hoverable = GetComponent<Hoverable>();
        m_Hoverable.OnHoverEnter += ShowHelper;
    }

    private void OnDestroy()
    {
        m_Hoverable.OnHoverEnter -= ShowHelper;
    }

    protected abstract void ShowHelper();
    public void Interact( PlayerAction playerAction )
    {
        OnInteract();
    }

    protected abstract void OnInteract();
}

