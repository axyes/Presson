using System.Collections;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Interactions;

public class SplashscreenController : MonoBehaviour
{
    private GameControls curControls;
    // Start is called before the first frame update
    void Awake()
    {
        curControls = new GameControls();
        curControls.Enable();
        curControls.splashscreen.ButtonAction.performed += ctx => LoadMenu();
        //curControls.splashscreen.ButtonAction.started += ctx => Debug.Log("started");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void LoadMenu()
    {
        Loader.LoadScene(Loader.Scene.Menu);
        curControls.Disable();
    }

    
}
