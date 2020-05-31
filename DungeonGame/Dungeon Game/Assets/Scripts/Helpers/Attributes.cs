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
        POINT_OF_INTEREST = 3
    }

    public enum OpenDirections
    {
        NONE = 0,
        LEFT = 1 << 0,
        RIGHT = 1 << 1,
        UP = 1 << 2,
        DOWN = 1 << 3,
        ALL = ~0
    }
}
