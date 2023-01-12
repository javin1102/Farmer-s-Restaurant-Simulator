using TMPro;
using UnityEngine;

public class UICoinUpdater : MonoBehaviour
{
    [SerializeField] private TMP_Text m_CoinsText;
    private PlayerAction m_PlayerAction;
    void Start()
    {
        m_PlayerAction = PlayerAction.Instance;
    }
    private void Update()
    {
        m_CoinsText.text = $"Koin Anda :<indent=50%><sprite=0><color=yellow>{m_PlayerAction.Coins}</color>";
    }
}

