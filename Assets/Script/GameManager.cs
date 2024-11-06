using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject titleScreen;
    public GameObject restartScreen;
    public Button startButton;
    public Button restartButton;
    public bool isGameActive = false;

    private SpawnManager spawnManager;

    void Start()
    {
        // Find and assign the SpawnManager component in the scene
        spawnManager = FindObjectOfType<SpawnManager>();

        // Ensure the title screen is shown at the start
        titleScreen.SetActive(true);
        restartScreen.SetActive(false);
    }

    public void StartGame()
    {
        isGameActive = true; // Activate the game
        titleScreen.SetActive(false); // Hide the title screen

        // Start spawning objects if SpawnManager exists
        if (spawnManager != null)
        {
            spawnManager.StartSpawning();
        }
    }

    public void EndGame()
    {
        isGameActive = false; // Deactivate the game
        titleScreen.SetActive(true); // Show the title screen
        Debug.Log("nyork");
        // Stop spawning objects if SpawnManager exists
        if (spawnManager != null)
        {
            spawnManager.StopSpawning();
        }

        restartScreen.SetActive(true);
    }
    public void RestartGame()
    {

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

}
