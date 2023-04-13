using System;
using System.Collections;
using UnityEngine;

namespace ScarabPuzzle.Audio
{
    public class AudioInstance : MonoBehaviour
    {
        public Action OnFinishPlaying;

        private AudioSource _audioSource;

        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
        }

        public void PlayClip(AudioClip clip)
        {
            _audioSource.PlayOneShot(clip);
        }

        public bool IsPlaying()
        {
            return _audioSource.isPlaying;
        }
    }
}