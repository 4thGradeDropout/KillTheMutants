using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class PathFinder
{
    public static int[,] GeneralMap;

    private int GetDistanceBetweenNeighbours()
    {
        return 1;
    }

    private int GetHeuristicPathLength(Point from, Point to)
    {
        return Math.Abs(from.X - to.X) + Math.Abs(from.Y - to.Y);
    }

    public List<Point> FindPath(Point start, Point goal)
    {
        return FindPath(GeneralMap, start, goal);
    }

    public List<Point> FindPath(int[,] field, Point start, Point goal)
    {
        // Шаг 1.
        var closedSet = new List<PathNode>();
        var openSet = new List<PathNode>();
        // Шаг 2.
        PathNode startNode = new PathNode()
        {
            Position = start,
            CameFrom = null,
            PathLengthFromStart = 0,
            HeuristicEstimatePathLength = GetHeuristicPathLength(start, goal)
        };
        openSet.Add(startNode);
        while (openSet.Count > 0)
        {
            // Шаг 3.
            var currentNode = openSet.OrderBy(node =>
              node.EstimateFullPathLength).First();
            //Console.WriteLine(currentNode);
            // Шаг 4.
            if (currentNode.Position == goal)
                return GetPathForNode(currentNode);
            // Шаг 5.
            openSet.Remove(currentNode);
            closedSet.Add(currentNode);
            // Шаг 6.
            var neighbours = GetNeighbours(currentNode, goal, field);
            //Console.WriteLine("Соседи текущего узла: ");
            //neighbours.ForEach(n => Console.WriteLine(n));
            foreach (var neighbourNode in neighbours)
            {
                // Шаг 7.
                if (closedSet.Count(node => node.Position == neighbourNode.Position) > 0)
                    continue;
                var openNode = openSet.FirstOrDefault(node =>
                  node.Position == neighbourNode.Position);
                // Шаг 8.
                if (openNode == null)
                    openSet.Add(neighbourNode);
                else
                  if (openNode.PathLengthFromStart > neighbourNode.PathLengthFromStart)
                {
                    // Шаг 9.
                    openNode.CameFrom = currentNode;
                    openNode.PathLengthFromStart = neighbourNode.PathLengthFromStart;
                }
            }
        }
        // Шаг 10.
        return null;
    }

    private List<PathNode> GetNeighbours(PathNode pathNode,
Point goal, int[,] field)
    {
        var result = new List<PathNode>();

        // Соседними точками являются соседние по стороне клетки.
        Point[] neighbourPoints = new Point[4];
        neighbourPoints[0] = new Point { X = pathNode.Position.X + 1, Y = pathNode.Position.Y };
        neighbourPoints[1] = new Point { X = pathNode.Position.X - 1, Y = pathNode.Position.Y };
        neighbourPoints[2] = new Point { X = pathNode.Position.X, Y = pathNode.Position.Y + 1 };
        neighbourPoints[3] = new Point { X = pathNode.Position.X, Y = pathNode.Position.Y - 1 };
        //Console.WriteLine("Предполагаемые соседи: ");
        //neighbourPoints.ToList().ForEach(np => Console.WriteLine(np));
        foreach (var point in neighbourPoints)
        {
            //Console.WriteLine($"Текущая точка - {point}");
            // Проверяем, что не вышли за границы карты.
            if (point.X < 0 || point.X >= field.GetLength(0))
            {
                //Console.WriteLine($"Вышли за границу по Х.");
                continue;
            }
            if (point.Y < 0 || point.Y >= field.GetLength(1))
            {
                //Console.WriteLine($"Вышли за границу по У.");
                continue;
            }
            // Проверяем, что по клетке можно ходить.
            if (field[point.X, point.Y] == 1)
            {
                //Console.WriteLine($"Это стена.");
                continue;
            }
            // Заполняем данные для точки маршрута.
            var neighbourNode = new PathNode()
            {
                Position = point,
                CameFrom = pathNode,
                PathLengthFromStart = pathNode.PathLengthFromStart +
                GetDistanceBetweenNeighbours(),
                HeuristicEstimatePathLength = GetHeuristicPathLength(point, goal)
            };
            result.Add(neighbourNode);
        }
        return result;
    }

    private List<Point> GetPathForNode(PathNode pathNode)
    {
        var result = new List<Point>();
        var currentNode = pathNode;
        while (currentNode != null)
        {
            result.Add(currentNode.Position);
            currentNode = currentNode.CameFrom;
        }
        result.Reverse();
        return result;
    }
}
