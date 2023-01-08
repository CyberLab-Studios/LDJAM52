using CyberLabStudios.Game.Interactions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleTile : MonoBehaviour, IInteractable
{
    public bool interacted { get; set; }
    public string interactionText;
    public Vector3 correctPosition;

    void Awake()
    {
        correctPosition = transform.localPosition;
    }


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
        GameEvents.Instance.OnTileMove(GetTransform());
    }
}
