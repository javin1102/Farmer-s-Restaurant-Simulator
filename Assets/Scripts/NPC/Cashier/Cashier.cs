using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NPC.Cashier
{
    [RequireComponent(typeof(Hoverable))]

    public abstract class Cashier : NPCManager, IInteractable
    {
        [SerializeField] protected PlayerAction m_PlayerAction;
        [SerializeField] protected Transform m_SpawnedItemTf;
        protected Hoverable m_Hoverable;
        private void Start()
        {
            m_Hoverable = GetComponent<Hoverable>();
        }
        public void Interact()
        {
            OnInteract();
        }

        protected abstract void OnInteract();
    }
}

