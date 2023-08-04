using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Point : Tile
{
    public int Value { get; private set; }



    protected override void Awake()
    {
        base.Awake();
    }



    public override void Initialize(Vector2 position, Vector2 index, TileType.Type tileType)
    {
        switch (tileType)
        {
            case TileType.Type.Point:
                Value = 10;
                break;

            case TileType.Type.BonusPoint:
                Value = 25;
                break;

            default:
                break;
        }
        
        // Initialize tile
        base.Initialize(position, index, tileType);
    }
}
