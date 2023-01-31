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
        if (UIMiscController.TabIndex == transform.GetSiblingIndex())
        {
            transform.localPosition = Vector3.zero;
        }

        else if (UIMiscController.TabIndex > transform.GetSiblingIndex())
        {
            transform.localPosition = Vector3.left * 1000f;
            transform.DOLocalMoveX(0, .5f);
        }

        else if (UIMiscController.TabIndex < transform.GetSiblingIndex())
        {
            transform.localPosition = Vector3.right * 1000f;
            transform.DOLocalMoveX(0, .5f);
        }

        UIMiscController.TabIndex = transform.GetSiblingIndex();
    }

}
