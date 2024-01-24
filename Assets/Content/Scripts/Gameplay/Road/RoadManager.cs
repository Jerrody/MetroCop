using System;
using System.Linq;
using UnityEngine;

namespace Road
{
    public sealed class RoadManager : MonoBehaviour
    {
        public static Action MoveCarEvent;

        private RoadController[] _roads;

        private int _currentActiveRoadIndex;
        private int _currentLastRoadIndex;

        private void Awake()
        {
            _roads = FindObjectsOfType<RoadController>(true);
            _roads.First().IsActiveRoad = true;
            _currentLastRoadIndex = _roads.Length - 1;

            MoveCarEvent += OnMoveCar;
        }

        private void OnDestroy()
        {
            MoveCarEvent = null;
        }

        private void OnMoveCar()
        {
            _roads[_currentActiveRoadIndex].IsActiveRoad = false;
            _currentLastRoadIndex = _currentActiveRoadIndex;

            _currentActiveRoadIndex = (_currentActiveRoadIndex + 1) % _roads.Length;
            _roads[_currentActiveRoadIndex].IsActiveRoad = true;

            _roads[_currentLastRoadIndex].transform.position = _roads[_currentActiveRoadIndex].transform.position +
                                                               Vector3.up * _roads[_currentActiveRoadIndex].length;
        }
    }
}