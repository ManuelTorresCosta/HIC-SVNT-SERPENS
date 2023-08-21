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
                SpriteRenderer.sortingOrder = 0;
                SpriteRenderer.enabled = false;
                name = "Tile (" + index.x + ", " + index.y + ")";
                break;

            case TileType.Type.Border:
                SpriteRenderer.sortingOrder = 1;
                SpriteRenderer.color = Color.black;
                name = "Border " + transform.parent.childCount;                
                break;

            case TileType.Type.CommonPoint:
                SpriteRenderer.sortingOrder = 2;
                SpriteRenderer.color = Color.black;
                //name = "Point";
                break;

            case TileType.Type.RarePoint:
                SpriteRenderer.sortingOrder = 2;
                SpriteRenderer.color = Color.black;
                //name = "BonusPoint";
                break;

            case TileType.Type.Segment:
                SpriteRenderer.sortingOrder = 3;
                SpriteRenderer.color = Color.black;
                name = "Segment " + transform.parent.childCount;
                break;

            case TileType.Type.GameOver:
                SpriteRenderer.sortingOrder = 5;
                SpriteRenderer.color = Color.black;
                SpriteRenderer.sortingOrder = 25;
                SpriteRenderer.enabled = false;
                name = "GameOver " + transform.parent.childCount;
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
