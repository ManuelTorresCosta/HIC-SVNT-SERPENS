using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public SpriteRenderer SpriteRenderer { get; private set; }
    public Vector2 Index { get; private set; }
    public Vector2 Size { get; private set; }

    public TileType TileType { get; private set; }



    private void Awake()
    {
        SpriteRenderer = GetComponent<SpriteRenderer>();

        // Get Size of sprite
        Size = (SpriteRenderer.sprite.rect.size / SpriteRenderer.sprite.pixelsPerUnit) * transform.lossyScale;
    }

    

    public void Initialize(int x, int y, TileType.Type tileType)
    {
        Index = new Vector2(x, y);

        switch (tileType)
        {
            case TileType.Type.Grid:
                SpriteRenderer.color = new Color(1, 1, 1, 0);
                name = "Tile (" + x + ", " + y + ")";
                break;

            case TileType.Type.Border:
                SpriteRenderer.color = Color.black;
                name = "Border";                
                break;
        }
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
