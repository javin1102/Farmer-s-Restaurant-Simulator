using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seat : Furniture
{
    public bool HasTable { get => m_Table != null; }
    private Table m_Table;

    public override void MainAction()
    {
        base.MainAction();
        CheckForTable();
        print( HasTable );
    }

    private void CheckForTable()
    {
        Ray ray = new( m_TempGO.transform.position, m_TempGO.transform.forward );
        if ( Physics.Raycast( ray, out RaycastHit hitInfo, 2 ) )
        {
            if ( hitInfo.collider != null && hitInfo.collider.CompareTag( Utils.TABLE_TAG ) ) m_Table = hitInfo.collider.GetComponent<Table>();
            else m_Table = null;
        }
    }
}
