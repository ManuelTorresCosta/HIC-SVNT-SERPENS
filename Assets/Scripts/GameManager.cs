using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GridManager gridManager;
    public Snake snake;


    private void Start()
    {
        gridManager.CreateMap();

        snake.Initialize(gridManager.minPositions, gridManager.maxPositions, gridManager.arraySize);
        snake.CreateSnake(20, Vector2.right, gridManager.SpawnTile);
    }
}
