using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class IngredientSearchProvider : ScriptableObject, ISearchWindowProvider
{
    private readonly List<ItemData> ingredients;
    private readonly Action<ItemData> callBack;
    public IngredientSearchProvider( List<ItemData> recipes, Action<ItemData> callBack )
    {
        this.ingredients = recipes;
        this.callBack = callBack;
    }
    public List<SearchTreeEntry> CreateSearchTree( SearchWindowContext context )
    {
        List<SearchTreeEntry> tree = new()
        {
            new SearchTreeGroupEntry( new GUIContent( "List" ), 0 ),
        };
        ingredients.Sort( ( x, y ) => x.name.CompareTo( y.name ) );
        Texture icon = EditorGUIUtility.IconContent( "d_ScriptableObject Icon" ).image;
        ingredients.ForEach( ingredient =>
        {
            SearchTreeEntry searchTreeEntry = new( new GUIContent( ingredient.name, icon ) )
            {
                level = 1,
                userData = ingredient
            };
            tree.Add( searchTreeEntry );
        } );

        return tree;
    }

    public bool OnSelectEntry( SearchTreeEntry SearchTreeEntry, SearchWindowContext context )
    {
        callBack?.Invoke( SearchTreeEntry.userData as ItemData );
        return true;
    }

}
