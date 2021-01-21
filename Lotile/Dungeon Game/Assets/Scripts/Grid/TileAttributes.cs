using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileAttributes
{
    Attributes.TileType _type = Attributes.TileType.EMPTY;
    Attributes.OpenDirections _openDirections = Attributes.OpenDirections.NONE;

    public void SetTileType(Attributes.TileType tileType)
    {
        _type = tileType;
    }

    public Attributes.TileType GetTileType()
    {
        return _type;
    }

    public bool IsTileType(Attributes.TileType tileType)
    {
        return _type == tileType;
    }

    public void SetOpenDirections(Attributes.OpenDirections openDirections)
    {
        _openDirections = openDirections;
    }

    public void AddOpenDirections(Attributes.OpenDirections openDirections)
    {
        _openDirections |= openDirections;
    }

    public void RemoveOpenDirections(Attributes.OpenDirections openDirections)
    {
        _openDirections &= ~openDirections;
    }

    public Attributes.OpenDirections GetOpenDirections()
    {
        return _openDirections;
    }

    public bool IsOpenAt(Attributes.OpenDirections openDirections)
    {
        return (_openDirections & openDirections) != 0;
    }
}
