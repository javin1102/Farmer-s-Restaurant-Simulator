using System;
using UnityEngine;

namespace NPC.Cashier
{
    [RequireComponent(typeof(Hoverable))]

    public abstract class Cashier : NPCManager, IInteractable
    {
        [SerializeField] protected PlayerAction m_PlayerAction;
        [SerializeField] protected Transform m_SpawnedItemTf;
        protected Hoverable m_Hoverable;
        protected UIManager m_UIManager;
        private new void Awake()
        {
            base.Awake();
            m_Hoverable = GetComponent<Hoverable>();
            Animator.SetTrigger(Utils.NPC_STAND_ANIM_PARAM);
            m_UIManager = UIManager.Instance;
        }



        public void Interact(PlayerAction playerAction)
        {
            m_PlayerAction = playerAction;
            m_PlayerAction.PlayAudio("button_sfx");
            OnInteract();
        }

        protected abstract void OnInteract();
    }
}

