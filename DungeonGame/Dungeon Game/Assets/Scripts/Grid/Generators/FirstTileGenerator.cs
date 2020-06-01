using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstTileGenerator : Generator
{
    public override void Generate(GridMaintaner gridMaintaner)
    {
        TileAttributes tile = gridMaintaner.GetTileAt(Vector2Int.zero);
        tile.SetTileType(Attributes.TileType.STARTING_TILE);
        tile.SetOpenDirections(Attributes.OpenDirections.ALL);

        LogSuccess("first tile");
    }

    public override GeneratorType GetGeneratorType()
    {
        return GeneratorType.STARTING_TILE;
    }
}
