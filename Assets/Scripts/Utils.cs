using UnityEngine;
public class Utils
{
    public static int FarmGroundMask = 1 << LayerMask.NameToLayer( "Farm Ground" );
    public static int RestaurantGroundMask = 1 << LayerMask.NameToLayer( "Restaurant Ground" );
    public static int RaycastableMask = 1 << LayerMask.NameToLayer( "Raycastable" );
    public static int GroundMask = 1 << LayerMask.NameToLayer( "Ground" );

    public const string MAIN_ACTION = "Main Action";
    public const string ITEM_SELECTION_ACTION = "Item Selection";
    public const string OBJECT_ROTATION_ACTION = "Object Rotation";
    public const string PLAYER_ROTATION_ACTION = "Rotation";
    public const string JUMP_ACTION = "Jump";
    public const string MOVE_ACTION = "Move";
    public const string STORE_ACTION = "Store";

    public const string FARM_GROUND_TAG = "Farm Ground";
    public const string RESTAURANT_GROUND_TAG = "Restaurant Ground";
    public const string TILE_TAG = "Tile";
    public const string PROP_TAG = "Props";
    public const string TILE_PARENT_TAG = "Tile Parent";

}
