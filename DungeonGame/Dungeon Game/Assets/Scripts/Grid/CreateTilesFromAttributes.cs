using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateTilesFromAttributes : MonoBehaviour
{
    private TileInterpreter[] _tileInterpreters = null;

    public void OnGeneratorsFinished(GridMaintaner grid)
    {
        Dictionary<Vector2Int, TileAttributes> attributes = grid.GetGridData();
        _tileInterpreters = GetComponents<TileInterpreter>();
        List<Generator.GeneratorType> types = grid.GetGeneratorTypes();

        foreach (KeyValuePair<Vector2Int, TileAttributes> tile in attributes)
        {
            foreach (TileInterpreter interpreter in _tileInterpreters)
            {
                if (types.Contains(interpreter.GetLinkedType()))
                    interpreter.InterpretAttributes(tile);
            }
        }

#if LOG_INTERPRETATION
        Debug.Log("[INTERPRETATION]: Tiles succesfully interpreted!");
#endif
    }
}
