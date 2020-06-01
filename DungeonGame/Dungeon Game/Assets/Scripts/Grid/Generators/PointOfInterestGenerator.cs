using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointOfInterestGenerator : Generator
{
    private static Dictionary<PointsOfInterest.POIType, PointOfInterest> _pointOfInterestPrefabs = new Dictionary<PointsOfInterest.POIType, PointOfInterest>();

    public override void Generate(GridMaintaner gridMaintaner)
    {
        if (_pointOfInterestPrefabs.Count == 0)
            GrabPOIPrefabs();

        #region TEMP
        List<Vector2Int> points = gridMaintaner.GetComponent<TEMP_SpecificPointsOfInterest>().Points;
        foreach (Vector2Int point in points)
        {
            PointOfInterest poi = _pointOfInterestPrefabs[PointsOfInterest.POIType.DEAD_END];
            List<Vector2Int> offsets = poi.GetPointOffsets();
            foreach (Vector2Int offset in offsets)
            {
                TileAttributes attr = gridMaintaner.GetTileAt(point + offset);
                attr.SetOpenDirections(poi.GetOpenDirectionsOfTile(offset));
                attr.SetTileType(Attributes.TileType.POINT_OF_INTEREST);
            }
            TileAttributes ti = gridMaintaner.GetTileAt(point);
            ti.SetTileType(Attributes.TileType.POINT_OF_INTEREST_MAIN);
        }
        #endregion

        LogSuccess("points of interest");
    }

    private void GrabPOIPrefabs()
    {
        GameObject[] objects = Resources.LoadAll<GameObject>("PointsOfInterest");
        foreach (GameObject o in objects)
        {
            PointOfInterest poi = o.GetComponent<PointOfInterest>();
            _pointOfInterestPrefabs.Add(poi.GetPOIType(), poi);
        }
    }

    public static PointOfInterest GetPointOfInterest(PointsOfInterest.POIType type)
    {
        return _pointOfInterestPrefabs[type];
    }

    public override GeneratorType GetGeneratorType()
    {
        return GeneratorType.POINTS_OF_INTEREST;
    }
}
