using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snake : MonoBehaviour
{
    public List<Segment> Segments { get; private set; }
    public Segment GetHead() { return Segments[0]; }

    [Header("References")]

    public Segment segmentPrefab;
    public Sprite[] sprites;
    public Sprite[] headSprites;

    [Header("Properties")]

    public int initialSize = 3;
    public float snakeSpeed = 1;
    public bool eating = false;
    public bool hasCollided = false;
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

    private List<Vector2> _eatenIndices;



    // Unity functions
    private void Awake()
    {
        Segments = new List<Segment>();

        _eatenIndices = new List<Vector2>();
    }



    // Initialization
    public void Initialize()
    {
        // Set starting properties
        //_headDirection = Vector2.right;

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

        //// Set the sprites according to the snake composition
        //SetSprites();

        isAlive = true;
    }

    // Movement functions
    private void HandleInput(float inputX, float inputY)
    {
        // Get head direction reference
        Vector2 currHeadDir = GetHead().Direction;

        // Right
        if ((Input.GetKeyDown(KeyCode.RightArrow) || inputX > 0) && currHeadDir != -Vector2.right)
        {
            if (currHeadDir == Vector2.right)
                return;

            _headDirection = Vector2.right;
            _changedDir = true;
        }

        // Left
        else if ((Input.GetKeyDown(KeyCode.LeftArrow) || inputX < 0) && currHeadDir != Vector2.right)
        {
            if (currHeadDir == -Vector2.right)
                return;

            _headDirection = -Vector2.right;
            _changedDir = true;
        }

        // Up
        else if ((Input.GetKeyDown(KeyCode.UpArrow) || inputY > 0) && currHeadDir != -Vector2.up)
        {
            if (currHeadDir == Vector2.up)
                return;

            _headDirection = Vector2.up;
            _changedDir = true;
        }

        //Down
        else if ((Input.GetKeyDown(KeyCode.DownArrow) || inputY < 0) && currHeadDir != Vector2.up)
        {
            if (currHeadDir == -Vector2.up)
                return;

            _headDirection = -Vector2.up;
            _changedDir = true;
        }
    }
    public void HandleMovement(float inpuX, float inputY)
    {
        // Input
        HandleInput(inpuX, inputY);

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

                if (!CheckSelfCollision())
                {
                    // Move segments
                    Move(segment);

                    // Handle the grid limits
                    HandleGridLimits(segment);

                    // Updated segment direction when on the correct tile index
                    UpdateSegmentDirection(segment);

                    // Set the sprites
                    SetSprite(segment, i);

                    // Set rotation
                    SetSpriteRotation(segment, i);
                }
                else
                    hasCollided = true;
            }

            // Reset timer
            _movementTimer = 0;
            _changedDir = false;
        }
        else
            _movementTimer += snakeSpeed * Time.deltaTime;
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
    private void UpdateSegmentDirection(Segment segment)
    {
        segment.changeDir = false;
        foreach (Vector2 index in segment.ChangeDirIndexes.Keys)
        {
            // Segment is on a 'change dir' index
            if (segment.Index == index)
            {
                segment.changeDir = true;

                Vector2 currDir = segment.Direction;
                Vector2 newDir = segment.ChangeDirIndexes[index];
                if (newDir == Vector2.up)
                {
                    if (currDir == -Vector2.right)
                        segment.clockwise = true;
                    else if (currDir == Vector2.right)
                        segment.clockwise = false;
                }
                else if (newDir == -Vector2.up)
                {
                    if (currDir == Vector2.right)
                        segment.clockwise = true;
                    else if (currDir == -Vector2.right)
                        segment.clockwise = false;
                }
                else if (newDir == Vector2.right)
                {
                    if (currDir == Vector2.up)
                        segment.clockwise = true;
                    else if (currDir == -Vector2.up)
                        segment.clockwise = false;
                }
                else if (newDir == -Vector2.right)
                {
                    if (currDir == -Vector2.up)
                        segment.clockwise = true;
                    else if (currDir == Vector2.up)
                        segment.clockwise = false;
                }

                // Apply change direction to the segment
                segment.Direction = segment.ChangeDirIndexes[index];

                // Remove item from the dictionary
                segment.ChangeDirIndexes.Remove(index);
                break;
            }
        }
    }

    // Sprites
    private void SetSprite(Segment segment, int i)
    {
        // Head
        if (i == 0)
            segment.SetBodySprite(!eating ? headSprites[0] : headSprites[1]);
        else
        {
            // Tail
            if (i == Segments.Count - 1)
                segment.SetBodySprite(sprites[3]);
            // Normal body
            else if (!segment.changeDir)
                segment.SetBodySprite(sprites[0]);
            // Body corner
            else
                segment.SetBodySprite(sprites[2], 1, segment.clockwise ? 1 : -1);
            

            // Override sprites when passing through an eaten spot
            foreach (Vector2 eatenIndex in _eatenIndices)
            {
                if (segment.Index == eatenIndex)
                {
                    // Fat body
                    if (i != Segments.Count - 1)
                        segment.SetBodySprite(sprites[1]);
                    // Remove index from list when it reaches the tail
                    else
                    {
                        _eatenIndices.RemoveAt(0);
                        break;
                    }
                }

            }
        }
    }
    private void SetSpriteRotation(Segment segment, int i)
    {
        // Update the rotation
        float rotation = segment.GetRotationFromDirection();
        segment.transform.rotation = Quaternion.Euler(0, 0, rotation);

        // Flip head when direction is left (Head only)
        if (i == 0)
        {
            bool flipY = segment.Direction.x == -1;
            segment.SpriteRenderer.flipY = flipY;
        }
    }

    // Collision functions
    public bool CheckSelfCollision()
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

        return false;
    }
    public bool CheckCollisionWith(Point point)
    {
        if (point == null)
        {
            eating = false;
            return false;
        }

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
                eating = true;
                _eatenIndices.Add(point.collisionIndices[i]);
                return true;
            }
        }

        eating = false;
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
        newSegment.transform.rotation = Quaternion.Euler(0, 0, tail.GetRotationFromDirection());

        // Copy all saved 'change directions' dictionary references
        newSegment.InheritChangeDirectionIndices(tail);

        // Add segment to the list
        Segments.Add(newSegment);
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
