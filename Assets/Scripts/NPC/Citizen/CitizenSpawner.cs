using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NPC.Citizen
{
    public class CitizenSpawner : BaseSpawner
    {
        private int waypointCount = 0;
        private Citizen m_Citizen;
        protected override void Get(NPCManager npc)
        {
            m_Citizen = npc as Citizen;
            int waypointIndex = waypointCount++ % transform.childCount;
            int randomize = Random.value < 0.5f ? 0 : 1;
            Transform selectedWaypoint = transform.GetChild(waypointIndex);
            Transform initTf;
            if (randomize == 0)
            {
                initTf = selectedWaypoint.GetChild(0);
                m_Citizen.TravelBackwards = false;
            }
            else
            {
                initTf = selectedWaypoint.GetChild(selectedWaypoint.childCount - 1);
                m_Citizen.TravelBackwards = true;
            }
            m_Citizen.CurrentWaypoint = initTf.GetComponent<Waypoint>();
            m_Citizen.gameObject.SetActive(true);
            m_Citizen.Agent.Warp(initTf.position);
        }



    }


}

