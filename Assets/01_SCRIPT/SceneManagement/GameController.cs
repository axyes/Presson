using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    private GameControls controls;

    private GameObject pauseMenuObject;
    private bool isPaused = false;
    // Start is called before the first frame update
    void Awake()
    {
        controls = new GameControls();

        controls.Gameplay.ButtonStart.performed += cxt => PauseGame();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void PauseGame()
    {
        Debug.Log("Pause the game");
        
    }
}
