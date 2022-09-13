using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NPC.Citizen
{
    public class CitizenSpawner : BaseSpawner
    {
        private int waypointCount = 0;
        protected override void Get( NPCManager npc )
        {
            Citizen citizen = npc as Citizen;
            citizen.gameObject.SetActive( true );
            int waypointIndex = waypointCount++ % transform.childCount;
            int randomize = Random.value < 0.5f ? 0 : 1;
            Transform selectedWaypoint = transform.GetChild(waypointIndex);
            Transform initTf;
            if ( randomize == 0 )
            {
                initTf = selectedWaypoint.GetChild( 0 );
                citizen.TravelBackwards = false;
            }
            else
            {
                initTf = selectedWaypoint.GetChild( selectedWaypoint.childCount - 1 );
                citizen.TravelBackwards = true;
            }
            citizen.CurrentWaypoint = initTf.GetComponent<Waypoint>();
            citizen.Agent.Warp( initTf.position );
        }

    }
}

