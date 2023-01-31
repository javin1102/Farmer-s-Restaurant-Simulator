using UnityEngine;

public class TileManager : MonoBehaviour
{
    public Grid Grid
    {
        get { return m_Grid; }
    }

    public Transform TileParent { get => m_TileParent; }

    public static TileManager instance;

    private Grid m_Grid;
    [SerializeField] private Transform m_TileParent;
    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
        m_Grid = GetComponent<Grid>();
    }


    public Vector3 WorldToTilePos(Vector3 worldPoint)
    {
        Vector3Int cellPos = Grid.WorldToCell(worldPoint);
        Vector3 tilePos = Grid.GetCellCenterWorld(cellPos);
        Vector3 finalPos = new(tilePos.x, worldPoint.y, tilePos.z);
        return finalPos;
    }


}
