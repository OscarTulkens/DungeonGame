using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileAttributes
{
    private List<int> _attributes = new List<int>();

    public void AddAttribute(int attribute)
    {
        _attributes.Add(attribute);
    }

    public bool HasAttribute(int attribute)
    {
        return _attributes.Contains(attribute);
    }
}
