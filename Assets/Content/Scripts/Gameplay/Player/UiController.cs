using Player;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Ui
{
    public sealed class UiController : MonoBehaviour
    {
        [Header("TMP_Texts")] [SerializeField] private TMP_Text speedText;

        [Header("Images")] [SerializeField] private Image healthBar;

        private CarController _player;

        private void Awake()
        {
            enabled = false;
        }

        private void Update()
        {
            healthBar.fillAmount = _player.HealthPercentage;
            speedText.text = $"{_player.speed:0.0} km/h";
        }

        public void AssignPlayer(CarController player)
        {
            enabled = true;
            _player = player;
        }
    }
}