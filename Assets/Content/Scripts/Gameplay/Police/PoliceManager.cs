using System;
using System.Collections;
using Cars;
using Player;
using Ui;
using UnityEngine;

public sealed class PoliceManager : MonoBehaviour
{
    public static Action DestroyPoliceEvent;

    [Header("Prefabs")] [SerializeField] private PoliceController policeCar;

    [Header("Info")] [SerializeField] private int maxPolice;
    [SerializeField] private float speedForManhunt = 10.0f;
    [SerializeField] private Vector3[] positionsOnRoad;

    private PlayerCarController _playerCar;

    private int _destroyedPolices;
    private int _currentActivePolice;
    private int _currentManHunt;

    private void Awake()
    {
        _playerCar = FindFirstObjectByType<PlayerCarController>();

        DestroyPoliceEvent += OnDestroyPolice;

        StartCoroutine(DecreaseManHuntLevel());
        StartCoroutine(CheckPlayerManHuntStatus());
    }

    private void OnDestroy()
    {
        DestroyPoliceEvent = null;
    }

    private void OnDestroyPolice()
    {
        _destroyedPolices++;
        _currentActivePolice--;
    }

    private IEnumerator DecreaseManHuntLevel()
    {
        while (true)
        {
            yield return new WaitForSeconds(3.0f);
            _currentManHunt = Mathf.Clamp(_currentManHunt - 1, default, maxPolice);
            _destroyedPolices = default;

            CheckManHunterStatus();
        }
    }

    private IEnumerator CheckPlayerManHuntStatus()
    {
        while (true)
        {
            yield return new WaitForSeconds(2.0f);

            CheckManHunterStatus();

            if (_currentManHunt >= default(int))
            {
                SpawnPolice();
            }
        }
    }

    private void CheckManHunterStatus()
    {
        if (_playerCar.Speed > speedForManhunt && _destroyedPolices > default(int))
        {
            _currentManHunt = Mathf.Clamp(_currentManHunt + 1, default, maxPolice);
            UiController.SetStarsEvent.Invoke(_currentManHunt);
        }
    }

    private void SpawnPolice()
    {
        if (_currentActivePolice < maxPolice)
        {
            var police = Instantiate(policeCar, transform);
            _currentActivePolice++;

            foreach (var positionOnRoad in positionsOnRoad)
            {
                var ray = new Ray(positionOnRoad + new Vector3(default, 5.0f, default), Vector3.down);
                var isHit = Physics.Raycast(ray, 10_000f, 1 << Layers.PoliceCar);
                if (!isHit)
                {
                    police.transform.position = positionOnRoad;
                    break;
                }
            }
        }
    }
}