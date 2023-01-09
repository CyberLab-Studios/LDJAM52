using CyberLabStudios.Game.Interactions;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DialogueManager : MonoBehaviour, IInteractable
{
    public bool playOnAwake = false;
    public string iteractionText;
    public bool resetIndexOnEnd = true;
    public bool isAutoplay = false;
    public float autoPlayTime = 2;
    PlayerMovement player;

    public List<DialogueSentence> sentences;

    public UnityEvent onDialogueStart;
    public UnityEvent onDialogueEnd;

    bool isShowing = false;
    int sentenceIndex = -1;

    public bool interacted { get; set; }

    void Start()
    {
        if (playOnAwake)
        {
            ShowNextSentence();
        }
    }

    public void Interact()
    {
        GameEvents.Instance.OnLookAt(transform.position);
        ShowNextSentence();
    }

    public void ShowNextSentence()
    {
        sentenceIndex++;
        if (!isShowing)
        {
            GameEvents.Instance.OnDialogueStart();
            onDialogueStart?.Invoke();
            isShowing = true;
            interacted = true;
        }

        if (sentenceIndex >= sentences.Count)
        {
            isShowing = false;
            GameEvents.Instance.OnDialogueEnd();
            onDialogueEnd?.Invoke();
            interacted = false;

            if (resetIndexOnEnd)
                sentenceIndex = -1;

            return;
        }

        var actualSentence = sentences[sentenceIndex];
        GameEvents.Instance.OnDialogueShow(actualSentence.WhosTalking, actualSentence.Sentence);

        Invoke("InvokeEvent", actualSentence.eventDelay);

        if (isAutoplay)
        {
            StopAllCoroutines();
            StartCoroutine(PlayNextSentence());
        }
    }

    void InvokeEvent()
    {
        sentences[sentenceIndex].sentenceEvent?.Invoke();
    }

    IEnumerator PlayNextSentence()
    {
        yield return new WaitForSeconds(autoPlayTime);
        ShowNextSentence();
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

[Serializable]
public class DialogueSentence
{
    public string WhosTalking;
    [TextArea(2, 5)]
    public string Sentence;
    public float eventDelay;
    public UnityEvent sentenceEvent;
}