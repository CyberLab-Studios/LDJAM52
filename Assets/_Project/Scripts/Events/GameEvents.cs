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

    public event Action<ScytheData> onEquipScythe;
    public void OnEquipScythe(ScytheData scytheData)
    {
        onEquipScythe?.Invoke(scytheData);
    }

    public event Action<float> onAttack;
    public void OnAttack(float time)
    {
        onAttack?.Invoke(time);
    }

    public event Action<Vector3> onLookAt;
    public void OnLookAt(Vector3 target)
    {
        onLookAt?.Invoke(target);
    }
}
