using UnityEngine;

public class MaterialChanger : MonoBehaviour
{
    public Material PreviewMaterial { get => m_PreviewMaterial; }
    public Material FinalMaterial { get => m_FinalMaterial; }
    [SerializeField] private Material m_PreviewMaterial, m_FinalMaterial;
    [Tooltip( "Set Preview Material Color" )]
    [SerializeField] private Color uninstatiableColor, instatiableColor;
    private MeshRenderer m_MeshRenderer;
    void OnEnable()
    {
        m_MeshRenderer = GetComponent<MeshRenderer>();
    }

    public void ChangeToFinalMaterial()
    {
        m_MeshRenderer.material = FinalMaterial;
    }

    public void ChangePreviewMaterialColor( bool isInstatiable )
    {
        if ( isInstatiable ) PreviewMaterial.color = instatiableColor;
        else PreviewMaterial.color = uninstatiableColor;
    }

}
