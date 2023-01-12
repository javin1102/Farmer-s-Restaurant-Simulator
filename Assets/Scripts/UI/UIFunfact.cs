using UnityEngine;
using TMPro;

namespace NPC.Funfact
{
    public class UIFunfact : MonoBehaviour
    {
        private FunfactSO m_Funfact;
        private ResourcesLoader m_ResourcesLoader;
        [SerializeField] private TMP_Text m_FunfactText;
        private NPCFunfact m_NPCFunfact;
        void Start()
        {
            m_ResourcesLoader = ResourcesLoader.Instance;
            m_NPCFunfact = transform.root.GetComponent<NPCFunfact>();
            m_NPCFunfact.UpdateFunfact += UpdateFunfact;
        }

        void OnDestroy()
        {
            m_NPCFunfact.UpdateFunfact -= UpdateFunfact;
        }

        private void UpdateFunfact()
        {
            m_Funfact = m_ResourcesLoader.GetRandomFunfact();
            m_FunfactText.text = m_Funfact.funfactDescription;
        }

    }

}
