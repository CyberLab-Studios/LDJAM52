using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CyberLabStudios.Game.Interactions
{
    public interface IInteractable
    {
        void Interact();
        string GetInteractText();
        Transform GetTransform();
    }
}
