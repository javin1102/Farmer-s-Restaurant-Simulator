using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class RestaurantManager : MonoBehaviour
{
    public static RestaurantManager Instance { get => m_Instance; }
    public Dictionary<RecipeData, bool> Recipes { get => m_Recipes; }
    public List<Table> Tables { get => m_Tables; }
    public List<Seat> UnoccupiedSeats { get => m_Seats; }


    [SerializeField] private List<Table> m_Tables = new();
    [SerializeField] private List<Seat> m_Seats = new();
    private int RandomIndex { get => Random.Range( 0, m_Seats.Count ); }
    private Dictionary<RecipeData, bool> m_Recipes = new();
    private static RestaurantManager m_Instance;
    private bool m_FirstLoad = true;

    private void Awake()
    {
        if ( m_Instance == null ) m_Instance = this;
        else Destroy( gameObject );

        //Check if player has save file
        if ( m_FirstLoad ) LoadRecipeData();
    }

    public bool FindUnoccupiedSeat( out Seat seat )
    {
        if ( m_Seats.Count == 0 )
        {
            seat = null;
            return false;
        }

        seat = m_Seats[RandomIndex];
        return true;
    }
    public void OrderFood()
    {
        //1.Get food from unlocked recipes
        //2.Check stock to make food

    }
    private void LoadRecipeData()
    {
        //ItemData[] x = Resources.LoadAll<ItemData>( "Data" );
        //foreach ( var item in x )
        //{
        //    Debug.Log( item.id );
        //}

        var guids = AssetDatabase.FindAssets( "t:RecipeData", new[] { "Assets/Data/Recipes" } );
        var paths = guids.Select( AssetDatabase.GUIDToAssetPath );
        var recipeData = paths.Select( AssetDatabase.LoadAssetAtPath<RecipeData> ).ToList();
        recipeData.ForEach( recipe => m_Recipes.Add( recipe, false ) );
    }
}
