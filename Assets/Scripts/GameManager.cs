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
        snake.CreateSnake(gridManager.SpawnTile);
    }
}
