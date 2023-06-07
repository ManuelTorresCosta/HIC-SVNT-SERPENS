using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Segment : Tile
{
    public Vector2 Direction;
    public Dictionary<Vector2, Vector2> ChangeDirIndexes { get; private set; }



    private void Awake()
    {
        ChangeDirIndexes = new Dictionary<Vector2, Vector2>();
    }



    public void Initialize(Vector2 position, Vector2 index)
    {
        name = "Segment";

        SetPosition(position, index);
        Direction = Vector2.right;
    }
}
