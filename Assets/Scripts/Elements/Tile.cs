using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public SpriteRenderer SpriteRenderer { get; private set; }
    public Vector2 Index { get; private set; }
    public Vector2 Size { get; private set; }

    public TileType TileType { get; private set; }



    protected virtual void Awake()
    {
        SpriteRenderer = GetComponent<SpriteRenderer>();

        // Get Size of sprite
        Size = (SpriteRenderer.sprite.rect.size / SpriteRenderer.sprite.pixelsPerUnit) * transform.lossyScale;
    }

    

    public virtual void Initialize(Vector2 position, Vector2 index, TileType.Type tileType)
    {
        switch (tileType)
        {
            case TileType.Type.Grid:
                SpriteRenderer.enabled = false;
                name = "Tile (" + index.x + ", " + index.y + ")";
                break;

            case TileType.Type.Border:
                SpriteRenderer.color = Color.black;
                name = "Border";                
                break;

            case TileType.Type.Segment:
                SpriteRenderer.color = Color.black;
                name = "Segment";
                break;

            case TileType.Type.Point:
                SpriteRenderer.color = Color.black;
                name = "Point";
                break;
        }

        SetPosition(position, index);
    }


    public void SetPosition(Tile tile)
    {
        transform.position = tile.transform.position;
        Index = tile.Index;
    }
    public void SetPosition(Vector2 position, Vector2 index)
    {
        transform.position = position;
        Index = index;
    }
}
