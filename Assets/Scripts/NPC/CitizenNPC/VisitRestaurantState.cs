using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class VisitRestaurantState : NPCBaseState
{
    private RestaurantManager m_Restaurant;
    private Citizen m_Citizen;
    private Seat m_Seat;
    public override void OnEnterState( NPCManager NPC )
    {
        m_Restaurant = RestaurantManager.Instance;
        m_Citizen = NPC as Citizen;
        if ( !m_Restaurant.FindUnoccupiedSeat( out m_Seat ) || !FindSeatDest( NPC, m_Seat ) )
        {

            //TODO::Can't find unoccupied table, or all seats has no path -> Exit State
            return;
        }

        //Order food from restaurant
        Debug.Log( "Enter" );
    }

    public override void OnExitState( NPCManager NPC )
    {
        //Pay player based on the food, leave restaurant
        if ( m_Seat != null )
        {
            m_Restaurant.UnoccupiedSeats.Add( m_Seat );
        }


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

    private bool FindSeatDest( NPCManager NPC, Seat seat )
    {
        if ( NPC.Agent.SetDestination( seat.transform.position ) )
        {
            seat.IsOccupied = true;
            m_Restaurant.UnoccupiedSeats.Remove( seat );

            //seat.table.Citizen = m_Citizen;
            return true;
        }

        Debug.Log( "NOPE" );
        //All seat has no path, go to exit state
        return false;
    }
}
