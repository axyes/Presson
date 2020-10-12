using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Charge les scènes
/// </summary>
public static class Loader
{
    /// <summary>
    /// Enum prenant en compte toutes les scènes
    /// </summary>
    public enum Scene
    {
        Loading,
        SplashScreen,
        Menu,
        Game,
        Editor
    }

    //Delegate
    private static Action onLoaderCallback;

    /// <summary>
    /// Charge la scene en passant par le loading
    /// </summary>
    /// <param name="scene"></param>
    public static void LoadScene(Scene scene)
    {
        //Chaque fois que onLoadCallback est appelé, charge la scène sélectionnée
        onLoaderCallback = () =>
        {
            SceneManager.LoadScene(scene.ToString());
        };

        //Charge la scène de loading
        SceneManager.LoadScene(Scene.Loading.ToString());
        
    }

    /// <summary>
    /// Appelle la fonction onLoaderCallback en Delegate
    /// </summary>
    public static void LoaderCallback()
    {
        if(onLoaderCallback != null)
        {
            onLoaderCallback();
            onLoaderCallback = null;
        }
    }
}
