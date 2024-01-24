using UI;
using UnityEngine;

namespace Audio
{
    public sealed class AudioSourceController : MonoBehaviour
    {
        private AudioSource _audioSource;
        
        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
        }

        public void PlaySound(AudioClip audioClip)
        {
            _audioSource.volume = MenuController.EffectsValue;
            _audioSource.clip = audioClip;
            _audioSource.Play();
        }
    }
}