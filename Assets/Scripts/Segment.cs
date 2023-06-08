using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Segment : Tile
{
    public Vector2 Direction;
    public Dictionary<Vector2, Vector2> ChangeDirIndexes { get; private set; }



    protected override void Awake()
    {
        base.Awake();

        ChangeDirIndexes = new Dictionary<Vector2, Vector2>();
    }



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
}
