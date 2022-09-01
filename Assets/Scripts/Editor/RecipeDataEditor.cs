using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

[CustomEditor( typeof( FoodData ) )]
public class RecipeDataEditor : Editor
{
    public int ingredientQuantity;
    private SerializedObject so;
    private SerializedProperty m_Ingredients;
    private List<ItemData> m_IngredientsAssetData;
    private ItemData m_SelectedIngredient;
    private void OnEnable()
    {
        so = serializedObject;
        m_IngredientsAssetData = LoadIngredientsData();
        m_Ingredients = so.FindProperty( "ingredients" );
    }
    public override void OnInspectorGUI()
    {
        so.Update();
        base.OnInspectorGUI();
        GUIStyle helpbox = new( "HelpBox" )
        {
            padding = new RectOffset( 10, 10, 10, 10 ),
        };

        GUIStyle popup = new( EditorStyles.popup )
        {
            fixedHeight = 35,
            padding = new RectOffset( 10, 10, 10, 10 ),
            stretchWidth = true,
            alignment = TextAnchor.MiddleCenter,
            fontStyle = FontStyle.Bold
        };

        GUIStyle button = new( GUI.skin.button )
        {
            padding = new RectOffset( 10, 10, 10, 10 ),
            fixedHeight = 30,
            stretchWidth = true,
            alignment = TextAnchor.MiddleCenter,
            fontStyle = FontStyle.Bold,
        };

        EditorGUILayout.Space( 20 );


        using ( new EditorGUILayout.VerticalScope( helpbox ) )
        {
            EditorGUILayout.LabelField( "Search Ingredients", EditorStyles.boldLabel );
            EditorGUI.indentLevel++;

            using ( new EditorGUILayout.HorizontalScope() )
            {
                m_SelectedIngredient = EditorGUILayout.ObjectField( "Ingredient", m_SelectedIngredient, typeof( ItemData ), true ) as ItemData;
            }

            using ( new EditorGUILayout.HorizontalScope() )
            {
                EditorGUILayout.LabelField( "Quantity " );
                ingredientQuantity = EditorGUILayout.IntField( ingredientQuantity );
            }

            EditorGUILayout.Space( 20 );

            if ( GUILayout.Button( "Search", popup ) )
            {
                Vector2 searchModalPos = Event.current.mousePosition;
                SearchWindow.Open( new SearchWindowContext( GUIUtility.GUIToScreenPoint( searchModalPos ) ), new IngredientSearchProvider( m_IngredientsAssetData, i =>
                    m_SelectedIngredient = i
                 ) );
            }

            EditorGUILayout.Space( 5 );

            if ( m_SelectedIngredient != null && GUILayout.Button( "Add", button ) )
            {
                if ( ingredientQuantity <= 0 )
                {
                    Debug.LogError( "Quantity must bigger than 0" );
                    return;
                }
                for ( int i = 0; i < m_Ingredients.arraySize; i++ )
                {
                    SerializedProperty el = m_Ingredients.GetArrayElementAtIndex( i );
                    if ( el.FindPropertyRelative( "ingredient" ).objectReferenceValue == m_SelectedIngredient )
                    {
                        Debug.LogError( $"{m_SelectedIngredient.name} exists in list at index ({i})" );
                        return;
                    }
                }

                m_Ingredients.arraySize += 1;
                int index = m_Ingredients.arraySize - 1;
                SerializedProperty arrEl = m_Ingredients.GetArrayElementAtIndex( index );
                arrEl.FindPropertyRelative( "ingredient" ).objectReferenceValue = m_SelectedIngredient;
                arrEl.FindPropertyRelative( "quantity" ).intValue = ingredientQuantity;

            }

        }
        so.ApplyModifiedProperties();

    }

    private List<ItemData> LoadIngredientsData()
    {
        var guids = AssetDatabase.FindAssets( "t:ItemData", new[] { "Assets/Data/Ingredients" } );
        var paths = guids.Select( AssetDatabase.GUIDToAssetPath );
        var ingredientsData = paths.Select( AssetDatabase.LoadAssetAtPath<ItemData> ).ToList();

        return ingredientsData;
    }
}
