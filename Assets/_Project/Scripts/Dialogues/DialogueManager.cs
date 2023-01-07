using CyberLabStudios.Game.Interactions;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DialogueManager : MonoBehaviour, IInteractable
{
    public bool playOnAwake = false;
    public string iteractionText;
    public bool resetIndexOnEnd = true;

    [TextArea(2, 5)]
    public List<string> sentences;

    public UnityEvent onDialogueStart;
    public UnityEvent onDialogueEnd;

    bool isShowing = false;
    int sentenceIndex = -1;

    void Start()
    {
        if (playOnAwake)
        {
            ShowNextSentence();
        }
    }

    public void Interact()
    {
        ShowNextSentence();
    }

    private void ShowNextSentence()
    {
        sentenceIndex++;
        if (!isShowing)
        {
            GameEvents.Instance.OnDialogueStart();
            onDialogueStart?.Invoke();
            isShowing = true;
        }

        if (sentenceIndex >= sentences.Count)
        {
            isShowing = false;
            GameEvents.Instance.OnDialogueEnd();
            onDialogueEnd?.Invoke();

            if (resetIndexOnEnd)
                sentenceIndex = -1;

            return;
        }

        GameEvents.Instance.OnDialogueShow(sentences[sentenceIndex]);
    }

    public string GetInteractText()
    {
        return iteractionText;
    }

    public Transform GetTransform()
    {
        return transform;
    }
}
