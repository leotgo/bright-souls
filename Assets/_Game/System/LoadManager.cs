using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DuloGames.UI;

public class LoadManager : MonoBehaviour {

    private UILoadingOverlay loadOverlay;
    private int currentScene;

    public SceneLoadData mainMenuScene;
    public SceneLoadData[] gameScenes;

    private void Start()
    {
        loadOverlay = GetComponentInChildren<UILoadingOverlay>();
        loadOverlay.gameObject.SetActive(false);
    }

    public void LoadMainMenu()
    {
        loadOverlay.gameObject.SetActive(true);
        loadOverlay.BackgroundSprite = mainMenuScene.loadScreenScreenshot;
        loadOverlay.LoadScene(mainMenuScene.sceneBuildIndex);
    }

    public void StartNewGame()
    {
        LoadGameScene(0);
    }

    public void LoadGameScene(int gameSceneIndex)
    {
        if (gameSceneIndex < 0 || gameSceneIndex >= gameScenes.Length)
            return;

        loadOverlay.gameObject.SetActive(true);
        currentScene = gameSceneIndex;
        var scene = gameScenes[gameSceneIndex];
        loadOverlay.BackgroundSprite = scene.loadScreenScreenshot;
        loadOverlay.LoadScene(scene.sceneBuildIndex);
    }

    public void ReloadScene()
    {
        LoadGameScene(currentScene);
    }
}
