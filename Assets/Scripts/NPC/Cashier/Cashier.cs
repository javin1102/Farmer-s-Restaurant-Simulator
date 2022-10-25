using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NPC.Cashier
{
    [RequireComponent(typeof(Hoverable))]

    public class Cashier : NPCManager, IInteractable
    {
        [SerializeField] private PlayerAction m_PlayerAction;
        [SerializeField] private Transform m_SpawnedItemTf;
        private Hoverable m_Hoverable;
        private void Start()
        {
            m_Hoverable = GetComponent<Hoverable>();
        }
        public void Interact()
        {
            m_PlayerAction.ToggleFurnitureStoreUI?.Invoke( m_SpawnedItemTf );
        }
    }
}

