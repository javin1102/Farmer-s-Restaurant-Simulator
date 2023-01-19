using UnityEngine;
using UnityEngine.UI;

public class UIHelperPanel : MonoBehaviour
{
    [SerializeField] private Button m_CloseButton;
    void Awake()
    {
        m_CloseButton.onClick.AddListener(() =>
        {
            gameObject.SetActive(false);
            PlayerAction.Instance.PlayAudio(Utils.BUTTON_SFX);
        });
    }
}
