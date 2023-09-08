using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointsManager : MonoBehaviour
{
    public Point pointPrefab;

    [Header("Rare Points")]

    public Transform talesParent;
    public List<Point> talesList { get; private set; }
    public List<Point> talesGraveyard { get; private set; }
    public Point Tale { get; private set; }
    public int _talesCaptured = 0;

    [Header("Common Points")]

    public Transform stonesParent;
    public GameObject lastStonePrefab;
    public List<Point> stonesList { get; private set; }
    public List<Point> stonesGraveyard { get; private set; }
    public Point Stone { get; private set; }
    public int _stonesCaptured = 0;
    public int maxStones = 2;
    public bool ignoreLastPoint = false;
    public float stoneValue;
    public float stoneDevalueSpeed = 20f;


    // Unity functions
    private void Awake()
    {
        talesList = new List<Point>();
        talesGraveyard = new List<Point>();

        stonesList = new List<Point>();
        stonesGraveyard = new List<Point>();
    }
    private void Start()
    {
        // Get the tales to the list
        for (int i = 0; i < talesParent.childCount; i++)
        {
            Point point = talesParent.GetChild(i).GetComponent<Point>();
            talesList.Add(point);

            point.gameObject.SetActive(false);
        }

        // Get the stones to the list
        for (int i = 0; i < stonesParent.childCount; i++)
        {
            Point point = stonesParent.GetChild(i).GetComponent<Point>();
            stonesList.Add(point);

            point.gameObject.SetActive(false);
        }
    }


    // Functions
    public bool CanSpawnTale()
    {
        return Tale == null && Stone == null;
    }
    public void SpawnTale(List<Segment> segments)
    {
        int randomIndex;
        int dir = _talesCaptured % 2 != 0 ? 1 : -1;
        
        if (dir == 1)
            randomIndex = Random.Range(talesList.Count / 2, talesList.Count);
        else
            randomIndex = Random.Range(0, talesList.Count / 2);
        
        Tale = talesList[randomIndex];
        
        // Create a bool in case of the point being in the same index as the snake
        bool recursive = false;

        // Check if the snake is on the random tile
        foreach (Segment segment in segments)
        {
            // If the index is the same
            if (segment.Index == Tale.collisionIndices[0])
            {
                recursive = true;
                break;
            }
        }

        // Create a point
        if (!recursive)
        {
            // Enable object
            Tale.gameObject.SetActive(true);

            // Initialize it at the random position
            Tale.Initialize(Tale.transform.position, Tale.Index, TileType.Type.Tale);
        }
        // Try again
        else
            SpawnTale(segments);

    }
    public void DespawnTale()
    {
        if (Tale == null)
            return;

        // Remove point from the list
        talesGraveyard.Add(Tale);
        talesList.Remove(Tale);

        // Make point inactive
        Tale.gameObject.SetActive(false);

        // Turn the point null in order to respawn another
        Tale = null;

        // Only work towards next rare point if no rare points are present
        if (Stone == null)
            _talesCaptured++;

        // Refill list if empty to start over
        if (talesList.Count == 0)
        {
            talesList = talesGraveyard;
            talesGraveyard.Clear();
        }
    }
    
    public bool CanSpawnStone()
    {
        if (Stone == null)
        {
            // Only spawn rare point if captured 3 common points
            if (_talesCaptured >= 3)
            {
                _talesCaptured = 0;
                return true;
            }
        }
        return false;
    }
    public void SpawnRandomStone(List<Segment> segments)
    {
        bool lastPoint = _stonesCaptured == maxStones;

        if (!lastPoint)
        {
            int randomIndex = Random.Range(0, stonesList.Count);
            
            // Prevent spawning last stone (when its not the time)
            if (stonesList[randomIndex].collisionIndices[0] == new Vector2(5, 30))
                SpawnRandomStone(segments);

            Stone = stonesList[randomIndex];
        }
        // Spawns the last rarePoint
        else
            Stone = Instantiate(lastStonePrefab, stonesParent).GetComponent<Point>();
        
        if (Stone != null)
        {
            // Create a bool in case of the point being in the same index as the snake
            bool recursive = false;

            // Ignore if last pont
            if (!lastPoint)
            {
                // Check if the snake is on the random tile
                foreach (Segment segment in segments)
                {
                    for (int i = 0; i < Stone.collisionIndices.Length; i++)
                    {
                        // If the index is the same
                        if (segment.Index == Stone.collisionIndices[i] || Tale.collisionIndices[0] == Stone.collisionIndices[i])
                        {
                            recursive = true;
                            break;
                        }
                    }
                }
            }
            
            // Create a point
            if (!recursive)
            {
                // Enable object
                Stone.gameObject.SetActive(true);

                // Initialize it at the random position
                Stone.Initialize(Stone.transform.position, Stone.Index, TileType.Type.Stone);
                stoneValue = Stone.Value;
            }
            // Try again
            else
                SpawnRandomStone(segments);
        }
    }
    public void DespawnStone(bool captured)
    {
        if (Stone == null)
            return;

        // Remove point from the list
        stonesGraveyard.Add(Tale);
        stonesList.Remove(Tale);

        // Not last point
        if (_stonesCaptured < maxStones)
            Stone.gameObject.SetActive(false);
        // Last point
        else if (_stonesCaptured == maxStones)
        {
            Destroy(Stone.gameObject);

            // Prevent next stone to be the last if player misses
            if (!captured)
                maxStones += 2;
        }
        
        // Turn the point null in order to respawn another
        Stone = null;

        // Refill list if empty to start over
        if (stonesList.Count == 0)
        {
            stonesList = talesGraveyard;
            stonesGraveyard.Clear();
        }

        // Increment stones score if the point was captured
        if (captured)
            _stonesCaptured++;
    }
    public bool IsStoneTimeEnded()
    {
        if (Stone != null)
        {
            if (stoneValue > 0)
            {
                stoneValue -= stoneDevalueSpeed * Time.deltaTime;
                Stone.Value = (int)stoneValue;
                return false;
            }
            else
            {
                stoneValue = 0;
                Stone.Value = 0;
                return true;
            }
        }
        return false;
    }
    public bool MaxStonesCaptured()
    {
        return _stonesCaptured > maxStones;
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
            Tale = Instantiate(pointPrefab, transform);

            // Initialize it at the random position
            Tale.Initialize(randomTile.transform.position, randomTile.Index, TileType.Type.Tale);
        }
        // Try again
        else
            GenerateRandomPoint(tiles, segments);
    }
}
