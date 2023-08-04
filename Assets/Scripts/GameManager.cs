using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public TileManager Tiles { get; private set; }
    public Snake Snake { get; private set; }
    public PointsGenerator PointsGenerator { get; private set; }
    public Transition Transition;

    public Animator CameraFx { get; private set; }

    public bool isGameplay;



    // Unity functions
    private void Awake()
    {
        Tiles = GetComponentInChildren<TileManager>();
        Snake = GetComponentInChildren<Snake>();
        PointsGenerator = GetComponentInChildren<PointsGenerator>();

        CameraFx = transform.parent.GetComponentInChildren<Animator>();
    }
    private void Start()
    {
        // Create the grid and border
        Tiles.CreateMap();

        // Initializes snake properties
        Snake.Initialize();

        // Set map limits
        Snake.SetMapLimits(Tiles.minPositions, Tiles.maxPositions, Vector2.zero, Tiles.ArraySize);
        
        // Create the snake objects
        Snake.CreateSnake(Vector2.right, Tiles.Spawn);

        // Starts gameplay on Update()
        isGameplay = true;
    }
    private void Update()
    {
        if (isGameplay)
        {
            if (Snake.isAlive)
            {
                // Generate a point if no points are on the grid
                if (PointsGenerator.Point == null)
                    PointsGenerator.GenerateRandomPoint(Tiles.List, Snake.Segments);

                // Check if snake is going to collide with self
                if (!Snake.CheckSelfCollision())
                {
                    // Move snake
                    Snake.HandleMovement();

                    // Check collision with a point
                    if (Snake.CheckCollisionWith(PointsGenerator.Point))
                    {
                        // Make snake grow
                        Snake.Grow();

                        // Remove point
                        PointsGenerator.DespawnPoint();
                    }
                }
                // Snake collided with self
                else
                {
                    // Removes map and points
                    Tiles.DeleteMap();
                    PointsGenerator.DespawnPoint();

                    //Transition.Run();
                    StartCoroutine(Transition.RunTransition());
                    isGameplay = false;
                }
            }
            else
            {
                
            }
        }
        else
        {
            // Blink and despawn
            Snake.Die(() =>
            {
                Snake.Despawn();
                Snake.isAlive = false;
            });
        }
    }



    // Actions
    private void Restart()
    {
        // Initializes the snake variables
        Snake.Initialize();

        // Recreates the snake
        Snake.CreateSnake(Vector2.right, Tiles.Spawn);

        // Turn gameplay back on
        isGameplay = true;
    }
}
