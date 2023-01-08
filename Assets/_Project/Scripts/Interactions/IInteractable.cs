using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CyberLabStudios.Game.Interactions
{
    public interface IInteractable
    {
        bool interacted { get; set; }
        bool enabled { get; }
        void Interact();
        string GetInteractText();
        Transform GetTransform();
    }
}
