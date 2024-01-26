using System;
using System.Collections;
using Cars;
using Player;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Ui
{
    [Serializable]
    public struct ScoreModifier
    {
        public float speed;
        public int modifier;
    }

    public sealed class UiController : MonoBehaviour
    {
        public static Action<int> SetStarsEvent;
        public static Action PlayerDeathEvent;
        public static Action<int> AddScoresEvent;

        [Header("References")] [SerializeField]
        private RectTransform gameplay;

        [SerializeField] private RectTransform escapeMenu;
        [SerializeField] private RectTransform deathScreen;
        [SerializeField] private RectTransform manhuntRoot;

        [Header("Info")] [SerializeField] private ScoreModifier[] scoreModifiers;

        [Header("TMP_Texts")] [SerializeField] private TMP_Text speedText;
        [SerializeField] private TMP_Text scoreText;
        [SerializeField] private TMP_Text endScreenScoreText;

        [Header("Images")] [SerializeField] private Image healthBar;
        [SerializeField] private Image[] stars;

        private PlayerCarController _player;


        private int _score;

        private void Awake()
        {
            _player = FindFirstObjectByType<PlayerCarController>();

            stars = manhuntRoot.GetComponentsInChildren<Image>(true);

            PlayerDeathEvent += OnPlayerDeath;
            SetStarsEvent += OnSetStars;
            AddScoresEvent += OnAddScore;
        }

        private void Start()
        {
            gameplay.gameObject.SetActive(true);
            deathScreen.gameObject.SetActive(false);

            PlayerPrefs.SetInt(MenuController.ScoreKey, _score);
            StartCoroutine(CheckScore());
        }

        private void OnDestroy()
        {
            PlayerDeathEvent = null;
        }

        private void Update()
        {
            healthBar.fillAmount = _player.HealthPercentage;
            speedText.text = $"{_player.speed:0.0} km/h";
        }

        public void AssignPlayer(PlayerCarController player)
        {
            enabled = true;
            _player = player;
        }

        private void OnPlayerDeath()
        {
            enabled = false;

            gameplay.gameObject.SetActive(false);
            deathScreen.gameObject.SetActive(true);
            
            scoreText.gameObject.SetActive(false);

            _player.enabled = false;
            var policeManager = FindObjectOfType<PoliceManager>();
            policeManager.enabled = false;
            var polices = FindObjectsOfType<PoliceController>();
            foreach (var police in polices)
            {
                Destroy(police.gameObject);
            }

            endScreenScoreText.text = $"Score: {_score}";
        }

        private void OnSetStars(int count)
        {
            foreach (var star in stars)
            {
                star.gameObject.SetActive(false);
            }

            for (int i = default; i < count; i++)
            {
                stars[i].gameObject.SetActive(true);
            }
        }

        private IEnumerator CheckScore()
        {
            while (true)
            {
                yield return new WaitForSeconds(1.0f);

                var speed = _player.speed;

                for (int i = default; i < scoreModifiers.Length; i++)
                {
                    if (speed > scoreModifiers[i].speed)
                    {
                        OnAddScore(scoreModifiers[i].modifier);
                        break;
                    }
                }
            }
        }

        private void OnAddScore(int scoreAmount)
        {
            _score += scoreAmount;
            SetScore();
        }

        private void SetScore()
        {
            scoreText.text = $"Score: {_score.ToString()}";
        }

        public void OnClickExit()
        {
            SceneManager.LoadScene(Scenes.Menu);
        }
    }
}