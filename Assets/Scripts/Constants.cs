
public static class Constants
{
    #region map
    public const int FLOOR_INDEX = 0;
    public const int NORTH_WALL_INDEX = 1;
    public const int EAST_WALL_INDEX = 2;
    public const int WEST_WALL_INDEX = 3;
    public const int SOUTH_WALL_INDEX = 4;
    public const int CEILING_INDEX = 5;
    #endregion

    #region food
    public const int FOOD_CUBE = 0;
    public const int FOOD_SPHERE = 1;
    public const int FOOD_RED_CUBE = 2;
    public const int FOOD_GREEN_CUBE = 3;
    public const int FOOD_BLUE_CUBE = 4;
    public const int FOOD_RED_SPHERE = 5;
    public const int FOOD_GREEN_SPHERE = 6;
    public const int FOOD_BLUE_SPHERE = 7;
    #endregion

    #region scouting
    public const int SCOUT_STILL = 0;
    public const int SCOUT_COLLISION = 1;
    public const int SCOUT_RANDOM = 2;
    public const int SCOUT_COLLISION_PARALLEL = 3;
    public const int SCOUT_RANDOM_PARALLEL = 4;
    /*public const int GREEN_CUBE = 3;
    public const int BLUE_CUBE = 4;
    public const int RED_SPHERE = 5;
    public const int GREEN_SPHERE = 6;
    public const int BLUE_SPHERE = 7;*/
    #endregion

    #region scan
    public const int SCAN_LEFT = 0;
    public const int SCAN_RIGHT = 1;
    public const int SCAN_SPRINKLER = 2;
    public const int SCAN_RANDOM = 3;
    /*public const int BLUE_CUBE = 4;
    public const int RED_SPHERE = 5;
    public const int GREEN_SPHERE = 6;
    public const int BLUE_SPHERE = 7;*/
    #endregion

    #region action
    public const int ACTION_IDLE= 0;
    public const int ACTION_SCAN = 1;
    public const int ACTION_SCOUT = 2;

    /*public const int BLUE_CUBE = 4;
    public const int RED_SPHERE = 5;
    public const int GREEN_SPHERE = 6;
    public const int BLUE_SPHERE = 7;*/
    #endregion
}
