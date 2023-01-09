using CyberLabStudios.Game.Interactions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneInteraction : MonoBehaviour, IInteractable
{
    public string interactionText;
    public string sceneName;
    public bool interacted { get; set; }

    public string GetInteractText()
    {
        return interactionText;
    }

    public Transform GetTransform()
    {
        return transform;
    }

    public void Interact()
    {
        SceneManager.LoadScene(sceneName);
    }
}
