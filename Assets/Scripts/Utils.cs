using UnityEngine;
public class Utils
{
    //LAYER
    public static int FarmGroundMask = 1 << LayerMask.NameToLayer( "Farm Ground" );
    public static int RestaurantGroundMask = 1 << LayerMask.NameToLayer( "Restaurant Ground" );
    public static int RaycastableMask = 1 << LayerMask.NameToLayer( "Raycastable" );
    public static int GroundMask = 1 << LayerMask.NameToLayer( "Ground" );
    public static int RestaurantMask = 1 << LayerMask.NameToLayer( "Restaurant" );
    public static int HandLayer = LayerMask.NameToLayer( "Hand" );

    //INPUT ACTION STRING
    public const string MAIN_ACTION = "Main Action";
    public static readonly string[] SELECT_SLOT_ACTION =
    {
        "SelectActionSlot1",
        "SelectActionSlot2",
        "SelectActionSlot3",
        "SelectActionSlot4",
        "SelectActionSlot5",
        "SelectActionSlot6",
    };
    public const string OBJECT_ROTATION_ACTION = "Object Rotation";
    public const string PLAYER_ROTATION_ACTION = "Rotation";
    public const string JUMP_ACTION = "Jump";
    public const string MOVE_ACTION = "Move";
    public const string STORE_ACTION = "Store";
    public const string DROP_ACTION = "Drop";
    public const string INVENTORY_ACTION = "Inventory";
    public const string OPENUI_ACTION = "OpenUI";
    public const string ALT_ACTION = "Alt";

    //TAGS
    public const string FARM_GROUND_TAG = "Farm Ground";
    public const string RESTAURANT_GROUND_TAG = "Restaurant Ground";
    public const string TILE_TAG = "Tile";
    public const string PROP_TAG = "Props";
    public const string TILE_PARENT_TAG = "Tile Parent";
    public const string RESTAURANT_TAG = "Restaurant";
    public const string TABLE_TAG = "Table";
    public const string SEAT_TAG = "SEAT";
    public const string TILE_WET_TAG = "Wet Tile";
    public const string CROP_TAG = "Crop";

}
