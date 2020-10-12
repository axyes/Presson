using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Gère les UI : permet d'appeler les différente scène (fonctions pour boutons)
/// </summary>
public class UITools : MonoBehaviour
{
    public Image ImgTestRotate;
    private void Start()
    {
        if (ImgTestRotate != null) ImgTestRotate.transform.localEulerAngles = new Vector3(0, 0, 0);
    }
    private void Update()
    {
        RotateToTest();
    }
    public void LoadMenu()
    {
        Debug.Log("Load Menu");
        Loader.LoadScene(Loader.Scene.Menu);
    }
    public void LoadGame()
    {
        Debug.Log("Load Game");
        Loader.LoadScene(Loader.Scene.Game);
    }

    void RotateToTest()
    {
        if(ImgTestRotate != null)
        ImgTestRotate.transform.localEulerAngles += new Vector3(0, 0, 2);
    }

}
