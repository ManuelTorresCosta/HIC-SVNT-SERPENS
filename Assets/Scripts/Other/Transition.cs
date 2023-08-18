using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.Image;

public class Transition : MonoBehaviour
{
    public BoxCollider2D Collider { get; private set; }

    public Tile tilePrefab;
    public Tile[,] Tiles { get; private set; }
    
    private Sprite _tileSprite;
    private Vector2 _arraySize;
    private int _x, _y;
    private int dir = 1;

    // Debug (non coroutine function)
    public float timer = 0;
    public float maxTimer = 100;
    public int speed = 10;
    public int x = 0;
    public int y = 0;


    // Unity funtions
    private void Awake()
    {
        Collider = GetComponentInChildren<BoxCollider2D>();

        _tileSprite = tilePrefab.GetComponent<SpriteRenderer>().sprite;
    }
    private void Start()
    {
        CreateGrid();
    }



    // Functions
    private void CreateGrid()
    {
        Vector2 gridSize = Collider.size;
        Vector2 tileSize = (_tileSprite.rect.size / _tileSprite.pixelsPerUnit) * tilePrefab.transform.lossyScale;
        _arraySize = new Vector2((int)(gridSize.x / tileSize.x), (int)(gridSize.y / tileSize.y));
        Tiles = new Tile[(int)_arraySize.x, (int)_arraySize.y];

        Bounds bounds = Collider.bounds;
        Vector2 origin = new Vector2(bounds.min.x, bounds.min.y);
        Vector2 offset = tileSize / 2;

        for (int y = 0; y < (int)_arraySize.y; y++)
        {
            for (int x = 0; x < (int)_arraySize.x; x++)
            {
                // Instantiate the tile
                Tile tile = Instantiate(tilePrefab, Collider.transform);

                // Calculate position
                Vector2 position = origin + offset + new Vector2(x * tile.Size.x, y * tile.Size.y);
                Vector2 index = new Vector2(x, y);

                // Initialize tile
                tile.Initialize(position, index, TileType.Type.Overlay);

                // Add tile to the array
                Tiles[x, y] = tile;
            }
        }
    }

    public IEnumerator RunTransition(Action callback)
    {
        for (int y = 0; y < (int)_arraySize.y; y++)
        {
            _y = y;
            for (int x = 0; x < (int)_arraySize.x; x++)
            {
                // X goes backwards if y is odd number
                if (_y % 2 == 0)
                    _x = x;
                else
                    _x = (int)_arraySize.x - 1 - x;

                // get the tile
                Tile tile = Tiles[_x, _y];

                // Turn on renderer except this indexes
                if (tile.Index != new Vector2(20, 15) && tile.Index != new Vector2(21, 15) && tile.Index != new Vector2(22, 15) &&
                    tile.Index != new Vector2(19, 16) && tile.Index != new Vector2(23, 16) && tile.Index != new Vector2(18, 17) &&
                    tile.Index != new Vector2(22, 15) && tile.Index != new Vector2(24, 17) && tile.Index != new Vector2(17, 18) &&
                    tile.Index != new Vector2(24, 18) && tile.Index != new Vector2(17, 19) && tile.Index != new Vector2(18, 20) &&
                    tile.Index != new Vector2(25, 20) && tile.Index != new Vector2(19, 21) && tile.Index != new Vector2(20, 21) &&
                    tile.Index != new Vector2(21, 21) && tile.Index != new Vector2(25, 21) && tile.Index != new Vector2(22, 22) &&
                    tile.Index != new Vector2(23, 23) && tile.Index != new Vector2(25, 22) && tile.Index != new Vector2(23, 33) &&
                    tile.Index != new Vector2(25, 23) && tile.Index != new Vector2(23, 24) && tile.Index != new Vector2(25, 24) &&
                    tile.Index != new Vector2(26, 25) && tile.Index != new Vector2(19, 26) && tile.Index != new Vector2(20, 26) &&
                    tile.Index != new Vector2(21, 26) && tile.Index != new Vector2(26, 26) && tile.Index != new Vector2(18, 27) &&
                    tile.Index != new Vector2(22, 27) && tile.Index != new Vector2(23, 27) && tile.Index != new Vector2(25, 27) &&
                    tile.Index != new Vector2(18, 28) && tile.Index != new Vector2(24, 28) && tile.Index != new Vector2(19, 29) &&
                    tile.Index != new Vector2(20, 29) && tile.Index != new Vector2(21, 30) && tile.Index != new Vector2(22, 30) &&
                    tile.Index != new Vector2(25, 19) && tile.Index != new Vector2(23, 31) && tile.Index != new Vector2(23, 32) &&
                    tile.Index != new Vector2(22, 33) && tile.Index != new Vector2(19, 34) && tile.Index != new Vector2(20, 34) && 
                    tile.Index != new Vector2(21, 34) && tile.Index != new Vector2(19, 35) && tile.Index != new Vector2(20, 35))
                {
                    // Turn renderer on
                    tile.SpriteRenderer.enabled = true;
                }

                yield return new WaitForEndOfFrame();
            }
        }

        yield return new WaitForSeconds(3f);

        callback();
    }
}
