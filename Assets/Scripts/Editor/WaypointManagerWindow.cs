using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.Runtime.CompilerServices;

public class WaypointManagerWindow : EditorWindow
{
    private SerializedObject so;
    public Transform waypointRoot;
    public float width;
    private SerializedProperty propWidth;
    [MenuItem( "Tools/Custom/Waypoint Editor" )]
    public static void Open()
    {
        GetWindow<WaypointManagerWindow>();
    }

    private void OnEnable()
    {
        SceneView.duringSceneGui += SceneGUI;
    }

    private void OnDisable()
    {
        SceneView.duringSceneGui -= SceneGUI;
    }
    private void OnGUI()
    {
        so = new( this );
        so.Update();
        propWidth = so.FindProperty( "width" );
        EditorGUILayout.PropertyField( so.FindProperty( "waypointRoot" ) );
        using ( new EditorGUILayout.HorizontalScope() )
        {
            EditorGUILayout.LabelField( "Width" );
            width = EditorGUILayout.Slider( width, 1, 10 );
        }

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

        so.ApplyModifiedProperties();
    }

    private void SceneGUI( SceneView sceneView )
    {
        //Handles.BeginGUI();
        Event e = Event.current;
        Vector3 pos;
        bool holdingAlt = ( e.modifiers & EventModifiers.Alt ) != 0;
        bool holdingCtrl = ( e.modifiers & EventModifiers.Control ) != 0;
        int controlId = GUIUtility.GetControlID( FocusType.Passive );
        if ( holdingAlt )
            sceneView.sceneViewState.alwaysRefresh = true;
        else
            sceneView.sceneViewState.alwaysRefresh = false;
        switch ( e.type )
        {
            case EventType.Repaint:
                if ( !holdingAlt ) return;
                if ( TryRaycastFromCamera( out pos ) )
                {
                    if ( Selection.activeGameObject == null ) return;
                    if ( !Selection.activeGameObject.TryGetComponent<Waypoint>( out var selectedWaypoint ) ) return;
                    Handles.color = Color.yellow * .5f;
                    Handles.SphereHandleCap( 0, pos, selectedWaypoint.transform.rotation, .2f, EventType.Repaint );
                    Handles.color = Color.white;
                    Handles.DrawLine( pos + ( selectedWaypoint.transform.right * width / 2f ), pos - ( selectedWaypoint.transform.right * width / 2f ) );
                }
                break;
            case EventType.MouseDown:
                if ( !holdingAlt ) return;
                if ( e.button == 0 )
                {
                    if ( TryRaycastFromCamera( out pos ) )
                    {
                        CreateWaypoint( pos );
                    }
                }
                else if ( e.button == 1 )
                {
                    RemoveWaypoint();
                }
                GUIUtility.hotControl = controlId;
                e.Use();
                break;
            case EventType.ScrollWheel:
                if ( holdingAlt )
                {
                    float scrollDir = Mathf.Clamp( -e.delta.y, -1, 1 );
                    so.Update();
                    propWidth.floatValue += scrollDir * .5f;
                    so.ApplyModifiedProperties();
                    Repaint();
                }

                else if ( holdingCtrl )
                {
                    if ( Selection.activeGameObject == null ) return;
                    if ( !Selection.activeGameObject.TryGetComponent<Waypoint>( out var selectedWaypoint ) ) return;
                    float scrollDir = Mathf.Clamp( e.delta.y, -1, 1 );
                    selectedWaypoint.transform.eulerAngles += 45 * scrollDir * Vector3.up;
                }

                e.Use();
                break;
        }
        //Handles.EndGUI();
    }

    private bool TryRaycastFromCamera( out Vector3 pos )
    {
        var ray = HandleUtility.GUIPointToWorldRay( Event.current.mousePosition );
        if ( Physics.Raycast( ray, out RaycastHit hitInfo ) )
        {
            pos = hitInfo.point + Vector3.up * .25f;
            return true;
        }
        pos = Vector3.zero;
        return false;
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
        GameObject waypointObject = new( "Waypoint " + waypointRoot.childCount, typeof( Waypoint ) );
        waypointObject.transform.SetParent( waypointRoot );
        Waypoint waypoint = waypointObject.GetComponent<Waypoint>();
        if ( waypointRoot.childCount > 1 )
        {
            waypoint.previousWaypoint = waypointRoot.GetChild( waypointRoot.childCount - 2 ).GetComponent<Waypoint>();
            waypoint.previousWaypoint.nextWayPoint = waypoint;
            waypoint.transform.position = waypoint.previousWaypoint.transform.position;
            waypoint.transform.forward = waypoint.previousWaypoint.transform.forward;
        }
        waypoint.width = propWidth.floatValue;
        Selection.activeGameObject = waypoint.gameObject;
    }

    void CreateWaypointBefore()
    {
        GameObject waypointObject = new( "Waypoint " + waypointRoot.childCount, typeof( Waypoint ) );
        waypointObject.transform.SetParent( waypointRoot, false );
        Waypoint waypoint = waypointObject.GetComponent<Waypoint>();
        Waypoint newWaypoint = waypointObject.GetComponent<Waypoint>();
        Waypoint selectedWaypoint = Selection.activeTransform.GetComponent<Waypoint>();
        waypointObject.transform.position = selectedWaypoint.transform.position;
        waypointObject.transform.forward = selectedWaypoint.transform.forward;

        if ( selectedWaypoint.previousWaypoint != null )
        {
            newWaypoint.previousWaypoint = selectedWaypoint.previousWaypoint;
            selectedWaypoint.previousWaypoint.nextWayPoint = newWaypoint;
        }
        waypoint.width = propWidth.floatValue;
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
        Waypoint waypoint = waypointObject.GetComponent<Waypoint>();
        Waypoint selectedWaypoint = Selection.activeTransform.GetComponent<Waypoint>();
        waypointObject.transform.position = selectedWaypoint.transform.position;
        waypointObject.transform.forward = selectedWaypoint.transform.forward;

        newWaypoint.previousWaypoint = selectedWaypoint;

        if ( selectedWaypoint.nextWayPoint != null )
        {
            selectedWaypoint.nextWayPoint.previousWaypoint = newWaypoint;
            newWaypoint.nextWayPoint = selectedWaypoint.nextWayPoint;
        }
        waypoint.width = propWidth.floatValue;
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
        if ( selectedWaypoint.previousWaypoint != null )
        {
            selectedWaypoint.previousWaypoint.nextWayPoint = selectedWaypoint.nextWayPoint;
            Selection.activeGameObject = selectedWaypoint.previousWaypoint.gameObject;
        }

        DestroyImmediate( selectedWaypoint.gameObject );

    }

    void CreateWaypoint( Vector3 position )
    {
        GameObject waypointObject = new( "Waypoint " + waypointRoot.childCount, typeof( Waypoint ) );
        waypointObject.transform.SetParent( waypointRoot );
        Waypoint waypoint = waypointObject.GetComponent<Waypoint>();
        if ( waypointRoot.childCount > 1 )
        {
            waypoint.previousWaypoint = waypointRoot.GetChild( waypointRoot.childCount - 2 ).GetComponent<Waypoint>();
            waypoint.previousWaypoint.nextWayPoint = waypoint;
            waypoint.transform.position = waypoint.previousWaypoint.transform.position;
            waypoint.transform.forward = waypoint.previousWaypoint.transform.forward;
        }
        waypoint.width = propWidth.floatValue;
        waypointObject.transform.position = position;
        Selection.activeGameObject = waypoint.gameObject;
    }
}
