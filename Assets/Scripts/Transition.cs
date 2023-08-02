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
                tile.Initialize(position, index, TileType.Type.Grid);

                // Add tile to the array
                Tiles[x, y] = tile;
            }
        }
    }
    public IEnumerator RunTransition()
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
                if (tile.Index != new Vector2(26, 15) && tile.Index != new Vector2(27, 15) && tile.Index != new Vector2(28, 15) &&
                    tile.Index != new Vector2(25, 16) && tile.Index != new Vector2(29, 16) && tile.Index != new Vector2(24, 17) &&
                    tile.Index != new Vector2(28, 15) && tile.Index != new Vector2(30, 17) && tile.Index != new Vector2(23, 18) &&
                    tile.Index != new Vector2(30, 18) && tile.Index != new Vector2(23, 19) && tile.Index != new Vector2(24, 20) &&
                    tile.Index != new Vector2(31, 20) && tile.Index != new Vector2(25, 21) && tile.Index != new Vector2(26, 21) &&
                    tile.Index != new Vector2(27, 21) && tile.Index != new Vector2(31, 21) && tile.Index != new Vector2(28, 22) &&
                    tile.Index != new Vector2(29, 23) && tile.Index != new Vector2(31, 22) && tile.Index != new Vector2(29, 33) &&
                    tile.Index != new Vector2(31, 23) && tile.Index != new Vector2(29, 24) && tile.Index != new Vector2(31, 24) &&
                    tile.Index != new Vector2(32, 25) && tile.Index != new Vector2(25, 26) && tile.Index != new Vector2(26, 26) &&
                    tile.Index != new Vector2(27, 26) && tile.Index != new Vector2(32, 26) && tile.Index != new Vector2(24, 27) &&
                    tile.Index != new Vector2(28, 27) && tile.Index != new Vector2(29, 27) && tile.Index != new Vector2(31, 27) &&
                    tile.Index != new Vector2(24, 28) && tile.Index != new Vector2(30, 28) && tile.Index != new Vector2(25, 29) &&
                    tile.Index != new Vector2(26, 29) && tile.Index != new Vector2(27, 30) && tile.Index != new Vector2(28, 30) &&
                    tile.Index != new Vector2(31, 19) && tile.Index != new Vector2(29, 31) && tile.Index != new Vector2(29, 32) &&
                    tile.Index != new Vector2(28, 33) && tile.Index != new Vector2(26, 34) && tile.Index != new Vector2(27, 34) &&
                    tile.Index != new Vector2(26, 35))
                {
                    // Turn renderer on
                    tile.SpriteRenderer.color = Color.black;
                    tile.SpriteRenderer.enabled = true;
                }

                yield return new WaitForEndOfFrame();
            }
        }
    }
}
