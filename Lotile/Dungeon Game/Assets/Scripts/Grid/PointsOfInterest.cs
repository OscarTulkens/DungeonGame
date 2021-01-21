using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointsOfInterest : MonoBehaviour
{
    public enum POIType
    {
        NONE = 0,
        VILLAGE = Attributes.TileType.POINT_OF_INTEREST,
        DEAD_END,
    }

    [SerializeField] private List<POIType> _pointsOfInterest = null;

    public List<POIType> GetPointsOfInterest()
    {
        return _pointsOfInterest;
    }
}
