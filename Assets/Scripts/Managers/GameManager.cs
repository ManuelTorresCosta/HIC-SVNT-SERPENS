using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public TileManager Tiles { get; private set; }
    public Snake Snake { get; private set; }
    public PointsManager Points { get; private set; }
    public ScoreManager Score { get; private set; }
    public EffectsManager Effects { get; private set; }

    public GameObject gameOverObj;

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
        Effects = transform.parent.GetComponentInChildren<EffectsManager>();
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

        Score.SetBonusPointUIActive(false);

        // Starts gameplay on Update()
        isGameplay = true;
    }

    
    // Functions
    public void Run(float inputX, float inputY)
    {
        if (isGameplay)
        {
            // Handles if the game goes back to the menu
            HandleInputActivity(inputX, inputY);

            if (Snake.isAlive)
            {
                HandlePoints();

                // Check end game conditions
                if (Snake.CheckSelfCollision() || Points.MaxStonesCaptured())
                {
                    // Removes map borders (and grid)
                    Tiles.DeleteMap();

                    // Despawn points
                    Points.DespawnTale();
                    Points.DespawnStone(true);

                    // Disable score UI
                    Score.SetUIActive(false);

                    // If Last stone captured
                    if (Points.MaxStonesCaptured())
                        Effects.RunGameOver();
                    else
                    {
                        // Show game over text
                        //Effects.Camera.backgroundColor = Color.black;
                        gameOverObj.gameObject.SetActive(true);
                    }

                    isGameplay = false;
                }
                // Snake movement
                else
                {
                    // Move snake
                    Snake.HandleMovement(inputX, inputY);

                    // Check collision with a point
                    if (Snake.CheckCollisionWith(Points.Tale))
                    {
                        // Make snake grow
                        Snake.Grow();

                        // Add point value to score
                        Score.AddPoint(Points.Tale.Value, TileType.Type.Tale);

                        // Remove point
                        Points.DespawnTale();
                    }
                    else if (Snake.CheckCollisionWith(Points.Stone))
                    {
                        // Make snake grow
                        Snake.Grow();

                        // Add point value to score
                        Score.AddPoint(Points.Stone.Value, TileType.Type.Stone);

                        // Remove point
                        Points.DespawnStone(true);
                    }
                    else
                        Snake.eating = false;
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

            // Last stone captured
            if (Points.MaxStonesCaptured())
            {
                // Checks if game over effect is finished
                if (Effects.IsGameOverFinished())
                    SceneManager.LoadScene(1);
            }
            else if (Input.anyKeyDown)
                SceneManager.LoadScene(0);
        }
    }


    private void HandleInputActivity(float inputX, float inputY)
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
        if (Input.anyKeyDown || inputX != 0 || inputY != 0)
        {
            if (_timer < 0)
                Effects.FadeOutOverlay();

            _timer = 0;
        }
    }
    private void HandlePoints()
    {
        // Spawn a point if no points are on the grid
        if (Points.CanSpawnTale())
            Points.SpawnTale(Snake.Segments);

        // Spawn a legend if no legends are active
        if (Points.CanSpawnStone())
        {
            Points.SpawnRandomStone(Snake.Segments);
            Score.SetBonusPointUIActive(true);
        }

        // Sets despawn timer for rare point
        if (Points.IsStoneTimeEnded() || Points.Stone == null)
        {
            Points.DespawnStone(false);
            Score.SetBonusPointUIActive(false);
        }
        // Update the UI
        else
            if (Points.Stone != null)
            Score.UpdateBonusPointDigits((int)Points.stoneValue);
    }
}
