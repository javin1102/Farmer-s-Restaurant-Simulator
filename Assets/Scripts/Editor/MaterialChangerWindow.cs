using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Drawing.Printing;
using static UnityEditor.Progress;

public class MaterialChangerWindow : EditorWindow
{
    [MenuItem( "Tools/Custom/Material Changer" )]
    public static void Open() => GetWindow<MaterialChangerWindow>();

    private void OnGUI()
    {
        SerializedObject so = new( this );
        Shader toonShader = Shader.Find( "Shader Graphs/ToonShading" );
        Shader urpLitShader = Shader.Find( "Universal Render Pipeline/Lit" );
        if ( GUILayout.Button( "Change From URP/Lit To Toon Shading" ) )
        {
            foreach ( var item in Selection.assetGUIDs )
            {
                string path = AssetDatabase.GUIDToAssetPath( item );
                Material mat = AssetDatabase.LoadAssetAtPath<Material>( path );
                if ( mat == null || mat.shader != urpLitShader ) continue;
                GetLitMaterialAssets( mat, out Color diffuseColor, out Texture diffuseTexture, out Texture normalMap, out Texture occlusionMap, out Texture emissionMap );
                mat.shader = toonShader;
                LitToToonShading( mat, diffuseColor, diffuseTexture, normalMap, occlusionMap, emissionMap );
            }
        }

        if ( GUILayout.Button( "Change From Toon Shading To URP/Lit" ) )
        {
            foreach ( var item in Selection.assetGUIDs )
            {
                string path = AssetDatabase.GUIDToAssetPath( item );
                Material mat = AssetDatabase.LoadAssetAtPath<Material>( path );
                if ( mat == null || mat.shader != toonShader ) continue;
                GetToonShaderMaterialAssets( mat, out Color diffuseColor, out Texture diffuseTexture, out Texture normalMap, out Texture occlusionMap, out Texture emissionMap );
                mat.shader = urpLitShader;
                ToonShadingToLit( mat, diffuseColor, diffuseTexture, normalMap, occlusionMap, emissionMap );

            }

        }


    }

    private void GetLitMaterialAssets( Material mat, out Color diffuseColor, out Texture diffuseTexture, out Texture normalMap, out Texture occlusionMap, out Texture emissionMap )
    {
        diffuseColor = mat.GetColor( "_Color" );
        diffuseTexture = mat.GetTexture( "_BaseMap" );
        normalMap = mat.GetTexture( "_BumpMap" );
        occlusionMap = mat.GetTexture( "_OcclusionMap" );
        emissionMap = mat.GetTexture( "_EmissionMap" );
    }

    private void ToonShadingToLit( Material mat, Color diffuseColor, Texture diffuseTexture, Texture normalMap, Texture occlusionMap, Texture emissionMap )
    {
        mat.SetTexture( "_BaseMap", diffuseTexture );
        mat.SetTexture( "_BumpMap", normalMap );
        mat.SetTexture( "_OcclusionMap", occlusionMap );
        mat.SetTexture( "_EmissionMap", emissionMap );
        mat.SetColor( "_Color", diffuseColor );
    }

    private void GetToonShaderMaterialAssets( Material mat, out Color diffuseColor, out Texture diffuseTexture, out Texture normalMap, out Texture occlusionMap, out Texture emissionMap )
    {
        diffuseColor = mat.GetColor( "_Diffuse_Color" );
        diffuseTexture = mat.GetTexture( "_Diffuse_Texture" );
        normalMap = mat.GetTexture( "_Normal_Map" );
        occlusionMap = mat.GetTexture( "_Occlusion_Texture" );
        emissionMap = mat.GetTexture( "_Emission_Map" );
    }

    private void LitToToonShading( Material mat, Color diffuseColor, Texture diffuseTexture, Texture normalMap, Texture occlusionMap, Texture emissionMap )
    {
        mat.SetTexture( "_Diffuse_Texture", diffuseTexture );
        mat.SetColor( "_Diffuse_Color", diffuseColor );
        mat.SetTexture( "_Occlusion_Texture", occlusionMap );
        mat.SetTexture( "_Emission_Map", emissionMap );
        mat.SetTexture( "_Normal_Map", normalMap );
    }


}
