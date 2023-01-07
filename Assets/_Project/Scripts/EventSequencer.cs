using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventSequencer : MonoBehaviour
{
    public List<SequenceElement> sequence = new List<SequenceElement>();
    public bool playOnAwake = true;

    // Start is called before the first frame update
    void Start()
    {
        if (playOnAwake)
        {
            StartSequence();
        }
    }

    public void StartSequence()
    {
        StartCoroutine(PlaySequence());
    }

    IEnumerator PlaySequence()
    {
        foreach (var element in sequence)
        {
            yield return new WaitForSeconds(element.timeBeforeInvoke);
            element.eventToInvoke.Invoke();
            yield return new WaitForSeconds(element.timeAfterInvoke);
        }
    }
}

[Serializable]
public class SequenceElement
{
    public float timeBeforeInvoke;
    public UnityEvent eventToInvoke;
    public float timeAfterInvoke;
}