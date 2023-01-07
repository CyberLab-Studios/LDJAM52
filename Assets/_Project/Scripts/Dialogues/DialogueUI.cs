using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Rendering;
using UnityEngine;

public class DialogueUI : MonoBehaviour
{
    public TextMeshProUGUI dialogueText;
    public TextMeshProUGUI whosTalkingText;
    public CanvasGroup dialoguePanel;
    public float charSpeed;
    public GameObject nextButton;
    bool isTyping = false;

    private void Awake()
    {
        GameEvents.Instance.onDialogueShow += ShowSentence;
        GameEvents.Instance.onDialogueStart += ShowPanel;
        GameEvents.Instance.onDialogueEnd += HidePanel;
    }

    private void Start()
    {
        HidePanel();
    }

    private void OnDisable()
    {
        GameEvents.Instance.onDialogueShow -= ShowSentence;
        GameEvents.Instance.onDialogueStart -= ShowPanel;
        GameEvents.Instance.onDialogueEnd -= HidePanel;
    }

    void ShowPanel()
    {
        dialoguePanel.alpha = 1.0f;
    }

    void HidePanel()
    {
        dialoguePanel.alpha = 0;
    }

    void ShowSentence(string whosTalking, string sentence)
    {
        StopAllCoroutines();
        whosTalkingText.text = whosTalking;
        StartCoroutine(TypeSentence(sentence));
    }

    IEnumerator TypeSentence(string sentence)
    {
        dialogueText.text = "";

        isTyping = true;
        foreach (var character in sentence.ToCharArray())
        {
            dialogueText.text += character;
            yield return new WaitForSeconds(charSpeed);
        }
        isTyping = false;
    }
}
