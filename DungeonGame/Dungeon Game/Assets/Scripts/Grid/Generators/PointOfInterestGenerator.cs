using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointOfInterestGenerator : Generator
{
    public override void Generate(GridMaintaner gridMaintaner)
    {
        LogSuccess("points of interest");
    }

    public override GeneratorType GetGeneratorType()
    {
        return GeneratorType.POINTS_OF_INTEREST;
    }
}
