using TMPro;
using UnityEngine;

namespace CyberLabStudios.Game.Interactions
{
    public class InteractablePopup : MonoBehaviour
    {
        [SerializeField] private CharacterInteraction charInteraction;
        [SerializeField] private CanvasGroup uiContainer;
        [SerializeField] private TextMeshProUGUI interactionText;

        void Update()
        {
            if (charInteraction != null)
            {
                var inter = charInteraction.GetInteractableObject();
                if (inter != null)
                {
                    if (inter.enabled && !inter.interacted)
                    {
                        ShowPopup(inter);
                        return;
                    }
                }
                else
                {
                    HidePopup();
                    return;
                }
            }
        }

        public void ShowPopup(IInteractable interactable)
        {
            uiContainer.alpha = 1f;
            interactionText.text = interactable.GetInteractText();
        }

        public void HidePopup()
        {
            uiContainer.alpha = 0f;
        }
    }

}
