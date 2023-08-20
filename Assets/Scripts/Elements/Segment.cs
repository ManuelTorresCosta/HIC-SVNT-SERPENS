using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Segment : Tile
{
    public Vector2 Direction;
    public int i;

    public Dictionary<Vector2, Vector2> ChangeDirIndexes { get; private set; }
    private float GetRotationFromDirection()
    {
        if (Direction == Vector2.right)
            return 0;
        else if (Direction == Vector2.up)
            return 90;
        else if (Direction == Vector2.left)
            return 180;
        else
            return -90;
    }

    
    // Unity functions
    protected override void Awake()
    {
        base.Awake();

        i = 0;
        ChangeDirIndexes = new Dictionary<Vector2, Vector2>();
    }


    // Functions
    public override void Initialize(Vector2 position, Vector2 index, TileType.Type tileType)
    {
        // Initialize from parent
        base.Initialize(position, index, tileType);

        // Set direction
        Direction = Vector2.right;
    }
    public void Reset()
    {
        ChangeDirIndexes.Clear();
    }


    public void InheritTailDirections(Segment tail)
    {
        foreach (Vector2 index in tail.ChangeDirIndexes.Keys)
        {
            ChangeDirIndexes.Add(index, tail.ChangeDirIndexes[index]);
        }
    }
    public void SetBodySprite(Sprite sprite, int flipX = 1, int flipY = 1)
    {
        // If the sprite needs to be updated
        if (SpriteRenderer.sprite != sprite)
            SpriteRenderer.sprite = sprite;

        // Flip sprite for tail
        transform.localScale = new Vector3(flipX, flipY, 1);
    }
    public void UpdateSpriteDirection(int x = 1, int y = 1) 
    {
        // Update the rotation
        float rotation = GetRotationFromDirection();
        transform.rotation = Quaternion.Euler(0, 0, rotation);

        // Flip head when direction is left (Head only)
        if (i == 0)
        {
            bool flipY = Direction.x == -1;
            SpriteRenderer.flipY = flipY;
        }
    }
}
