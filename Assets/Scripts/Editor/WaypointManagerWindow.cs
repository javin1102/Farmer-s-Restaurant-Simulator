using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

public class WaypointManagerWindow : EditorWindow
{
    [MenuItem( "Tools/Waypoint Editor" )]
    public static void Open()
    {
        GetWindow<WaypointManagerWindow>();
    }

    public Transform waypointRoot;

    private void OnGUI()
    {
        SerializedObject obj = new( this );
        EditorGUILayout.PropertyField( obj.FindProperty( "waypointRoot" ) );
        if ( waypointRoot == null )
        {
            EditorGUILayout.HelpBox( "Root transform must be selected. Please assign a root transform.", MessageType.Warning );
        }

        else
        {
            using ( new EditorGUILayout.VerticalScope() )
            {
                DrawButtons();
            }
        }
        obj.ApplyModifiedProperties();
    }

    private void DrawButtons()
    {
        if ( GUILayout.Button( "Create Waypoint" ) )
        {
            CreateWaypoint();
        }

        if ( Selection.activeGameObject != null && Selection.activeGameObject.GetComponent<Waypoint>() )
        {
            if ( GUILayout.Button( "Create Waypoint Before" ) )
            {
                CreateWaypointBefore();
            }
            if ( GUILayout.Button( "Create Waypoint After" ) )
            {
                CreateWaypointAfter();

            }
            if ( GUILayout.Button( "Remove Waypoint" ) )
            {
                RemoveWaypoint();
            }
        }
    }

    private void CreateWaypoint()
    {
        GameObject waypointObject = new GameObject( "Waypoint " + waypointRoot.childCount, typeof( Waypoint ) );
        waypointObject.transform.SetParent( waypointRoot );
        Waypoint waypoint = waypointObject.GetComponent<Waypoint>();
        if ( waypointRoot.childCount > 1 )
        {
            waypoint.previousWaypoint = waypointRoot.GetChild( waypointRoot.childCount - 2 ).GetComponent<Waypoint>();
            waypoint.previousWaypoint.nextWayPoint = waypoint;
            waypoint.transform.position = waypoint.previousWaypoint.transform.position;
            waypoint.transform.forward = waypoint.previousWaypoint.transform.forward;
        }
        Selection.activeGameObject = waypoint.gameObject;
    }

    void CreateWaypointBefore()
    {
        GameObject waypointObject = new GameObject( "Waypoint " + waypointRoot.childCount, typeof( Waypoint ) );
        waypointObject.transform.SetParent( waypointRoot, false );
        Waypoint newWaypoint = waypointObject.GetComponent<Waypoint>();
        Waypoint selectedWaypoint = Selection.activeTransform.GetComponent<Waypoint>();
        waypointObject.transform.position = selectedWaypoint.transform.position;
        waypointObject.transform.forward = selectedWaypoint.transform.forward;

        if ( selectedWaypoint.previousWaypoint != null )
        {
            newWaypoint.previousWaypoint = selectedWaypoint.previousWaypoint;
            selectedWaypoint.previousWaypoint.nextWayPoint = newWaypoint;
        }

        newWaypoint.nextWayPoint = selectedWaypoint;
        selectedWaypoint.previousWaypoint = newWaypoint;
        newWaypoint.transform.SetSiblingIndex( selectedWaypoint.transform.GetSiblingIndex() );
        Selection.activeGameObject = newWaypoint.gameObject;

    }

    void CreateWaypointAfter()
    {
        GameObject waypointObject = new GameObject( "Waypoint " + waypointRoot.childCount, typeof( Waypoint ) );
        waypointObject.transform.SetParent( waypointRoot, false );
        Waypoint newWaypoint = waypointObject.GetComponent<Waypoint>();
        Waypoint selectedWaypoint = Selection.activeTransform.GetComponent<Waypoint>();
        waypointObject.transform.position = selectedWaypoint.transform.position;
        waypointObject.transform.forward = selectedWaypoint.transform.forward;

        newWaypoint.previousWaypoint = selectedWaypoint;

        if ( selectedWaypoint.nextWayPoint != null )
        {
            selectedWaypoint.nextWayPoint.previousWaypoint = newWaypoint;
            newWaypoint.nextWayPoint = selectedWaypoint.nextWayPoint;
        }

        selectedWaypoint.nextWayPoint = newWaypoint;

        newWaypoint.transform.SetSiblingIndex( selectedWaypoint.transform.GetSiblingIndex() );

        Selection.activeGameObject = newWaypoint.gameObject;


    }

    void RemoveWaypoint()
    {
        Waypoint selectedWaypoint = Selection.activeGameObject.GetComponent<Waypoint>();
        if ( selectedWaypoint.nextWayPoint != null )
        {
            selectedWaypoint.nextWayPoint.previousWaypoint = selectedWaypoint.previousWaypoint;
        }
        if(selectedWaypoint.previousWaypoint != null )
        {
            selectedWaypoint.previousWaypoint.nextWayPoint = selectedWaypoint.nextWayPoint;
            Selection.activeGameObject = selectedWaypoint.previousWaypoint.gameObject;
        }

        DestroyImmediate( selectedWaypoint.gameObject );
    }
}
