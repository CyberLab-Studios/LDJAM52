using CyberLabStudios.Game.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UtilityGameObject : MonoBehaviour
{
    public void PlaySound(AudioData clip)
    {
        Utility.PlayOneShotAudio(gameObject, clip.clip, clip.volume);
    }
}