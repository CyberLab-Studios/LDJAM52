using CyberLabStudios.Game.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace CyberLabStudios.Game.Interactions
{
    public class ButtonInteractable : MonoBehaviour, IInteractable
    {
        [SerializeField]
        internal bool repetable = false;
        [SerializeField]
        internal bool wasUsed = false;

        public UnityEvent onInteract;

        public string interactionMessage;

        public bool interacted { get; set; }

        public string GetInteractText()
        {
            return !interactionMessage.IsFilled() ? "Interagisci" : interactionMessage;
        }

        public Transform GetTransform()
        {
            return transform;
        }

        public void Interact()
        {
            if (wasUsed && !repetable) return;
            onInteract?.Invoke();
        }
    }
}
