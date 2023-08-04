using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SERPENS : MonoBehaviour
{
    public Camera Camera { get; private set; }
    public GameManager Game { get; private set; }
    public MenuManager Menu { get; private set; }

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
        Camera = GetComponentInChildren<Camera>();
        Game = GetComponentInChildren<GameManager>();
        Menu = GetComponentInChildren<MenuManager>();
    }
    private void Start()
    {
        
    }



    // This class controls the states and transitions between game logic (menu -> gameplay -> credits on lose/win)
}
