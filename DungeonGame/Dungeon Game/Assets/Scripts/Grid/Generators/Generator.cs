using UnityEngine;

public abstract class Generator
{
    public enum GeneratorType
    {
        UNDEFINED = 0,
        POINTS_OF_INTEREST = 1,
        EMPTY_TILES = 2,
        BIOMES = 3,
        STARTING_TILE = 4
    }

    public abstract void Generate(GridMaintaner gridMaintaner);

    public abstract GeneratorType GetGeneratorType();

    public static Generator[] GetGenerators()
    {
        return new Generator[] 
        { 
            new FirstTileGenerator(),
            new PointOfInterestGenerator(), 
            new EmptyTileGenerator(), 
            new BiomeGenerator() 
        };
    }

    protected void LogSuccess(string name)
    {
#if LOG_GENERATION
        Debug.Log("[GENERATION]: Successfully generated " + name + "!");
#endif
    }
}
