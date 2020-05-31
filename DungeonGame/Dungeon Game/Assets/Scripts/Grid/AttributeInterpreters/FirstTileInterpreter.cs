using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstTileInterpreter : TileInterpreter
{
    [SerializeField] private GameObject _firstTile = null;

    public override Generator.GeneratorType GetLinkedType()
    {
        return Generator.GeneratorType.STARTING_TILE;
    }

    public override void InterpretAttributes(KeyValuePair<Vector2Int, TileAttributes> attributes)
    {
#if LOG_INTERPRETATION
        if (!attributes.Value.IsTileType(Attributes.TileType.STARTING_TILE))
        {
            Debug.LogWarning("[INTERPRETATION]: Attempted to interpret first tile attribute when first tile attribute was not present!");
            return;
        }
#endif
        Vector3 pos = GridMaintaner.GetGridPositionOf(attributes.Key);
        Instantiate(_firstTile, pos, Quaternion.identity);
        LogSuccess("first tile");
    }
}
