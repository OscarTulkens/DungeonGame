using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmptyTileGenerator : Generator
{
    public override void Generate(GridMaintaner gridMaintaner)
    {
        LogSuccess("empty tiles");
    }

    public override GeneratorType GetGeneratorType()
    {
        return GeneratorType.EMPTY_TILES;
    }
}
