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

    //Création d'une classe vide qui hérite de MonoBehaviour (pour l'update)
    private class simpleMonoBehaviour : MonoBehaviour { }

    //Delegate
    private static Action onLoaderCallback;
    private static AsyncOperation loadingAsyncOperation;
    /// <summary>
    /// Charge la scene en passant par le loading
    /// </summary>
    /// <param name="scene"></param>
    public static void LoadScene(Scene scene)
    {
        //Chaque fois que onLoadCallback est appelé, charge la scène sélectionnée
        onLoaderCallback = () =>
        {
            GameObject loadingGameObject = new GameObject("Loading Game Object");
            loadingGameObject.AddComponent<simpleMonoBehaviour>().StartCoroutine(LoadSceneAsync(scene));
        };

        //Charge la scène de loading
        SceneManager.LoadScene(Scene.Loading.ToString());
        
    }

    /// <summary>
    /// Coroutine asynchronisée pour charger la futur scene
    /// </summary>
    /// <param name="scene"></param>
    /// <returns></returns>
    private static IEnumerator LoadSceneAsync(Scene scene)
    {
        yield return null;
        loadingAsyncOperation = SceneManager.LoadSceneAsync(scene.ToString());
        while(!loadingAsyncOperation.isDone)
        {
            yield return null;
        }
    }

    /// <summary>
    /// Renvoie la progression du chargement
    /// </summary>
    /// <returns></returns>
    public static float GetLoadingProgress()
    {
        if(loadingAsyncOperation != null)
        {
            return loadingAsyncOperation.progress; //Renvoie la progression actuelle
        }
        else
        {
            return 1f; //Progression terminée
        }
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
