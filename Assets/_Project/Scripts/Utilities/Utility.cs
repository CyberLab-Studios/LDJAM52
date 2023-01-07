using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;

namespace CyberLabStudios.Game.Utilities
{
    public static class Utility
    {
        public static void PlayOneShotAudio(this GameObject obj, AudioClip clip, float volume = 1f, AudioMixerGroup mixerGroup = null)
        {
            var source = obj.AddComponent<AudioSource>();
            source.PlayOneShot(clip, volume);
            source.outputAudioMixerGroup = mixerGroup;
            Object.Destroy(source, clip.length);
        }


        public static void PlayRandomOneShotAudio(this GameObject obj, List<AudioClip> clips, float volume = 1f, AudioMixerGroup mixerGroup = null)
        {
            var clip = clips[Random.Range(0, clips.Count)];
            PlayOneShotAudio(obj, clip, volume, mixerGroup);
        }

        public static void PlayOneShotParticle(this ParticleSystem particle, Vector3 position)
        {
            var part = GameObject.Instantiate(particle.gameObject, position, Quaternion.identity);
            GameObject.Destroy(part, particle.main.startLifetimeMultiplier);
        }

        public static void Swap<T>(ref T left, ref T right)
        {
            T temp;
            temp = left;
            left = right;
            right = temp;
        }

        public static bool IsFilled(this string str) => !str.IsNullOrEmpty() && !str.IsNullOrWhiteSpace();
        public static bool IsNullOrEmpty(this string str) => string.IsNullOrEmpty(str);
        public static bool IsNullOrWhiteSpace(this string str) => string.IsNullOrWhiteSpace(str);
        public static bool InRange(this float value, float minValue, float maxValue) => value >= minValue && value <= maxValue;
    }
}