using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Patterns.Observer;

public class GameManager : MonoBehaviour, IObserver {

    public static GameManager singleton = null;
    public GameManager prefab;

    private LoadManager loadManager;
    private LoadManager LoadManager {
        get {
            if (!loadManager)
                loadManager = GetComponentInChildren<LoadManager>();
            return loadManager;
        }
    }

    public enum GameState
    {
        Playing,
        Paused
    }
    public GameState gameState = GameState.Playing;

    private void Awake()
    {
        if (singleton == null)
        {
            singleton = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else if (singleton != this)
        {
            Debug.Log("Destroy " + this.name);
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        SubscribeToMessages();
    }

    private void SubscribeToMessages()
    {
        this.Observe(Message.System_NewGame);
        this.Observe(Message.System_TogglePause);
        this.Observe(Message.System_ReloadScene);
        this.Observe(Message.System_Exit);
    }

    public void OnNotification(object sender, Message msg, params object[] args)
    {
        switch(msg)
        {
            case Message.System_NewGame:
                NewGame();
                break;
            case Message.System_TogglePause:
                LoadManager.LoadMainMenu();
                break;
            case Message.System_ReloadScene:
                LoadManager.ReloadScene();
                break;
            case Message.System_Exit:
                QuitGame();
                break;
        }
    }

    public void NewGame()
    {
        LoadManager.StartNewGame();
    }

    public void PauseGame()
    {
        if (gameState == GameState.Playing)
        {
            this.Notify(Message.System_Pause);
            Time.timeScale = 0f;
            gameState = GameState.Paused;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else if (gameState == GameState.Paused)
        {
            this.Notify(Message.System_Unpause);
            Time.timeScale = 1f;
            gameState = GameState.Playing;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    [ExecuteInEditMode]
    public void QuitGame()
    {
        Application.Quit();
    }

}
