using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snake : MonoBehaviour
{
    public Segment segmentPrefab;
    public List<Segment> Segments { get; private set; }

    [Header("Properties")]

    // Public variables
    public int initialSize = 3;
    public float snakeSpeed = 1;
    public float blinkSpeed = 20;
    public bool isAlive;

    // Private variables
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

    

    // Initialization
    public void Initialize()
    {
        // Set starting properties
        _headDirection = Vector2.right;
        _changedDir = false;
        isAlive = true;
        _timer = 0f;
    }
    public void SetMapLimits(Vector2 minPositions, Vector2 maxPositions, Vector2 minIndexes, Vector2 maxIndexes)
    {
        // Map limits
        _minPositions = minPositions;
        _maxPositions = maxPositions;
        _minIndexes = minIndexes;
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
            Vector2 position = new Vector2(spawnTile.transform.position.x - (i * segment.Size.x), spawnTile.transform.position.y);
            Vector2 index = new Vector2(spawnTile.Index.x - i, spawnTile.Index.y);

            // Initialize
            segment.Initialize(position, index, TileType.Type.Segment);
            
            // Add to a list
            Segments.Add(segment);
        }

        isAlive = true;
    }

    // Movement functions
    public void HandleMovement()
    {
        // Input
        HandleInput();

        if (_timer >= 1)
        {
            // Move each segment
            for (int i = Segments.Count - 1; i >= 0; i--)
            {
                // Get the segment
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

                    // Check collision
                    bool collisionDetected = CheckCollisions(segment.Index + segment.Direction);
                    if (collisionDetected)
                    {
                        // Change alive status
                        isAlive = false;

                        // Reset timer for blinking
                        _timer = 0;
                        break;
                    }
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
        Vector2 position = segment.transform.position + (Vector3)(segment.Direction * segment.Size);
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
            Vector2 index = new Vector2(_minIndexes.x, segment.Index.y);

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
            Vector2 index = new Vector2(segment.Index.x, _minIndexes.y);

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
    private bool CheckCollisions(Vector2 nextIndex)
    {
        for (int i = 1; i < Segments.Count; i++)
        {
            if (nextIndex == Segments[i].Index)
                return true;
        }
        return false;
    }

    public void Die(Action callback)
    {
        Blink(30f, callback);
    }
    private void Blink(float time, Action callback)
    {
        if (_timer < time)
        {
            // Increase timer
            _timer += blinkSpeed * Time.deltaTime;

            // Blink all the segments
            foreach (Segment segment in Segments)
            {
                // Change sprite visibility for each segment
                if ((int)_timer % 2 == 0)
                    segment.SpriteRenderer.enabled = false;
                else
                    segment.SpriteRenderer.enabled = true;
            }
        }
        else
        {
            // Stop timer
            _timer = time;

            // Reset and remove snake
            Despawn();

            // Calls the callback function
            callback();
        }
    }
    private void Despawn()
    {
        // Reset each snake segment
        foreach (Segment segment in Segments)
        {
            // Clear segment dictionary
            segment.Reset();

            // Destroy segment objects
            Destroy(segment.gameObject);
        }

        // Clear segments list
        Segments.Clear();
    }
}
