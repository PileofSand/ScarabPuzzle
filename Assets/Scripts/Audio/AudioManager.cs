using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ScarabPuzzle.Audio
{
    public class AudioManager : Singleton<AudioManager>
    {
        [SerializeField]
        private AudioSource _backgroundMusic;
        [SerializeField]
        private GameObject _audioInstance;
        [SerializeField]
        private int _numberOfInstances = 10;

        private void Start()
        {
            ObjectPooling.Preload(_audioInstance, _numberOfInstances);
            _backgroundMusic.Play();
        }

        public void PlayClip(AudioClip audioClip)
        {
            AudioInstance instance = ObjectPooling.Spawn(_audioInstance, transform.position, Quaternion.identity).GetComponent<AudioInstance>();
            instance.PlayClip(audioClip);
            StartCoroutine(AfterPlayed(instance));
        }

        private IEnumerator AfterPlayed(AudioInstance instance)
        {
            yield return new WaitWhile(() => instance.IsPlaying());
            ObjectPooling.Despawn(instance.gameObject);
        }

    }
}