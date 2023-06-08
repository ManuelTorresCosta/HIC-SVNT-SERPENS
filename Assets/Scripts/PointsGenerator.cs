using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointsGenerator : MonoBehaviour
{
    public Point pointPrefab;

    public Point Point { get; private set; }


    public void GenerateRandomPoint(Tile[,] tiles, List<Segment> segments)
    {
        // Get a random index from the tile array
        int randomX = Random.Range(0, tiles.GetLength(0) - 1);
        int randomY = Random.Range(0, tiles.GetLength(1) - 1);

        // Get the tile from the random index
        Tile randomTile = tiles[randomX, randomY];

        // Create a bool in case of the point being in the same index as the snake
        bool recursive = false;

        // Check if the snake is on the random tile
        foreach (Segment segment in segments)
        {
            // If the index is the same
            if (segment.Index == randomTile.Index)
            {
                recursive = true;
                break;
            }
        }

        // Create a point
        if (!recursive)
        {
            // Instantiate object
            Point = Instantiate(pointPrefab, transform);

            // Initialize it at the random position
            Point.Initialize(randomTile.transform.position, randomTile.Index, TileType.Type.Point);
        }
        // Try again
        else
            GenerateRandomPoint(tiles, segments);
    }
    public void DespawnPoint()
    {
        if (Point == null)
            return;

        // Remove gameobject from the scene
        Destroy(Point.gameObject);

        // Turn the point null in order to respawn another
        Point = null;
    }
}
