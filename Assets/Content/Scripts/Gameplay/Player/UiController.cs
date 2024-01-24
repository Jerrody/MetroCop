using System;
using System.Collections;
using Player;
using TMPro;
using UnityEngine;
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
        public static Action<int> AddScoreEvent;

        [Header("References")] [SerializeField]
        private RectTransform gameplay;

        [SerializeField] private RectTransform escapeMenu;
        [SerializeField] private RectTransform deathScreen;
        [SerializeField] private RectTransform manhuntRoot;

        [Header("Info")] [SerializeField] private ScoreModifier[] scoreModifiers;
        [SerializeField] private int scoreAddition;

        [Header("TMP_Texts")] [SerializeField] private TMP_Text speedText;
        [SerializeField] private TMP_Text scoreText;

        [Header("Buttons")] [SerializeField] private Button exitButton;

        [Header("Images")] [SerializeField] private Image healthBar;

        private PlayerCarController _player;

        private Image[] _stars;

        private int _score;

        private void Awake()
        {
            _player = FindFirstObjectByType<PlayerCarController>();
            PlayerDeathEvent += OnPlayerDeath;

            _stars = manhuntRoot.GetComponentsInChildren<Image>(true);

            SetStarsEvent += OnSetStars;
        }

        private void Start()
        {
            gameplay.gameObject.SetActive(true);
            escapeMenu.gameObject.SetActive(false);
            deathScreen.gameObject.SetActive(false);

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
            gameplay.gameObject.SetActive(false);
            escapeMenu.gameObject.SetActive(false);
            deathScreen.gameObject.SetActive(true);
        }

        private void OnSetStars(int count)
        {
            foreach (var star in _stars)
            {
                star.gameObject.SetActive(false);
            }

            for (int i = default; i < count; i++)
            {
                _stars[i].gameObject.SetActive(true);
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

        // TODO: Change to Exit To Menu
        private void OnClickExit()
        {
            Application.Quit();
        }
    }
}