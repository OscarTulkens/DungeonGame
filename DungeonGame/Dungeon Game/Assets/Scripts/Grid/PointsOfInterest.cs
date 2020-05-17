using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointsOfInterest : MonoBehaviour
{
    public enum POIType
    {
        NONE = 0,
        VILLAGE = 1,
        FILET_MIGNON = 2
    }

    [SerializeField] private List<POIType> _pointsOfInterest = null;

    public List<POIType> GetPointsOfInterest()
    {
        return _pointsOfInterest;
    }
}
