using UnityEngine;

public class GridManager : MonoBehaviour
{
    // Protected references
    public BoxCollider2D Collider { get; private set; }
    public Tile[,] Tiles { get; private set; }
    public Tile SpawnTile { get; private set; }

    // Public variables
    public Transform gridParent;
    public Transform borderParent;
    public Tile tilePrefab;
    public Vector2 arraySize;
    public Vector2 minPositions;
    public Vector2 maxPositions;

    // Private variables
    private Sprite _tileSprite;
    private Vector2 _tileSize;




    private void Awake()
    {
        Collider = GetComponentInChildren<BoxCollider2D>();
        _tileSprite = tilePrefab.GetComponent<SpriteRenderer>().sprite;
    }



    public void CreateMap()
    {
        // Get the bounds of the boxcollider2D
        Bounds bounds = Collider.bounds;

        // Get the size of the BoxCollider2D
        Vector2 gridSize = Collider.size;

        // Get the size of a tile
        _tileSize = (_tileSprite.rect.size / _tileSprite.pixelsPerUnit) * tilePrefab.transform.lossyScale;

        // Origin of the grid
        Vector2 origin = new Vector2(bounds.min.x, bounds.min.y);

        // Offset to apply to tiles spawn
        Vector2 offset = _tileSize / 2;

        //  Calculate array size
        arraySize = new Vector2((int)(gridSize.x / _tileSize.x), (int)(gridSize.y / _tileSize.y));
        Tiles = new Tile[(int)arraySize.x, (int)arraySize.y];

        // Create the grid
        CreateGrid(origin, offset);

        // Save min and max positions
        minPositions = Tiles[0, 0].transform.position;
        maxPositions = Tiles[(int)arraySize.x - 1, (int)arraySize.y - 1].transform.position;

        // Set the spawn tile
        Vector2 spawnIndex = new Vector2((int)arraySize.x / 2, (int)arraySize.y / 2);
        SpawnTile = Tiles[(int)spawnIndex.x, (int)spawnIndex.y];

        // -----------------------------------------------------------------------------------------

        // Create borders around the grid
        origin = new Vector2(bounds.min.x - _tileSize.x, bounds.min.y - _tileSize.y);

        // Create border
        CreateBorders(origin, offset);
    }
    private void CreateGrid(Vector2 origin, Vector2 offset)
    {
        for (int y = 0; y < (int)arraySize.y; y++)
        {
            for (int x = 0; x < (int)arraySize.x; x++)
            {
                // Set the position
                Vector2 position = origin + offset + new Vector2(x * _tileSize.x, y * _tileSize.y);

                // Instantiate the tile
                Tile tile = Instantiate(tilePrefab, position, Quaternion.identity, gridParent);
                tile.Initialize(x, y, TileType.Type.Grid);

                // Add tile to the array
                Tiles[x, y] = tile;
            }
        }
    }
    private void CreateBorders(Vector2 origin, Vector2 offset)
    {
        for (int y = 0; y < (int)arraySize.y + 2; y++)
        {
            for (int x = 0; x < (int)arraySize.x + 2; x++)
            {
                // Only instantiate tiles on the outside of the grid
                if (x == 0 || y == 0 || x == (int)arraySize.x + 1 || y == (int)arraySize.y + 1)
                {
                    //  Set the position
                    Vector2 position = origin + offset + new Vector2(x * _tileSize.x, y * _tileSize.y);

                    // Instantiate tile border
                    Tile tile = Instantiate(tilePrefab, position, Quaternion.identity, borderParent);
                    tile.Initialize(x, y, TileType.Type.Border);
                }
            }
        }
    }

    public Vector2 GetTileSize()
    { 
        return _tileSize; 
    }
}
