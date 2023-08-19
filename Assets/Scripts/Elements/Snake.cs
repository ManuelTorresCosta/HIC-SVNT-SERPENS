using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snake : MonoBehaviour
{
    public List<Segment> Segments { get; private set; }
    public Segment GetHead() { return Segments[0]; }
    
    private List<Vector2> _eatenIndexes;

    [Header("References")]    
    
    public Segment segmentPrefab;
    public Sprite[] sprites;
    public Sprite[] headSprites;

    [Header("Properties")]

    public int initialSize = 3;
    public float snakeSpeed = 1;
    public bool isAlive;

    // Private variables
    private Vector2 _headDirection;
    private bool _changedDir;
    private float _movementTimer;
    private float _blinkTimer;
    private float _blinkSpeed;

    private Vector2 _minPositions;
    private Vector2 _maxPositions;
    private Vector2 _minIndexes;
    private Vector2 _maxIndexes;



    private void Awake()
    {
        Segments = new List<Segment>();
        _eatenIndexes = new List<Vector2>();
    }

    

    // Initialization
    public void Initialize()
    {
        // Set starting properties
        _headDirection = Vector2.right;

        isAlive = true;
        _changedDir = false;

        _movementTimer = 0f;
        _blinkTimer = 0f;
        _blinkSpeed = 15;
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
            Vector2 tileIndex = new Vector2(spawnTile.Index.x - i, spawnTile.Index.y);

            // Initialize
            segment.Initialize(position, tileIndex, TileType.Type.Segment);
            segment.i = i;

            // Add to a list
            Segments.Add(segment);
        }

        // Set the sprites according to the snake composition
        UpdateSprites();

        isAlive = true;
    }
    private void UpdateSprites(bool eating = false)
    {
        // Go though all the segments
        for (int i = 0; i < Segments.Count; i++)
        {
            // Get the segment from list
            Segment segment = Segments[i];  

            // Head
            if (i == 0)
                segment.SetBodySprite(!eating ? headSprites[0] : headSprites[1]);              
            // The first body segment
            if (i == 1)
                segment.SetBodySprite(sprites[0]);
            // The last segment
            else if (i == Segments.Count - 1)
                segment.SetBodySprite(sprites[1]);
            // The second and before last segments
            else if (i == 2)
                segment.SetBodySprite(sprites[2]);
            else if (i == Segments.Count - 2)
                segment.SetBodySprite(sprites[2], -1, -1);
            // The rest of the body alternates
            else
                segment.SetBodySprite(i % 2 != 0 ? sprites[0] : sprites[1]);

            // Set fat snake (when snake eats points)
            //for (int j = 0; j < _eatenIndexes.Count; j++)
            //{
            //    // Dont change head
            //    if (i == 0)
            //        continue;

            //    // Change sprite when the body passes in the eaten point
            //    if (segment.Index == _eatenIndexes[j])
            //    {
            //        segment.SetBodySprite(sprites[3]);

            //        // Remove eaten point when it reaches the tail
            //        if (i == Segments.Count - 1)
            //            _eatenIndexes.Remove(segment.Index);
            //    }
            //}

            segment.UpdateSpriteDirection();
        }
    }

    // Movement functions
    public void HandleMovement()
    {
        // Input
        HandleInput();

        if (_movementTimer >= 1)
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
                }

                // Move segments
                Move(segment);

                // Handle the grid limits
                HandleGridLimits(segment);

                // Updated segment direction when on the correct tile index
                CheckSegmentChangeDir(segment);
            }

            // Reset timer
            _movementTimer = 0;
            _changedDir = false;
        }
        else
            _movementTimer += snakeSpeed * Time.deltaTime;
    }
    private void HandleInput()
    {
        // Right
        if (Input.GetKeyDown(KeyCode.RightArrow) && GetHead().Direction != -Vector2.right)
        {
            _headDirection = Vector2.right;
            _changedDir = true;
        }

        // Left
        else if (Input.GetKeyDown(KeyCode.LeftArrow) && GetHead().Direction != Vector2.right)
        {
            _headDirection = -Vector2.right;
            _changedDir = true;
        }

        // Up
        else if (Input.GetKeyDown(KeyCode.UpArrow) && GetHead().Direction != -Vector2.up)
        {
            _headDirection = Vector2.up;
            _changedDir = true;
        }

        //Down
        else if (Input.GetKeyDown(KeyCode.DownArrow) && GetHead().Direction != Vector2.up)
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
        segment.UpdateSpriteDirection();
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

    // Collision functions
    public bool CheckSelfCollision()
    {
        if (_movementTimer >= 1)
        {
            // Get the next position of the head
            Vector2 nextHeadPos = GetHead().Index + _headDirection;

            // Go through all the segments (excluding the head)
            for (int i = 1; i < Segments.Count; i++)
            {
                // If the next position collides with the body
                if (nextHeadPos == Segments[i].Index)
                    return true;
            }
        }

        return false;
    }
    public bool CheckCollisionWith(Point point)
    {
        if (point == null)
            return false;

        Segment head = Segments[0];

        // Check for all collision index of the point
        for (int i = 0; i < point.collisionIndices.Length; i++)
        {
            // Animate head when eating
            if (head.Index + 1 * head.Direction == point.collisionIndices[i] || head.Index == point.collisionIndices[i])
                head.SetBodySprite(headSprites[1]);
            else
                head.SetBodySprite(headSprites[0]);

            // Check if head index is the same as the point
            if (head.Index == point.collisionIndices[i])
            {
                _eatenIndexes.Add(head.Index);
                return true;
            }
        }

        return false;
    }
    public void Grow()
    {
        // Create a new segment
        Segment newSegment = Instantiate(segmentPrefab, transform);

        // Set the position at the end of the body
        Segment tail = Segments[Segments.Count - 1];
        Vector2 position = (Vector2)tail.transform.position + (newSegment.Size * -tail.Direction);
        Vector2 index = tail.Index + (-tail.Direction);

        // Initialize it
        newSegment.Initialize(position, index, TileType.Type.Segment);
        newSegment.Direction = tail.Direction;

        // Copy all saved 'change directions' dictionary references
        newSegment.InheritTailDirections(tail);

        // Add segment to the list
        Segments.Add(newSegment);

        // Update the tail sprites
        UpdateSprites(true);
    }

    // Despawn functions
    public void Die(Action despawn)
    {
        if (_blinkTimer < 30f)
        {
            // Increase timer
            _blinkTimer += _blinkSpeed * Time.deltaTime;

            // Blink all the segments
            foreach (Segment segment in Segments)
            {
                // Change sprite visibility for each segment
                if ((int)_blinkTimer % 2 == 0)
                    segment.SpriteRenderer.enabled = false;
                else
                    segment.SpriteRenderer.enabled = true;
            }
        }
        else
        {
            despawn();
        }
    }
    public void Despawn()
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
