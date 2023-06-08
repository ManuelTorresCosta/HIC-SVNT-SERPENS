using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Point : Tile
{
    public int value = 10;



    protected override void Awake()
    {
        base.Awake();
    }


    public override void Initialize(Vector2 position, Vector2 index, TileType.Type tileType)
    {
        base.Initialize(position, index, tileType);
    }
}
