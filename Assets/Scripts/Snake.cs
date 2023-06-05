using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snake : MonoBehaviour
{
    public Tile segment;
    public Sprite sprite;
    public List<Tile> Segments { get; private set; }

    // Private variables
    private int initialSize = 4;
    private Vector2 _segmentSize;



    private void Awake()
    {
        Segments = new List<Tile>();

        // Get the size of a tile
        Vector2 local_sprite_size = sprite.rect.size / sprite.pixelsPerUnit;
        _segmentSize = local_sprite_size;
        _segmentSize.x *= transform.lossyScale.x;
        _segmentSize.y *= transform.lossyScale.y;
    }



    public void CreateSnake(Tile spawnTile)
    {
        for (int i = 0; i < initialSize; i++)
        {
            Vector2 position = new Vector2(spawnTile.transform.position.x - (i * _segmentSize.x), spawnTile.transform.position.y);

            Tile snake = Instantiate(segment, position, Quaternion.identity, transform);
            snake.Initialize((int)spawnTile.Index.x, (int)spawnTile.Index.y, TileType.Type.Segment);
        }
    }
}
