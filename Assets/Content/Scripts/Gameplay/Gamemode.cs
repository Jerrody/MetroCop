using System.Collections;
using System.Collections.Generic;
using Player;
using Ui;
using UnityEngine;

namespace Gameplay
{
    public sealed class Gamemode : MonoBehaviour
    {
        [Header("Prefabs")] [SerializeField] private GameObject policePrefab;

        [Header("Info")] [SerializeField] private int maxActivePolice = 2;
        [SerializeField] private Vector3[] positionsOnRoad;

        private PlayerCarController _player;
        private UiController _ui;

        private HashSet<int> _activeRoads = new();

        private int _currentActivePolice;

        private void Awake()
        {
            _player = FindFirstObjectByType<PlayerCarController>();
            _ui = FindFirstObjectByType<UiController>();
        }

        private void Start()
        {
            _ui.AssignPlayer(_player);
        }

        private void OnChangedPosition(int roadIndex)
        {
        }
    }
}