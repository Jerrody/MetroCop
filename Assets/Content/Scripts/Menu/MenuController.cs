using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI
{
    public sealed class MenuController : MonoBehaviour
    {
        private const string SoundKey = "Sound";
        private const string MusicKey = "Music";
        public const string ScoreKey = "Menu";

        public static float EffectsValue;
        private static float _musicValue;
        private static int _scoreValue;

        [Header("References")] [SerializeField]
        private AudioSource musicSource;

        [Header("Sliders")] [SerializeField] private Slider effectsSlider;
        [SerializeField] private Slider musicSlider;

        [Header("TMP_Text")] [SerializeField] private TMP_Text scoreText;

        [Header("Images")] [SerializeField] private Image background;

        [Header("Buttons")] [SerializeField] private Button playButton;
        [SerializeField] private Button exitButton;

        [Header("Camera")] [SerializeField] private Vector3 cameraEndPosition;
        [SerializeField] private float speed = 0.2f;

        private Camera _camera;

        private Vector3 _cameraStartPosition;
        private Vector3 _currentCameraVelocity;

        private void Awake()
        {
            _camera = FindFirstObjectByType<Camera>();
            _cameraStartPosition = _camera.transform.position;

            playButton.onClick.AddListener(OnPlay);
            exitButton.onClick.AddListener(OnExit);

            if (!PlayerPrefs.HasKey(SoundKey))
            {
                SetKey(SoundKey, 1.0f);
                SetKey(MusicKey, 1.0f);
                SetKey(ScoreKey, 0);
            }

            EffectsValue = PlayerPrefs.GetFloat(SoundKey);
            _musicValue = PlayerPrefs.GetFloat(MusicKey);
            _scoreValue = PlayerPrefs.GetInt(ScoreKey);

            musicSlider.value = _musicValue;
            effectsSlider.value = EffectsValue;
            scoreText.text = $"Score: {_scoreValue.ToString()}";
            SetMusicSource();
        }

        private void Start()
        {
            background.DOFade(default, 1.0f);
        }

        private void Update()
        {
            var cameraTransform = _camera.transform;
            cameraTransform.position =
                Vector3.Lerp(cameraTransform.position, cameraEndPosition, speed * Time.deltaTime);

            if (Vector3.Distance(cameraTransform.position, cameraEndPosition) < 10.0f)
            {
                enabled = false;

                cameraTransform.position = cameraEndPosition;
                background.enabled = true;
                background.DOFade(1.0f, 1.0f).OnComplete(() =>
                {
                    background.DOFade(default, 1.0f);
                    _camera.transform.position = _cameraStartPosition;
                    enabled = true;
                });
            }
        }

        private void OnPlay()
        {
            SceneManager.LoadScene(Scenes.Loading);
        }

        public void OnEffectsSlider(float value)
        {
            EffectsValue = value;
            SetKey(SoundKey, value);
        }

        public void OnMusicSlider(float value)
        {
            _musicValue = value;
            SetKey(MusicKey, value);

            SetMusicSource();
        }

        private void OnExit()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }

        private void SetKey<T>(string key, T value)
        {
            SetMapValue(key, value);
        }

        private void SetMapValue<T>(string key, T value)
        {
            if (value is int)
            {
                PlayerPrefs.SetInt(key, Convert.ToInt32(value));
            }
            else if (value is float)
            {
                PlayerPrefs.SetFloat(key, Convert.ToSingle(value));
            }
        }

        private void SetMusicSource()
        {
            musicSource.volume = _musicValue;
        }
    }
}