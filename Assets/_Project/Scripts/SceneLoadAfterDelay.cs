using CyberLabStudios.Game.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneLoadAfterDelay : MonoBehaviour
{
    public float delay;

    public void LoadScene(string sceneName)
    {
        StartCoroutine(Loader(sceneName));
    }

    IEnumerator Loader(string sceneName)
    {
        yield return new WaitForSeconds(delay);
        Utility.LoadScene(sceneName);
    }
}
