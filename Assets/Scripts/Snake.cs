using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using UnityEngine;

public class Snake : MonoBehaviour
{
    public Segment segmentPrefab;
    public List<Segment> Segments { get; private set; }

    // Public variables
    public int initialSize = 3;
    public float snakeSpeed = 1;

    // Private variables
    private Vector2 _segmentSize;
    private Vector2 _headDirection;
    private bool _changedDir;
    private float _timer;

    private Vector2 _minPositions;
    private Vector2 _maxPositions;
    private Vector2 _minIndexes;
    private Vector2 _maxIndexes;



    private void Awake()
    {
        Segments = new List<Segment>();
    }
    private void Update()
    {
        // Input
        HandleInput();
        
        if (_timer >= 1)
        {
            // Move each segment
            for (int i = Segments.Count - 1; i >= 0; i--)
            {
                Segment segment = Segments[i];

                // Snake Body
                if (i != 0)
                {
                    // Save the tile index to change the direction
                    if (_changedDir && !segment.ChangeDirIndexes.ContainsKey(Segments[0].Index))
                    {
                        // Save direction of previous head position in the body segment dictionary
                        segment.ChangeDirIndexes.Add(Segments[0].Index, _headDirection);
                    }
                }
                // Snake head
                else
                {
                    // Change head direction to the new direction
                    segment.Direction = _headDirection;
                }

                // Move segments
                Move(segment);
                
                // Handle the grid limits
                HandleGridLimits(segment);

                // Updated segment direction when on the correct tile index
                CheckSegmentChangeDir(segment);
            }

            // Reset timer
            _timer = 0;
            _changedDir = false;
        }
        else
            _timer += snakeSpeed * Time.deltaTime;
    }



    public void Initialize(Vector2 tileSize, Vector2 minPositions, Vector2 maxPositions, Vector2 maxIndexes)
    {
        // Segment properties
        _segmentSize = tileSize;
        _headDirection = new Vector2(1, 0);
        _changedDir = false;
        _timer = 0f;

        // Map limits
        _minPositions = minPositions;
        _maxPositions = maxPositions;
        _minIndexes = Vector2.zero;
        _maxIndexes = maxIndexes;
    }
    public void CreateSnake(Vector2 direction, Tile spawnTile)
    {
        _headDirection = direction;

        // Create snake segments
        for (int i = 0; i < initialSize; i++)
        {
            // Instantiate object
            Segment segment = Instantiate(segmentPrefab, transform);

            // Calculate position
            Vector2 position = new Vector2(spawnTile.transform.position.x - (i * _segmentSize.x), spawnTile.transform.position.y);
            Vector2 index = new Vector2(spawnTile.Index.x - i, spawnTile.Index.y);

            // Initialize
            segment.Initialize(position, index);
            
            // Add to a list
            Segments.Add(segment);
        }
    }


    private void HandleInput()
    {
        // Right
        if (Input.GetKeyDown(KeyCode.RightArrow) && _headDirection != -Vector2.right)
        {
            _headDirection = Vector2.right;
            _changedDir = true;
        }

        // Left
        else if (Input.GetKeyDown(KeyCode.LeftArrow) && _headDirection != Vector2.right)
        {
            _headDirection = -Vector2.right;
            _changedDir = true;
        }

        // Up
        else if (Input.GetKeyDown(KeyCode.UpArrow) && _headDirection != -Vector2.up)
        {
            _headDirection = Vector2.up;
            _changedDir = true;
        }

        //Down
        else if (Input.GetKeyDown(KeyCode.DownArrow) && _headDirection != Vector2.up)
        {
            _headDirection = -Vector2.up;
            _changedDir = true;
        }
    }
    private void Move(Segment segment)
    {
        Vector2 position = segment.transform.position + (Vector3)(segment.Direction * _segmentSize);
        Vector2 index = segment.Index + segment.Direction;

        segment.SetPosition(position, index);
    }
    private void HandleGridLimits(Segment segment)
    {
        // Far left
        if (segment.Index.x < 0)
        {
            Vector2 position = new Vector2(_maxPositions.x, segment.transform.position.y);
            Vector2 index = new Vector2(_maxIndexes.x - 1, segment.Index.y);

            segment.SetPosition(position, index);
        }
        // Far
        else if (segment.Index.x > _maxIndexes.x - 1)
        {
            Vector2 position = new Vector2(_minPositions.x, segment.transform.position.y);
            Vector2 index = new Vector2(0, segment.Index.y);

            segment.SetPosition(position, index);
        }
        if (segment.Index.y < 0)
        {
            Vector2 position = new Vector2(segment.transform.position.x, _maxPositions.y);
            Vector2 index = new Vector2(segment.Index.x, _maxIndexes.y - 1);

            segment.SetPosition(position, index);
        }
        else if (segment.Index.y > _maxIndexes.y - 1)
        {
            Vector2 position = new Vector2(segment.transform.position.x, _minPositions.y);
            Vector2 index = new Vector2(segment.Index.x, 0);

            segment.SetPosition(position, index);
        }

    }
    private void CheckSegmentChangeDir(Segment segment)
    {
        foreach (Vector2 index in segment.ChangeDirIndexes.Keys)
        {
            // Segment is on a 'change dir' index
            if (segment.Index == index)
            {
                // Apply change direction to the segment
                segment.Direction = segment.ChangeDirIndexes[index];

                // Remove item from the dictionary
                segment.ChangeDirIndexes.Remove(index);
                break;
            }
        }
    }
}
