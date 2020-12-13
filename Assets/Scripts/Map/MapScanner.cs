using UnityEngine;
using UnityEngine.Tilemaps;

public class MapScanner
{
    public MapScanner(Tilemap map)
    {
        Map = map;
    }

    public float InitialSearchRadius { get; protected set; } = 0.1f;

    /// <summary>
    /// Радиус поиска с каждой итерацией будет увеличиваться на эту величину
    /// </summary>
    public float SearchRadiusStep { get; protected set; } = 0.3f;

    public float SlightExtensionQuotient { get; protected set; } = 2f;

    public int PointsCountPerIteration { get; protected set; } = 35;

    protected int MaxSearchIterations { get; set; } = 50;

    protected Tilemap Map { get; set; }

    /// <summary>
    /// Возвращает ближайшую незанятую точку к точке initialPoint
    /// </summary>
    /// <param name="initialPoint"></param>
    /// <returns></returns>
    public Vector2 GetNearestEmptyPoint(Vector2 initialPoint)
    {
        Debug.Log("Starting to search nearest unoccupied point...");
        int counter = 0;
        float currentRadius = InitialSearchRadius;
        do
        {
            for (int i = 0; i < PointsCountPerIteration; i++)
            {
                Vector2 randomPointOnUnitCircle = Random.insideUnitCircle.normalized;
                Vector2 randomPointOnCircle = randomPointOnUnitCircle * currentRadius;
                Vector2 randomPointAroundInitialPoint = randomPointOnCircle + initialPoint;
                if (!PointIsOccupied(randomPointAroundInitialPoint))
                {
                    //Debug.Log($"Found empty point! It's {randomPointAroundInitialPoint}");
                    Vector2 slightlyExtended = SlightExtensionQuotient * randomPointOnCircle + initialPoint;
                    return slightlyExtended;
                }
            }
            currentRadius += SearchRadiusStep;
            counter++;
            if (counter >= MaxSearchIterations)
            {
                Debug.Log("Didn't manage to find empty point :(");
                return initialPoint;
            }
        } while (true);
    }

    protected bool PointIsOccupied(Vector2 uwPoint)
    {
        var mapGenerator = GameObject.Find("ObstaclesMapGenerator").GetComponent<ObstaclesMapGenerator>();
        var converter = new TileCoordSystemConverter(mapGenerator.WorldSize, mapGenerator.CellSize);
        var usPoint = converter.UW_To_US(uwPoint);
        var tileAtLocation = Map.GetTile(new Vector3Int(usPoint.X, usPoint.Y, 0));
        bool result = tileAtLocation != null;
        if (result)
        {
            return result;
        }

        //bool result = false;
        var colliderOnPoint = Physics2D.OverlapCircle(uwPoint, 0.01f, 0, -100f, 100f);
        result = colliderOnPoint != null;

        return result;
    }
}
