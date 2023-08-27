using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SERPENS : MonoBehaviour
{
    public MenuManager Menu { get; private set; }
    public GameManager Game { get; private set; }

    public enum GameState
    {
        MENU,
        GAMEPLAY,
        CREDITS
    }
    public GameState gameState;



    // Unity Functions
    private void Awake()
    {
        Application.targetFrameRate = 30;

        Menu = GetComponentInChildren<MenuManager>();
        Game = GetComponentInChildren<GameManager>();
    }
    private void Start()
    {
        switch (gameState)
        {
            case GameState.MENU:
                Menu.gameObject.SetActive(true);
                Game.gameObject.SetActive(false);
                break;

            case GameState.GAMEPLAY:
                Menu.gameObject.SetActive(false);
                Game.gameObject.SetActive(true);
                break;

            case GameState.CREDITS:
                break;
        }
        Game.gameObject.SetActive(false);
    }
    private void Update()
    {
        switch (gameState)
        {
            case GameState.MENU:
                // Run menu
                Menu.Run();

                // Trigger transition on any key pressed
                if (Input.anyKeyDown)
                {
                    Menu.StartTransition();
                    StartGame();
                }
                break;

            case GameState.GAMEPLAY:

                // Get Joystick input
                float inputX = Input.GetAxisRaw("Horizontal");
                float inputY = Input.GetAxisRaw("Vertical");

                // Start gameplay
                Game.Run(inputX, inputY);

                break;

            case GameState.CREDITS:
                break;
        }
    }



    private void StartGame()
    {
        Game.gameObject.SetActive(true);
        gameState = GameState.GAMEPLAY;
    }
    private void StartMenu()
    {
        Menu.gameObject.SetActive(false);
        gameState = GameState.MENU;
    }
}
