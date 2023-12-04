using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Global
{
    [Serializable]
    public enum AudioDimension
    {
        Three,
        Two
    }

    [Serializable]
    public struct Audio
    {
        [field: SerializeField] public AudioType audioType { get; private set; }
        [field: SerializeField] public AudioClip audioClip { get; private set; }
        [field: SerializeField] public float maxDistance { get; private set; }
        [SerializeField] private AudioDimension audioDimension;

        public float GetSpatialBlend()
        {
            var value = audioDimension switch
            {
                AudioDimension.Two => default,
                AudioDimension.Three => 1.0f,
                _ => throw new ArgumentOutOfRangeException()
            };

            return value;
        }
    }

    public sealed class AudioManager : MonoBehaviour
    {
        public static AudioManager instance { get; private set; }

        [Header("References")] [SerializeField]
        private AudioSource audioSourceToCreate;

        [SerializeField] private AudioSource musicSource;
        [SerializeField] private List<AudioClip> musicSoundtracks;

        [Header("Info")] [SerializeField] private List<Audio> audios;

        private float _musicVolume = 1.0f;
        private float _soundEffectsVolume = 1.0f;

        private List<AudioSource> _soundEffects;

        private Dictionary<int, AudioSource> _loopingSources;

        private void Awake()
        {
            instance = this;

            _soundEffects = GetComponentsInChildren<AudioSource>(true).ToList();

            _loopingSources = new(_soundEffects.Count);
        }

        private void OnDestroy()
        {
            instance = null;
        }

        private void Start()
        {
            musicSource.volume = _musicVolume;
            _soundEffects.ForEach(soundEffect => soundEffect.gameObject.SetActive(false));
        }

        public void SetMusicVolume(float volume)
        {
            volume = Mathf.Clamp01(volume);
            _musicVolume = volume;

            musicSource.volume = _musicVolume;
        }

        public void SetSoundEffectsVolume(float volume)
        {
            volume = Mathf.Clamp01(volume);
            _soundEffectsVolume = volume;

            _soundEffects.ForEach(soundEffect => soundEffect.volume = _soundEffectsVolume);
        }

        private AudioSource CreateNewAudioSource()
        {
            var audioSource = Instantiate(audioSourceToCreate);
            audioSource.gameObject.SetActive(false);
            _soundEffects.Add(audioSource);

            return audioSource;
        }

        public AudioSource PlayClipAtPoint(Transform parentTransform, Audio audio, bool loop = false)
        {
            var source = GetAvailableAudioSource();
            if (source == null)
            {
                source = CreateNewAudioSource();
            }

            var sourceTransform = source.transform;
            sourceTransform.parent = parentTransform;
            sourceTransform.localPosition = default;
            source.clip = audio.audioClip;
            source.loop = loop;
            source.volume = _soundEffectsVolume;
            source.minDistance = audio.maxDistance / 6.5f;
            source.maxDistance = audio.maxDistance;
            source.rolloffMode = AudioRolloffMode.Linear;
            source.spatialBlend = audio.GetSpatialBlend();
            source.gameObject.SetActive(true);
            source.Play();

            if (!source.loop)
            {
                StartCoroutine(DeactivateAfterFinishedPlaying(source));
            }

            return source;
        }

        public AudioSource PlayClipAtPoint(Vector3 position, Audio audio, bool loop = false)
        {
            var source = GetAvailableAudioSource();
            if (source == null)
            {
                source = CreateNewAudioSource();
            }

            var sourceTransform = source.transform;
            sourceTransform.position = position;
            sourceTransform.parent = null;
            source.clip = audio.audioClip;
            source.loop = loop;
            source.volume = _soundEffectsVolume;
            source.minDistance = audio.maxDistance / 6.5f;
            source.maxDistance = audio.maxDistance;
            source.rolloffMode = AudioRolloffMode.Linear;
            source.spatialBlend = audio.GetSpatialBlend();
            source.gameObject.SetActive(true);
            source.Play();

            if (!source.loop)
            {
                StartCoroutine(DeactivateAfterFinishedPlaying(source));
            }

            return source;
        }

        public void StopLoopingSound(AudioSource source)
        {
            var sourceId = source.GetInstanceID();
            if (_loopingSources.All(loopSource => loopSource.Key != sourceId))
            {
                return;
            }

            source.Stop();
            source.loop = false;
            source.gameObject.SetActive(false);
            _loopingSources.Remove(sourceId);
        }

        public AudioSource PlayLoopingClipAtPoint(Transform parentTransform, Audio audio)
        {
            var source = PlayClipAtPoint(parentTransform, audio, true);
            var audioId = source.GetInstanceID();
            if (_loopingSources.ContainsKey(audioId))
            {
                return null;
            }

            _loopingSources.Add(source.GetInstanceID(), source);

            return source;
        }

        public AudioSource PlayLoopingClipAtPoint(Vector3 position, Audio audio)
        {
            var source = PlayClipAtPoint(position, audio, true);
            _loopingSources.Add(source.GetInstanceID(), source);

            return source;
        }

        private AudioSource GetAvailableAudioSource()
        {
            return _soundEffects.FirstOrDefault(source => !source.gameObject.activeInHierarchy);
        }

        private IEnumerator DeactivateAfterFinishedPlaying(AudioSource source)
        {
            yield return new WaitForSeconds(source.clip.length);

            source.gameObject.SetActive(false);
            source.transform.parent = null;
        }
    }
}