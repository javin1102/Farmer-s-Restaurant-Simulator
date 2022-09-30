using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class UITab : MonoBehaviour
{
    public string Name { get => m_TabName; }
    public UIMenu Menu { get => m_Menu; }

    [SerializeField] private string m_TabName;
    [SerializeField] private UIMenu m_Menu;


    private void OnEnable()
    {
        if ( UIOtherController.TabIndex == transform.GetSiblingIndex() )
        {
            transform.localPosition = Vector3.zero;
        }

        else if ( UIOtherController.TabIndex > transform.GetSiblingIndex() )
        {
            transform.localPosition = Vector3.left * 1000f;
            transform.DOLocalMoveX( 0, .5f );
        }

        else if( UIOtherController.TabIndex < transform.GetSiblingIndex() )
        {
            transform.localPosition = Vector3.right * 1000f;
            transform.DOLocalMoveX( 0, .5f );
        }

        UIOtherController.TabIndex = transform.GetSiblingIndex();
    }

}
