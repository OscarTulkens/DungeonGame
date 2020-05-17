using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstTileGenerator : Generator
{
    public override void Generate(GridMaintaner gridMaintaner)
    {
        Attributes.AddAttribute("left open");
        Attributes.AddAttribute("right open");
        Attributes.AddAttribute("up open");
        Attributes.AddAttribute("down open");
        Attributes.AddAttribute("first tile");

        TileAttributes tile = gridMaintaner.GetTileAt(Vector2Int.zero);
        tile.AddAttribute(Attributes.GetAttribute("first tile"));

        LogSuccess("first tile");
    }

    public override GeneratorType GetGeneratorType()
    {
        return GeneratorType.STARTING_TILE;
    }
}
