using Player;
using Ui;
using UnityEngine;

namespace Gameplay
{
    public sealed class Gamemode : MonoBehaviour
    {
        private CarController _player;
        private UiController _ui;

        private void Awake()
        {
            _player = FindObjectOfType<CarController>();
            _ui = FindObjectOfType<UiController>();
        }

        private void Start()
        {
            _ui.AssignPlayer(_player);
        }
    }
}