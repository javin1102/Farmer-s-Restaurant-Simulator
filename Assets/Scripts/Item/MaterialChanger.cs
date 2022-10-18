using UnityEngine;

public class MaterialChanger : MonoBehaviour
{
    public Material PreviewMaterial { get => m_PreviewMaterial; }
    public Material FinalMaterial { get => m_FinalMaterial; }
    [SerializeField] private Material m_PreviewMaterial, m_FinalMaterial;
    [Tooltip( "Set Preview Material Color" )]
    //[SerializeField] private Color m_UninstatiableColor = new( 0.9f, .1f, 0.03f, .5f ), m_InstatiableColor = new( .5f, 1, .5f, .5f );
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
        //m_MeshRenderer.material = PreviewMaterial;
        //if ( isInstatiable ) PreviewMaterial.color = m_InstatiableColor;
        //else PreviewMaterial.color = m_UninstatiableColor;
    }

}
