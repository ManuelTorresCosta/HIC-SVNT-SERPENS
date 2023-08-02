using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public TileGenerator gridManager;
    public Snake snake;
    public PointsGenerator pointsGenerator;
    public Transition Transition { get; private set; }

    public bool isGameplay;



    // Unity functions
    private void Awake()
    {
        Transition = GetComponentInChildren<Transition>();
    }
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
                    StopGameplay();
                });
            }
        }
        else
        {
            // Press key to restart
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Restart();
            }
        }

        // Key to trigger end game transition
        if (Input.GetKeyDown(KeyCode.Return))
        {
            StopGameplay();
            gridManager.DeleteMap();

            // Runs the end game transition
            StartCoroutine(Transition.RunTransition());
        }
    }


    // Actions
    private void StopGameplay()
    {
        snake.isAlive = false;
        snake.Die(() =>
        {
            // Remove point
            pointsGenerator.DespawnPoint();

            // Stop running gameplay code
            isGameplay = false;
        });
    }
    private void Restart()
    {
        // Initializes the snake variables
        snake.Initialize();

        // Recreates the snake
        snake.CreateSnake(Vector2.right, gridManager.SpawnTile);

        // Turn gameplay back on
        isGameplay = true;
    }
}
