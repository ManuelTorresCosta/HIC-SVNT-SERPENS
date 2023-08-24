using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointsManager : MonoBehaviour
{
    public Point pointPrefab;

    [Header("Rare Points")]

    public Transform commonPointsParent;
    public List<Point> commonPoints { get; private set; }
    public Point commonPoint { get; private set; }
    public int _commonPointsCaptured = 0;

    [Header("Common Points")]

    public Transform rarePointsParent;
    public List<Point> rarePoints { get; private set; }
    public Point rarePoint { get; private set; }
    public int _rarePointsCaptured = 0;
    public float rarePointValue;
    public float rarePointDevalueSpeed = 18f;


    // Unity functions
    private void Awake()
    {
        commonPoints = new List<Point>();
        rarePoints = new List<Point>();
    }
    private void Start()
    {
        // Get the points to the list
        for (int i = 0; i < commonPointsParent.childCount; i++)
        {
            Point point = commonPointsParent.GetChild(i).GetComponent<Point>();
            commonPoints.Add(point);

            point.gameObject.SetActive(false);
        }

        // Get the legends to the list
        for (int i = 0; i < rarePointsParent.childCount; i++)
        {
            Point point = rarePointsParent.GetChild(i).GetComponent<Point>();
            rarePoints.Add(point);

            point.gameObject.SetActive(false);
        }
    }


    // Functions
    public bool CanSpawnCommonPoint()
    {
        return commonPoint == null && rarePoint == null;
    }
    public void SpawnRandomCommonPoint(List<Segment> segments)
    {
        // Generate a random index from the list
        int randomIndex = Random.Range(0, commonPoints.Count);
        commonPoint = commonPoints[randomIndex];

        // Create a bool in case of the point being in the same index as the snake
        bool recursive = false;

        // Check if the snake is on the random tile
        foreach (Segment segment in segments)
        {
            // If the index is the same
            if (segment.Index == commonPoint.collisionIndices[0])
            {
                recursive = true;
                break;
            }
        }

        // Create a point
        if (!recursive)
        {
            // Enable object
            commonPoint.gameObject.SetActive(true);

            // Initialize it at the random position
            commonPoint.Initialize(commonPoint.transform.position, commonPoint.Index, TileType.Type.CommonPoint);
        }
        // Try again
        else
            SpawnRandomCommonPoint(segments);

    }
    public void DespawnCommonPoint()
    {
        if (commonPoint == null)
            return;

        // Remove point from the list
        commonPoints.Remove(commonPoint);

        // Remove gameobject from the scene
        Destroy(commonPoint.gameObject);

        // Turn the point null in order to respawn another
        commonPoint = null;

        // Only work towards next rare point if no rare points are present
        if (rarePoint == null)
            _commonPointsCaptured++;
    }
    
    public bool CanSpawnRarePoint()
    {
        if (rarePoint == null)
        {
            // Only spawn rare point if captured 3 common points
            if (_commonPointsCaptured >= 3)
            {
                _commonPointsCaptured = 0;
                return true;
            }
        }
        return false;
    }
    public void SpawnRandomRarePoint(List<Segment> segments)
    {
        bool lastPoint = _rarePointsCaptured >= 3;

        if (!lastPoint)
        {
            // Generate a random index from the list
            int randomIndex = Random.Range(0, rarePoints.Count);
            rarePoint = rarePoints[randomIndex];
        }
        // Spawns the last rarePoint
        else
        {
            foreach (Point point in rarePoints)
                if (point.collisionIndices[0] == new Vector2(5, 30))
                    rarePoint = point;
        }

        if (rarePoint != null)
        {
            // Create a bool in case of the point being in the same index as the snake
            bool recursive = false;

            // Check if the snake is on the random tile
            foreach (Segment segment in segments)
            {
                for (int i = 0; i < rarePoint.collisionIndices.Length; i++)
                {
                    // If the index is the same
                    if (segment.Index == rarePoint.collisionIndices[i] || commonPoint.collisionIndices[0] == rarePoint.collisionIndices[i])
                    {
                        recursive = true;
                        break;
                    }
                }
            }
            
            // Create a point
            if (!recursive || lastPoint)
            {
                // Enable object
                rarePoint.gameObject.SetActive(true);

                // Initialize it at the random position
                rarePoint.Initialize(rarePoint.transform.position, rarePoint.Index, TileType.Type.RarePoint);

                rarePointValue = rarePoint.Value;
            }
            // Try again
            else
                SpawnRandomRarePoint(segments);
        }
    }
    public void DespawnRarePoint(bool captured)
    {
        if (rarePoint == null)
            return;

        // Remove point from the list
        rarePoints.Remove(rarePoint);

        // Remove gameobject from the scene
        Destroy(rarePoint.gameObject);

        // Turn the point null in order to respawn another
        rarePoint = null;

        if (captured)
            _rarePointsCaptured++;
    }
    public bool IsRarePointTimeEnded()
    {
        if (rarePoint != null)
        {
            if (rarePointValue > 0)
            {
                rarePointValue -= rarePointDevalueSpeed * Time.deltaTime;
                rarePoint.Value = (int)rarePointValue;
                return false;
            }
            else
            {
                rarePointValue = 0;
                rarePoint.Value = (int)rarePointValue;
                return true;
            }
        }
        return false;
    }
    public bool MaxRarePointsCaptured()
    {
        return _rarePointsCaptured > 3;
    }

    // Old
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
            commonPoint = Instantiate(pointPrefab, transform);

            // Initialize it at the random position
            commonPoint.Initialize(randomTile.transform.position, randomTile.Index, TileType.Type.CommonPoint);
        }
        // Try again
        else
            GenerateRandomPoint(tiles, segments);
    }
}
