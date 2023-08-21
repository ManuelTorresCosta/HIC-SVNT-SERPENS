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
    private float _timer = 0f;
    private float _maxTimer = 60f;


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

        Score.SetRarePointValue(25);

        // Starts gameplay on Update()
        isGameplay = true;
    }



    // Functions
    public void Run()
    {
        if (isGameplay)
        {
            // Handles if the game goes back to the menu
            HandleInputActivity();

            if (Snake.isAlive)
            {
                // Spawn a point if no points are on the grid
                if (Points.CanSpawnCommonPoint())
                    Points.SpawnRandomCommonPoint(Snake.Segments);

                // Spawn a legend if no legends are active
                if (Points.CanSpawnRarePoint())
                    Points.SpawnRandomRarePoint(Snake.Segments);

                // Sets despawn timer for rare point
                if (Points.IsRarePointTimeEnded())
                    Points.DespawnRarePoint(false);

                // Check end game conditions
                if (Snake.CheckSelfCollision() || Points.MaxRarePointsCaptured())
                {
                    // Removes map borders (and grid)
                    Tiles.DeleteMap();

                    // Despawn points
                    Points.DespawnCommonPoint();
                    Points.DespawnRarePoint(true);

                    // Disable score UI
                    Score.SetActive(false);

                    // Run the gameover effect
                    Effects.RunGameOver();
                    
                    isGameplay = false;
                }
                // Snake movement
                else
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
                        Points.DespawnRarePoint(true);

                        //Effects.ColorFadeEffect();
                    }

                    // Update snake sprites when it eats
                    if (Snake.IsCollidingWithEatenPoint())
                        Snake.UpdateEatenPoints();
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

            // Checks if game over effect is finished
            if (Effects.IsGameOverFinished())
                SceneManager.LoadScene(1);
        }

        
    }

    private void HandleInputActivity()
    {
        // If the user does not interact with the game for some time...
        if (_timer >= 0)
        {
            if (_timer < _maxTimer)
                _timer += Time.deltaTime;
            else
            {
                // Overlay effect kicks in (fade in)
                Effects.FadeInOverlay();
                _timer = -1;
            }
        }
        else
        {
            // Loads the scene 0 at the end of the animation
            if (_timer > -4)
                _timer -= Time.deltaTime;
            else
                SceneManager.LoadScene(0);
        }

        // Reset timer
        if (Input.anyKeyDown)
        {
            if (_timer < 0)
                Effects.FadeOutOverlay();

            _timer = 0;
        }
    }
}
