using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Présent seulement dans la scene Loading,
/// 
/// </summary>
public class LoaderCallBack : MonoBehaviour
{
    private bool IsFirstUpdate =  true;

    private void Update()
    {
        if(IsFirstUpdate)
        {
            IsFirstUpdate = false;
            Loader.LoaderCallback();
        }
    }
}
