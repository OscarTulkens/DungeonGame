using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointOfInterest : MonoBehaviour
{
    [SerializeField] private PointsOfInterest.POIType _poiType = PointsOfInterest.POIType.NONE;
    [SerializeField] private List<Vector2Int> _pointOffsets = new List<Vector2Int> { new Vector2Int(0, 0) };
    [SerializeField] private List<Attributes.OpenDirections> _openDirections = new List<Attributes.OpenDirections> { Attributes.OpenDirections.ALL };

    public PointsOfInterest.POIType GetPOIType()
    {
        return _poiType;
    }

    public List<Vector2Int> GetPointOffsets()
    {
        return _pointOffsets;
    }

    public Attributes.OpenDirections GetOpenDirectionsOfTile(Vector2Int offset)
    {
        return _openDirections[_pointOffsets.IndexOf(offset)];
    }
}
