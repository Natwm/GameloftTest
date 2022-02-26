using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;

    void Awake()
    {
        if (instance != null)
            Debug.LogWarning("Multiple instance of same Singleton : LevelManager");
        instance = this;
    }

    /// <summary>
    /// Reload the current scene
    /// </summary>
    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    /// <summary>
    /// Load the next Scene.
    /// If there is no more scene, load the game menu.
    /// </summary>
    public void LoadNextScene()
    {
        if (SceneManager.sceneCountInBuildSettings > SceneManager.GetActiveScene().buildIndex + 1)
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        else
            LoadMenu();
    }

    /// <summary>
    /// Return to the menu Scene
    /// </summary>
    public void LoadMenu()
    {
        SceneManager.LoadScene(0);
    }

}
