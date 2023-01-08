using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable, CreateAssetMenu(fileName = "Audio Data" , menuName = "Scriptable Objects/New Audio Data")]
public class AudioData : ScriptableObject
{
    public AudioClip clip;
    public float volume;
}