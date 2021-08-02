using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadManager : MonoBehaviour {

    //private UILoadingOverlay loadOverlay;
    private int currentScene;

    public SceneLoadData mainMenuScene;
    public SceneLoadData[] gameScenes;

    private void Start()
    {
    }

    public void LoadMainMenu()
    {
    }

    public void StartNewGame()
    {
        LoadGameScene(0);
    }

    public void LoadGameScene(int gameSceneIndex)
    {
        if (gameSceneIndex < 0 || gameSceneIndex >= gameScenes.Length)
            return;

        currentScene = gameSceneIndex;
        var scene = gameScenes[gameSceneIndex];
    }

    public void ReloadScene()
    {
        LoadGameScene(currentScene);
    }
}
