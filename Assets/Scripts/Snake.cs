using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using UnityEngine;

public class Snake : MonoBehaviour
{
    public Tile segment;
    public Sprite sprite;
    public List<Tile> Segments { get; private set; }

    // Private variables
    private Vector2 _segmentSize;
    private Vector2 _direction;
    private bool _changedDir;
    private float _timer;
    public float snakeSpeed = 1;

    private Vector2 _minPositions;
    private Vector2 _maxPositions;
    private Vector2 _minIndexes;
    private Vector2 _maxIndexes;



    private void Awake()
    {
        Segments = new List<Tile>();
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
                Tile segment = Segments[i];

                // Snake Body
                if (i != 0)
                {
                    // Save the tile index to change the direction
                    if (_changedDir && !segment.changeDir.ContainsKey(Segments[0].Index))
                    {
                        // Save direction of previous head position in the body segment dictionary
                        segment.changeDir.Add(Segments[0].Index, _direction);
                    }
                }
                // Snake head
                else
                {
                    // Change head direction to the new direction
                    segment.Direction = _direction;
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



    public void Initialize(Vector2 minPositions, Vector2 maxPositions, Vector2 maxIndexes)
    {
        // Get the size of a segment tile
        _segmentSize = sprite.rect.size / sprite.pixelsPerUnit;
        _segmentSize.x *= transform.lossyScale.x;
        _segmentSize.y *= transform.lossyScale.y;

        _direction = new Vector2(1, 0);
        _changedDir = false;

        _timer = 0f;

        _minPositions = minPositions;
        _maxPositions = maxPositions;
        _minIndexes = Vector2.zero;
        _maxIndexes = maxIndexes;
    }
    public void CreateSnake(int initialSize, Vector2 direction, Tile spawnTile)
    {
        _direction = direction;

        // Create snake segments
        for (int i = 0; i < initialSize; i++)
        {
            Vector2 position = new Vector2(spawnTile.transform.position.x - (i * _segmentSize.x), spawnTile.transform.position.y);

            Tile segment = Instantiate(this.segment, position, Quaternion.identity, transform);
            segment.Initialize((int)spawnTile.Index.x - i, (int)spawnTile.Index.y, TileType.Type.Segment);
            
            Segments.Add(segment);
        }
    }


    private void HandleInput()
    {
        // Right
        if (Input.GetKeyDown(KeyCode.RightArrow) && _direction != -Vector2.right)
        {

            _direction = Vector2.right;
            _changedDir = true;
        }

        // Left
        else if (Input.GetKeyDown(KeyCode.LeftArrow) && _direction != Vector2.right)
        {
            _direction = -Vector2.right;
            _changedDir = true;
        }

        // Up
        else if (Input.GetKeyDown(KeyCode.UpArrow) && _direction != -Vector2.up)
        {
            _direction = Vector2.up;
            _changedDir = true;
        }

        //Down
        else if (Input.GetKeyDown(KeyCode.DownArrow) && _direction != Vector2.up)
        {
            _direction = -Vector2.up;
            _changedDir = true;
        }
    }
    private void Move(Tile segment)
    {
        // Update position
        segment.transform.position += (Vector3)(segment.Direction * _segmentSize);

        // Update Index
        segment.Index += segment.Direction;
    }
    private void HandleGridLimits(Tile segment)
    {
        // Far left
        if (segment.Index.x < 0)
        {
            segment.Index.x = _maxIndexes.x - 1;
            segment.transform.position = new Vector3(_maxPositions.x, segment.transform.position.y, 0);
        }
        // Far
        else if (segment.Index.x > _maxIndexes.x - 1)
        {
            segment.Index.x = 0;
            segment.transform.position = new Vector3(_minPositions.x, segment.transform.position.y, 0);
        }
        if (segment.Index.y < 0)
        {
            segment.Index.y = _maxIndexes.y - 1;
            segment.transform.position = new Vector3(segment.transform.position.x, _maxPositions.y, 0);
        }
        else if (segment.Index.y > _maxIndexes.y - 1)
        {
            segment.Index.y = 0;
            segment.transform.position = new Vector3(segment.transform.position.x, _minPositions.y, 0);
        }

    }
    private void CheckSegmentChangeDir(Tile segment)
    {
        foreach (Vector2 index in segment.changeDir.Keys)
        {
            // Segment is on a 'change dir' index
            if (segment.Index == index)
            {
                // Apply change direction to the segment
                segment.Direction = segment.changeDir[index];

                // Remove item from the dictionary
                segment.changeDir.Remove(index);
                break;
            }
        }
    }
}
