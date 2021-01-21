using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class POIInterpreter : TileInterpreter
{
    public override Generator.GeneratorType GetLinkedType()
    {
        return Generator.GeneratorType.POINTS_OF_INTEREST;
    }

    public override void InterpretAttributes(KeyValuePair<Vector2Int, TileAttributes> attributes)
    {
#if LOG_INTERPRETATION
        if (attributes.Value.GetTileType() < Attributes.TileType.POINT_OF_INTEREST)
        {
            Debug.LogWarning("[INTERPRETATION]: Attempted to interpret point of interest attibute when point of interest attribute was not present!");
            return;
        }
#endif

        Vector3 pos = GridMaintaner.GetGridPositionOf(attributes.Key);
        Instantiate(PointOfInterestGenerator.GetPointOfInterest((PointsOfInterest.POIType)attributes.Value.GetTileType()), pos, Quaternion.identity);
        LogSuccess("point of interest");
    }
}
