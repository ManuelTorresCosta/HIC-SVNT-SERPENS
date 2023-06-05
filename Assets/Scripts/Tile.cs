using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public SpriteRenderer SpriteRenderer { get; private set; }
    public TileType TileType { get; private set; }
    public Vector2 Size { get; private set; }
    public Vector2 Index { get; set; }


    private void Awake()
    {
        SpriteRenderer = GetComponent<SpriteRenderer>();

        // Get Size of sprite
        CalculateSize();
    }


    public void Initialize(int x, int y, TileType.Type tileType)
    {
        switch (tileType)
        {
            case TileType.Type.Grid:
                name = "Tile (" + x + ", " + y + ")";

                break;

            case TileType.Type.Border:
                name = "Border";

                // Turn sprite black
                SpriteRenderer.color = Color.black;

                break;

            case TileType.Type.Segment:
                name = "Snake";

                SpriteRenderer.color = Color.black;
                break;
        }

        Index = new Vector2(x, y);
    }

    private void CalculateSize()
    {
        Vector2 local_sprite_size = SpriteRenderer.sprite.rect.size / SpriteRenderer.sprite.pixelsPerUnit;
        Vector3 world_size = local_sprite_size;
        world_size.x *= transform.lossyScale.x;
        world_size.y *= transform.lossyScale.y;

        Size = world_size;
    }

}
