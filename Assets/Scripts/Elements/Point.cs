using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Point : Tile
{
    public int Value { get; set; }
    public Vector2[] collisionIndices;


    protected override void Awake()
    {
        base.Awake();
    }



    public override void Initialize(Vector2 position, Vector2 index, TileType.Type tileType)
    {
        switch (tileType)
        {
            case TileType.Type.CommonPoint:
                Value = 10;
                break;

            case TileType.Type.RarePoint:
                Value = 99;
                break;

            default:
                break;
        }
        
        // Initialize tile
        base.Initialize(position, index, tileType);
    }
}
