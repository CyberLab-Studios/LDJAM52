using CyberLabStudios.Game.Interactions;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

namespace CyberLabStudios.Game.Interactions
{
    public class CharacterInteraction : MonoBehaviour
    {
        [SerializeField]
        private float interactRange = 4f;

        public void OnInteract(CallbackContext ctx)
        {
            if (ctx.started)
            {
                CheckInteraction();
            }
        }

        void CheckInteraction()
        {
            IInteractable interactable = GetInteractableObject();
            if (interactable != null)
            {
                interactable.Interact();
            }
        }

        public IInteractable GetInteractableObject()
        {
            List<IInteractable> interactables = new List<IInteractable>();

            Collider[] colliders = Physics.OverlapSphere(transform.position, interactRange);
            foreach (Collider collider in colliders)
            {
                if (collider.TryGetComponent(out IInteractable interactable))
                {
                    interactables.Add(interactable);
                }
            }

            IInteractable closestInteractable = interactables.OrderBy(x => Vector3.Distance(x.GetTransform().position, transform.position)).FirstOrDefault();

            return closestInteractable;
        }
    }
}
