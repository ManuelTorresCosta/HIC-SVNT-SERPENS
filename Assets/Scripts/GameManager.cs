using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GridManager gridManager;
    public Snake snake;



    private void Start()
    {
        // Create the grid and border
        gridManager.CreateMap();

        // Initializes snake properties
        snake.Initialize(gridManager.GetTileSize(), gridManager.minPositions, gridManager.maxPositions, gridManager.arraySize);
        
        // Create the snake objects
        snake.CreateSnake(Vector2.right, gridManager.SpawnTile);
    }
}
