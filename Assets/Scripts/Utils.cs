using UnityEngine;
public class Utils
{
    //LAYER
    public static int FarmGroundMask = 1 << LayerMask.NameToLayer("Farm Ground");
    public static int RestaurantGroundMask = 1 << LayerMask.NameToLayer("Restaurant Ground");
    public static int RaycastableMask = 1 << LayerMask.NameToLayer("Raycastable") | 1 << LayerMask.NameToLayer("Outline");
    public static int GroundMask = 1 << LayerMask.NameToLayer("Ground");
    public static int PlayerMask = LayerMask.GetMask("Player", "Helper");
    public static int RestaurantMask = 1 << LayerMask.NameToLayer("Restaurant");
    public static int UILayer = LayerMask.NameToLayer("UI");
    public static int HandLayer = LayerMask.NameToLayer("Hand");
    public static int OutlineLayer = LayerMask.NameToLayer("Outline");
    public static int RaycastableLayer = LayerMask.NameToLayer("Raycastable");
    public static int HelperLayer = LayerMask.NameToLayer("Helper");
    public static int DefaultMask = (1 << 0) | (1 << 1) | (1 << 2) | (1 << 3) | (1 << 4) | (1 << 5) | (1 << 13);
    public static int CityMask = DefaultMask | (1 << 6) | (1 << 8) | (1 << 12);
    public static int HouseMask = DefaultMask | (1 << 14) | (1 << 8) | (1 << 15);
    public static int FarmMask = DefaultMask | (1 << 8) | (1 << 9);

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
    public const string PAUSE_ACTION = "Pause";
    //TAGS
    public const string GROUND_TAG = "Ground";
    public const string RESTAURANT_GROUND_TAG = "Restaurant Ground";
    public const string FARM_GROUND_TAG = "Farm Ground";
    public const string TILE_TAG = "Tile";
    public const string PROP_TAG = "Props";
    public const string TILE_PARENT_TAG = "Tile Parent";
    public const string RESTAURANT_TAG = "Restaurant";
    public const string TABLE_TAG = "Table";
    public const string SEAT_TAG = "SEAT";
    public const string TILE_WET_TAG = "Wet Tile";
    public const string CROP_TAG = "Crop";
    public const string PLAYER_TAG = "Player";
    public const string TREE_OBSTACLE_TAG = "Tree Obstacle";

    //Anim
    public static int NPC_WALK_ANIM_PARAM = Animator.StringToHash("Walk");
    public static int NPC_EAT_ANIM_PARAM = Animator.StringToHash("Eat");
    public static int NPC_SIT_ANIM_PARAM = Animator.StringToHash("Sit");
    public static int NPC_SPEED_ANIM_PARAM = Animator.StringToHash("Speed");
    public static int NPC_COOKING_ANIM_PARAM = Animator.StringToHash("Cooking");


    //Scenes
    public const string SCENE_HOUSE = "House";
    public const string SCENE_CITY = "City";
    public const string SCENE_FARM = "Farm";
    public const string SCENE_MAINMENU = "MainMenu";

    //Save
    public const string FARM_OBJECTS_FILENAME = "farmobjects.json";
    public const string GAME_TIME_FILENAME = "gametime.json";
    public const string RESTAURANT_OBJECTS_FILENAME = "restaurantobject.json";
    public const string FOODS_FILENAME = "recipe.json";
    public const string FARM_GENERATOR_TIME_FILENAME = "farmgeneratortime.json";
    public const string PLAYERDATA_FILENAME = "playerdata.json";

    public const string BUTTON_SFX = "button_sfx";


}
