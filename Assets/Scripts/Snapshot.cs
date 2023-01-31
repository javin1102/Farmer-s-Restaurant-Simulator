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
    public Vector3 offset;
    public Vector3 angles;
    public Vector3 scale;


    void Start()
    {
        _cam = Camera.main;
        _snapshotCamera = SnapshotCamera.MakeSnapshotCamera(16);
        _snapshotCamera.defaultPositionOffset = Vector3.zero;
        _snapshotCamera.transform.position = _cam.transform.position;
        _snapshotCamera.transform.rotation = _cam.transform.rotation;
        // scale = prefab.transform.localScale;
        //TakeAllFurnituresSnapshot();
        TakeSnapshot();

    }

    [ContextMenu("Take Snapshot")]
    public void TakeSnapshot()
    {
        Texture2D texture = _snapshotCamera.TakePrefabSnapshot(prefab, offset, Quaternion.Euler(angles), scale, 2048, 2048);
        SnapshotCamera.SavePNG(texture, prefab.name, "Assets/Snapshots");
    }
    private void TakeSnapshot(GameObject prefab)
    {
        Texture2D texture = _snapshotCamera.TakePrefabSnapshot(prefab, Vector3.forward + Vector3.up * -.5f, Quaternion.Euler(0, 75, 0), prefab.transform.localScale, 2048, 2048);
        SnapshotCamera.SavePNG(texture, prefab.name, "Assets/Snapshots");
    }

    private void TakeAllFurnituresSnapshot()
    {
        Resources.LoadAll<FurnitureData>("Data/Furnitures").ToList().ForEach(furnitureData => TakeSnapshot(furnitureData.prefab));
    }
}
