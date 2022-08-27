using System.Collections;
using System.Collections.Generic;
using System.Drawing.Printing;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
public class VisitRestaurantState : NPCBaseState
{
    private RestaurantManager m_Restaurant;
    private Citizen m_Citizen;
    public override void OnEnterState( NPCManager NPC )
    {
        m_Restaurant = RestaurantManager.Instance;
        m_Citizen = NPC as Citizen;
        if ( !m_Restaurant.FindUnoccupiedTable( out Table table ) || !FindSeat( NPC, table ) )
        {

            //TODO::Can't find unoccupied table, or all seats has no path -> Exit State
            return;
        }

        //Order food from restaurant
        Debug.Log( "Enter" );
    }

    public override void OnExitState( NPCManager NPC )
    {
        //Pay player based on the food


    }

    public override void OnUpdateState( NPCManager NPC )
    {
        //Debug.Log("Update");
        //if ( Vector3.Distance( NPC.transform.position, NPC.Agent.destination ) <= .35f )
        //{
        //    //Take a seat
        //}

        //If food has arrived, switch to eat animation
    }

    private bool FindSeat( NPCManager NPC, Table table )
    {
        foreach ( Seat seat in table.Seats )
        {
            if ( NPC.Agent.SetDestination( seat.transform.position ) )
            {
                table.IsOccupied = true;
                table.Citizen = m_Citizen;
                return true;
            }
        }

        Debug.Log( "NOPE" );
        //All seat has no path, go to exit state
        return false;
    }
}
