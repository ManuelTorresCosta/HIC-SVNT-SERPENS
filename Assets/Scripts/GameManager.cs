using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GridManager gridManager;
    public Snake snake;
    public PointsGenerator pointsGenerator;

    public bool isGameplay;


    private void Start()
    {
        // Create the grid and border
        gridManager.CreateMap();

        // Initializes snake properties
        snake.Initialize();

        // Set map limits
        snake.SetMapLimits(gridManager.minPositions, gridManager.maxPositions, Vector2.zero, gridManager.arraySize);
        
        // Create the snake objects
        snake.CreateSnake(Vector2.right, gridManager.SpawnTile);

        // Starts gameplay on Update()
        isGameplay = true;
    }
    private void Update()
    {
        if (isGameplay)
        {
            if (snake.isAlive)
            {
                // Generate a point if no points are on the grid
                if (pointsGenerator.Point == null)
                    pointsGenerator.GenerateRandomPoint(gridManager.Tiles, snake.Segments);

                // Check if snake is going to collide with self
                if (!snake.CheckSelfCollision())
                {
                    // Move snake
                    snake.HandleMovement();

                    // Check collision with a point
                    if (snake.CheckCollisionWith(pointsGenerator.Point))
                    {
                        // Make snake grow
                        snake.Grow();

                        // Remove point
                        pointsGenerator.DespawnPoint();
                    }
                }
                // Snake collided with self
                else
                {
                    // Change snake alive state 
                    snake.isAlive = false;
                }
            }
            else
            {
                // Despawn snake
                snake.Die(() =>
                {
                    // Stop running gameplay code
                    isGameplay = false;
                });
            }
        }
        else
        {
            // Press key to restart
            if (Input.GetKeyDown(KeyCode.Space))
            {
                // Initializes the snake variables
                snake.Initialize();

                // Recreates the snake
                snake.CreateSnake(Vector2.right, gridManager.SpawnTile);

                // Turn gameplay back on
                isGameplay = true;
            }
        }
    }
}
