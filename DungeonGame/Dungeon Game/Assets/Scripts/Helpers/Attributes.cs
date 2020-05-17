using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Attributes
{
    static Dictionary<string, int> _attributes = new Dictionary<string, int>();
    static int _attributeCount = 0;

    public static void AddAttribute(string attribute)
    {
#if LOG_ATTRIBUTES
        Debug.Log("[ATTRIBUTES]: Attribute added: \"" + attribute + "\" at index: " + _attributeCount.ToString());
#endif
        _attributes.Add(attribute, _attributeCount++);
    }

    public static int GetAttribute(string attribute)
    {
        return _attributes[attribute];
    }
}
