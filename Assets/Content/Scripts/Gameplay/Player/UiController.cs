using System;
using Player;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Ui
{
    public sealed class UiController : MonoBehaviour
    {
        public static Action PlayerDeathEvent;

        [Header("References")]
        [SerializeField] private RectTransform gameplay;
        [SerializeField] private RectTransform escapeMenu;
        [SerializeField] private RectTransform deathScreen;
        
        [Header("TMP_Texts")] [SerializeField] private TMP_Text speedText;
        [Header("Images")] [SerializeField] private Image healthBar;

        private PlayerCarController _player;

        private void Awake()
        {
            enabled = false;

            PlayerDeathEvent += OnPlayerDeath;
        }

        private void Start()
        {
            gameplay.gameObject.SetActive(true);
            escapeMenu.gameObject.SetActive(false);
            deathScreen.gameObject.SetActive(false);
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
    }
}