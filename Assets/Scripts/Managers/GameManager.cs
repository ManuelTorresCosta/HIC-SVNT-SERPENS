using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public TileManager Tiles { get; private set; }
    public Snake Snake { get; private set; }
    public PointsManager Points { get; private set; }
    public ScoreManager Score { get; private set; }

    public EffectsManager Effects;

    public bool isGameplay;



    // Unity functions
    private void Awake()
    {
        Tiles = GetComponentInChildren<TileManager>();
        Snake = GetComponentInChildren<Snake>();
        Points = GetComponentInChildren<PointsManager>();
        Score = GetComponentInChildren<ScoreManager>();
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



    // Functions
    public void Run()
    {
        if (isGameplay)
        {
            if (Snake.isAlive)
            {
                // Spawn a point if no points are on the grid
                if (Points.CanSpawnCommonPoint())
                    Points.SpawnRandomCommonPoint(Snake.Segments);

                // Spawn a legend if no legends are active
                if (Points.CanSpawnRarePoint())
                    Points.SpawnRandomRarePoint(Snake.Segments);

                // Check if snake is going to collide with self
                if (!Snake.CheckSelfCollision())
                {
                    // Move snake
                    Snake.HandleMovement();

                    // Check collision with a point
                    if (Snake.CheckCollisionWith(Points.commonPoint))
                    {
                        // Make snake grow
                        Snake.Grow();

                        // Add point value to score
                        Score.AddPoint(Points.commonPoint.Value);

                        // Remove point
                        Points.DespawnCommonPoint();

                        //Effects.ColorFadeEffect();
                    }
                    else if (Snake.CheckCollisionWith(Points.rarePoint))
                    {
                        // Make snake grow
                        Snake.Grow();

                        // Add point value to score
                        Score.AddPoint(Points.rarePoint.Value);

                        // Remove point
                        Points.DespawnRarePoint();

                        //Effects.ColorFadeEffect();
                    }
                }
                // Snake collided with self
                else
                {
                    // Removes map and points
                    Tiles.DeleteMap();
                    Score.SetActive(false);
                    Points.DespawnCommonPoint();
                    
                    // Run the transition effect
                    Effects.RunTransition(() =>
                    {
                        // Load the end scene
                        SceneManager.LoadScene(1);
                    });
                    isGameplay = false;
                }
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
