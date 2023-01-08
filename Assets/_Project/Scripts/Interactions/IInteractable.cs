using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CyberLabStudios.Game.Interactions
{
    public interface IInteractable
    {
        bool enabled { get; }
        void Interact();
        string GetInteractText();
        Transform GetTransform();
    }
}
