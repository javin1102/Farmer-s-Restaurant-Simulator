using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Snapshot : MonoBehaviour
{
    // Start is called before the first frame update
    SnapshotCamera _snapshotCamera;
    [SerializeField] private GameObject prefab;
    Camera _cam;


    void Start()
    {
        _cam = Camera.main;
        _snapshotCamera = SnapshotCamera.MakeSnapshotCamera( 16 );
        _snapshotCamera.defaultPositionOffset = Vector3.zero;
        _snapshotCamera.transform.position = _cam.transform.position;
        _snapshotCamera.transform.rotation = _cam.transform.rotation;
        TakeAllFurnituresSnapshot();

    }

    [ContextMenu("Take Snapshot")]
    public void TakeSnapshot()
    {
        Texture2D texture = _snapshotCamera.TakePrefabSnapshot( prefab, Vector3.forward + Vector3.up * -.5f, Quaternion.Euler( 0, 160, 0 ), Vector3.one, 512, 512 );
        SnapshotCamera.SavePNG( texture, prefab.name, "Assets/Snapshots" );
    }
    private void TakeSnapshot(GameObject prefab)
    {
        Texture2D texture = _snapshotCamera.TakePrefabSnapshot( prefab, Vector3.forward + Vector3.up * -.5f, Quaternion.Euler( 0, 160, 0 ), Vector3.one, 2048, 2048 );
        SnapshotCamera.SavePNG( texture, prefab.name, "Assets/Snapshots" );
    }

    private void TakeAllFurnituresSnapshot()
    {
        Resources.LoadAll<FurnitureData>( "Data/Furnitures" ).ToList().ForEach(furnitureData=> TakeSnapshot(furnitureData.prefab));
    }
}
