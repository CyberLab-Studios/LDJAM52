using CyberLabStudios.Game.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UtilityGameObject : MonoBehaviour
{
    public void PlaySound(AudioClip clip)
    {
        Utility.PlayOneShotAudio(gameObject, clip, .25f);
    }

}
