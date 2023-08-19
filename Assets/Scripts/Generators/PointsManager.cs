using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointsManager : MonoBehaviour
{
    public Point pointPrefab;

    public Transform commonPointsParent;
    public List<Point> commonPoints { get; private set; }
    public Point commonPoint { get; private set; }
    public bool CanSpawnCommonPoint()
    {
        return commonPoint == null;
    }
    private int _totalCommonPoints;

    public Transform rarePointsParent;
    public List<Point> rarePoints { get; private set; }
    public Point rarePoint { get; private set; }
    public bool CanSpawnRarePoint()
    {
        if (rarePoint == null)
        {
            if (_totalCommonPoints - commonPoints.Count == 3)
            {
                _totalCommonPoints = commonPoints.Count;
                return true;
            }
        }
        return false;
    }

    public float rarePoointDespawnTimer = 10f;
    private float _rarePointTimer = 0f;
    public bool IsRarePointTimeEnded()
    {
        if (rarePoint != null)
        {
            if (_rarePointTimer < rarePoointDespawnTimer)
            {
                _rarePointTimer += Time.deltaTime;
                return false;
            }
            else
            {
                _rarePointTimer = rarePoointDespawnTimer;
                _totalCommonPoints = commonPoints.Count;
                return true;
            }
        }
        return false;
    }
    private int _rarePointsCapturedCounter = 0;
    public bool MaxRarePointsCaptured()
    {
        return _rarePointsCapturedCounter > 3;
    }


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
        }
        _totalCommonPoints = commonPoints.Count;

        // Get the legends to the list
        for (int i = 0; i < rarePointsParent.childCount; i++)
        {
            // ignore the last rarePoint to trigger the endGame
            if (i != 13)
            {
                Point point = rarePointsParent.GetChild(i).GetComponent<Point>();
                rarePoints.Add(point);
            }
        }
    }


    // Functions
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
    }

    public void SpawnRandomRarePoint(List<Segment> segments)
    {
        if (_rarePointsCapturedCounter < 3)
        {
            // Generate a random index from the list
            int randomIndex = Random.Range(0, rarePoints.Count);
            rarePoint = rarePoints[randomIndex];
        }
        // Spawns the last rarePoint
        else
            rarePoint = rarePoints[13];

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
            if (!recursive)
            {
                // Enable object
                rarePoint.gameObject.SetActive(true);

                // Initialize it at the random position
                rarePoint.Initialize(rarePoint.transform.position, rarePoint.Index, TileType.Type.RarePoint);
            }
            // Try again
            else
                SpawnRandomRarePoint(segments);
        }
    }
    public void DespawnRarePoint(bool captured = true)
    {
        if (rarePoint == null)
            return;

        // Remove point from the list
        rarePoints.Remove(commonPoint);

        // Remove gameobject from the scene
        Destroy(rarePoint.gameObject);

        // Turn the point null in order to respawn another
        rarePoint = null;

        _rarePointTimer = 0f;

        if (captured)
            _rarePointsCapturedCounter++;
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
