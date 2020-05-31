using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Attributes
{
    public enum TileType
    {
        EMPTY = 0,
        STARTING_TILE = 1,
        REGULAR = 2,
        POINT_OF_INTEREST = 3,
        POINT_OF_INTEREST_MAIN = 4,
    }

    public enum OpenDirections
    {
        NONE = 0,
        LEFT = 1 << 0,
        RIGHT = 1 << 1,
        UP = 1 << 2,
        DOWN = 1 << 3,
#region left
        L_R = LEFT | RIGHT,
        L_U = LEFT | UP,
        L_D = LEFT | DOWN,
#endregion
#region right
        R_L = L_R,
        R_U = RIGHT | UP,
        R_D = RIGHT | DOWN,
#endregion
#region up
        U_L = L_U,
        U_R = R_U,
        U_D = UP | DOWN,
#endregion
#region down
        D_L = L_D,
        D_R = R_D,
        D_U = U_D,
#endregion
#region triplets
        L_R_U = LEFT | RIGHT | UP,
        L_R_D = LEFT | RIGHT | DOWN,
        L_U_D = LEFT | UP | DOWN,
        R_U_D = RIGHT | UP | DOWN,
#endregion
        ALL = ~0
    }
}
