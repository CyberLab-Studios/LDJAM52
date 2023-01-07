using CyberLabStudios.Game.Interactions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquippableScythe : MonoBehaviour, IInteractable
{
    public string interactionText;
    public ScytheData scythe;

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
        GameEvents.Instance.OnEquipScythe(scythe);
        Destroy(gameObject);
    }
}
