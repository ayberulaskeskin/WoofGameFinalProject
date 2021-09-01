using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Game manager is a state machine that controls the state of the game.
    public static GameManager manager;       
    public enum GameState 
    {
        Prepare,
        MainGame,
        FinishGame,
    }
    private GameState _currentGameState;
    public GameState CurrentGameState
    {
        get { return _currentGameState;}
        set
        {
            switch (value)
            {
                case GameState.Prepare:
                    break;
                case GameState.MainGame:                       
                    break;
                case GameState.FinishGame:                        
                    break;
            }
            _currentGameState = value;
        }           
    }
    private void Awake() 
    {
        manager = this;    
    }
    private void Update()
    {
        switch (CurrentGameState)
        {
            case GameState.Prepare:
                break;
            case GameState.MainGame:
                break;
            case GameState.FinishGame:
                break;
        }
    }
    public void ToMainGame()
    {
        _currentGameState = GameState.MainGame;
    }
    public void ToFinishGame()
    {
        _currentGameState = GameState.FinishGame;
    }
    public void ToPrepare()
    {
        _currentGameState = GameState.Prepare;
    }
}