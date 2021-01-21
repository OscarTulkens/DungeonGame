using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridMaintaner : MonoBehaviour
{
    [SerializeField] private CreateTilesFromAttributes _tileCreator = null;
    private static Grid _grid = null;
    private Dictionary<Vector2Int, TileAttributes> _gridData = new Dictionary<Vector2Int, TileAttributes>();
    
    [SerializeField] private List<Generator.GeneratorType> _generatorOrder = null;

    private void Awake()
    {
        _grid = GetComponent<Grid>();
    }

    public Dictionary<Vector2Int, TileAttributes> GetGridData()
    {
        return _gridData;
    }

    public List<Generator.GeneratorType> GetGeneratorTypes()
    {
        return _generatorOrder;
    }

    public static Vector3 GetGridPositionOf(Vector2Int gridPos)
    {
        return _grid.CellToWorld(new Vector3Int(gridPos.x, 0, gridPos.y));
    }

    private void Start()
    {
        GenerateGrid();
        _tileCreator.OnGeneratorsFinished(this);
    }

    private void GenerateGrid()
    {
        Generator[] generators = Generator.GetGenerators();
        for (int i = 0, count = _generatorOrder.Count; i < count; ++i)
        {
            Generator gen = FindNextGenerator(_generatorOrder[i], generators);
            gen.Generate(this);
        }
    }

    private Generator FindNextGenerator(Generator.GeneratorType type, Generator[] generators)
    {
        foreach (Generator gen in generators)
        {
            if (gen.GetGeneratorType() == type)
                return gen;
        }
        return null;
    }

    public TileAttributes GetTileAt(Vector2Int gridPos)
    {
        if (!_gridData.ContainsKey(gridPos))
            _gridData.Add(gridPos, new TileAttributes());

        return _gridData[gridPos];
    }
}
