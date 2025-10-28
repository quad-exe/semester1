using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static event Action<GameState> OnBeforeStateChanged;
    public static event Action<GameState> OnAfterStateChanged;

    private static GameManager _instance;

    public static GameManager Instance 
    {
        get 
        {
            if (_instance == null) { Debug.LogError("The singleton GameManager is null"); }
            return _instance;
        }
    }

    public GameState State { get; private set; }

    // this will set the gamemanager when the app is first started.
    private void Awake() {
        _instance = this;
    }
    void Start() {
        ChangeState(GameState.MainMenu);
    } 

    public void ChangeState(GameState newState) {
        OnBeforeStateChanged?.Invoke(newState);

        State = newState;
        switch (newState) {
            case GameState.MainMenu:
                HandleMainMenu();
                break;
            case GameState.PauseMenu:
                HandlePauseMenu();
                break;
            case GameState.CompletedLevel:
                HandleCompletedLevelMenu();
                break;
            case GameState.GameOver:
                HandleGameOver();
                break;
            case GameState.GamePlaying:
                HandleGamePlaying();
                break;
            case GameState.CompletedGame:
                HandleCompletedGame();
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
        }
        OnAfterStateChanged?.Invoke(newState);
        Debug.Log($"New state: {newState}");
    }

    private void HandleMainMenu() {
        // TODO launch the main menu to start the game
        Debug.Log("From HandleMainMenu in the GameManager");
    }

    private void HandlePauseMenu() {
        // TODO should pause the game and desplay a pause screen when a specifik button is pressed
        // when the button is pressed again game play should resume.
        throw new NotImplementedException();
    }

    private void HandleCompletedLevel() {
        // TODO Will launch the end of level menu
        throw new NotImplementedException();
    }

    private void HandleGameOver() {
        // TODO handle what ever needs to happen when the player dies.
        throw new NotImplementedException();
    }

    private void HandleGamePlaying() {
        // TODO this is the state when the normal gameplay is running
        throw new NotImplementedException();
    }

    private void HandleCompletedGame() {
        // TODO this is the state of a final screen that shows up, when the player has comleted the game.
        throw new NotImplementedException();
    }
}


[Serializable]
public enum GameState
{
    MainMenu = 0,
    PauseMenu = 1,
    EndLevelMenu = 2,
    GameOver = 3,
    GamePlaying = 4,
    CompletedGame = 5,
}