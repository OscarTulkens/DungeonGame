using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BiomeGenerator : Generator
{
    public override void Generate(GridMaintaner gridMaintaner)
    {
        LogSuccess("biomes");
    }

    public override GeneratorType GetGeneratorType()
    {
        return GeneratorType.BIOMES;
    }
}
