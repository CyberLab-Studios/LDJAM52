using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEvents : SingletonClass<GameEvents>
{
    public event Action<string> onButtonInteract;
    public void OnButtonInteract(string id)
    {
        onButtonInteract?.Invoke(id);
    }

    public event Action<string, string> onDialogueShow;
    public void OnDialogueShow(string whosTalking, string sentence)
    {
        onDialogueShow?.Invoke(whosTalking, sentence);
    }

    public event Action onDialogueStart;
    public void OnDialogueStart()
    {
        onDialogueStart?.Invoke();
    }

    public event Action onDialogueEnd;
    public void OnDialogueEnd()
    {
        onDialogueEnd?.Invoke();
    }
}