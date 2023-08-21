using UnityEngine;

public class TileManager : MonoBehaviour
{
    // Protected references
    public BoxCollider2D Collider { get; private set; }
    public Tile[,] List { get; private set; }
    public Tile Spawn { get; private set; }

    // Public variables
    public Transform gridParent;
    public Transform borderParent;
    public Tile tilePrefab;
    public Tile borderPrefab;

    public Vector2 ArraySize { get; private set; }
    public Vector2 minPositions { get; private set; }
    public Vector2 maxPositions { get; private set; }

    // Private variables
    private Sprite _tileSprite;
    private Sprite _borderSprite;



    private void Awake()
    {
        Collider = GetComponentInChildren<BoxCollider2D>();

        _tileSprite = tilePrefab.GetComponent<SpriteRenderer>().sprite;
        _borderSprite = borderPrefab.GetComponent<SpriteRenderer>().sprite;
    }



    public void CreateMap()
    {
        // Get the bounds of the boxcollider2D
        Bounds bounds = Collider.bounds;

        // Get the size of the BoxCollider2D
        Vector2 gridSize = Collider.size;

        // Get the size of a tile
        Vector2 tileSize = (_tileSprite.rect.size / _tileSprite.pixelsPerUnit) * tilePrefab.transform.lossyScale;

        // Origin of the grid
        Vector2 origin = new Vector2(bounds.min.x, bounds.min.y);

        // Offset to apply to tiles spawn
        Vector2 offset = tileSize / 2;

        //  Calculate array size
        ArraySize = new Vector2((int)(gridSize.x / tileSize.x), (int)(gridSize.y / tileSize.y));
        List = new Tile[(int)ArraySize.x, (int)ArraySize.y];

        // Create the grid
        CreateGrid(origin, offset);

        // Save min and max positions
        minPositions = List[0, 0].transform.position;
        maxPositions = List[(int)ArraySize.x - 1, (int)ArraySize.y - 1].transform.position;

        // Set the spawn tile
        Vector2 spawnIndex = new Vector2((int)ArraySize.x / 2, (int)ArraySize.y / 2);
        Spawn = List[(int)spawnIndex.x, (int)spawnIndex.y];

        // Create borders around the grid -------------------------------------------------------

        // Get the size of the border tile
        Vector2 borderSize = (_borderSprite.rect.size / _borderSprite.pixelsPerUnit) * borderPrefab.transform.lossyScale;

        // Set origin with an offset (outside the grid)
        origin = new Vector2(bounds.min.x - (tileSize.x), bounds.min.y - (tileSize.y));         // With offset
        //origin = new Vector2(bounds.min.x - (tileSize.x / 2), bounds.min.y - (tileSize.y / 2)); // Without offset

        // Create border
        //CreateBorders(origin, offset);
    }
    public void DeleteMap()
    {
        // Delete Grid
        foreach (Transform transform in gridParent)
            Destroy(transform.gameObject);

        // Delete Border
        foreach (Transform transform in borderParent)
            Destroy(transform.gameObject);

        // Reset tiles array
        for (int y = 0; y < (int)ArraySize.y; y++)
            for (int x = 0; x < (int)ArraySize.x; x++)
                List[x, y] = null;

    }

    private void CreateGrid(Vector2 origin, Vector2 offset)
    {
        for (int y = 0; y < (int)ArraySize.y; y++)
        {
            for (int x = 0; x < (int)ArraySize.x; x++)
            {
                // Instantiate the tile
                Tile tile = Instantiate(tilePrefab, gridParent);

                // Calculate position
                Vector2 position = origin + offset + new Vector2(x * tile.Size.x, y * tile.Size.y);
                Vector2 index = new Vector2(x, y);

                // Initialize tile
                tile.Initialize(position, index, TileType.Type.Grid);

                // Add tile to the array
                List[x, y] = tile;
            }
        }
    }
    private void CreateBorders(Vector2 origin, Vector2 offset)
    {
        // The array size is bigger than the original array (because of the offset origin and the border tile size is half the size  of the grid tile)
        Vector2 size = new Vector2((ArraySize.x * 2) + 4, (ArraySize.y * 2) + 4);   // with offset
        //Vector2 size = new Vector2((ArraySize.x * 2) + 1, (ArraySize.y * 2) + 1);   // Without offset

        for (int y = 0; y < (int)size.y; y++)
        {
            for (int x = 0; x < (int)size.x; x++)
            {
                // Only instantiate tiles on the outside of the grid
                if (x == 0 || y == 0 || x == (int)size.x - 1 || y == (int)size.y - 1)
                {
                    // Instantiate tile border
                    Tile border = Instantiate(borderPrefab, borderParent);

                    // Calculate position
                    Vector2 position = origin + offset + new Vector2(x * border.Size.x, y * border.Size.y);
                    Vector2 index = new Vector2(x, y);

                    // Initialize
                    border.Initialize(position, index, TileType.Type.Border);
                }
            }
        }
    }
}
