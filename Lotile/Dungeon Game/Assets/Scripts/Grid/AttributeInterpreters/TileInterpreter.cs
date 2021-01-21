using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TileInterpreter : MonoBehaviour
{
    public abstract void InterpretAttributes(KeyValuePair<Vector2Int, TileAttributes> attributes);

    public abstract Generator.GeneratorType GetLinkedType();

    protected static void LogSuccess(string name)
    {
#if LOG_INTERPRETATION
        Debug.Log("[INTERPRETATION]: Successfully interpreted " + name + "!");
#endif
    }
}