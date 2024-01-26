using System;
using System.Collections;
using Cars;
using Player;
using Ui;
using UnityEngine;
using Random = UnityEngine.Random;

public sealed class PoliceManager : MonoBehaviour
{
    public static Action DestroyPoliceEvent;

    [Header("Prefabs")] [SerializeField] private PoliceController policeCar;

    [Header("Info")] [SerializeField] private int maxPolice;
    [SerializeField] private int damage;
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

    private void Start()
    {
        StartCoroutine(DamagePlayer());
    }

    private void OnDestroy()
    {
        DestroyPoliceEvent = null;
    }

    private void OnDestroyPolice()
    {
        _destroyedPolices++;
        _currentActivePolice = Mathf.Clamp(_currentActivePolice - 1, default, int.MaxValue);
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
            yield return new WaitForSeconds(4.0f);

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
            _currentManHunt = Mathf.Clamp(_currentManHunt + 1, default, int.MaxValue);
            UiController.SetStarsEvent.Invoke(_currentManHunt);
        }
    }

    private void SpawnPolice()
    {
        if (_currentActivePolice < maxPolice && Random.Range(0, 1) == default)
        {
            var xPosition = _playerCar.transform.position.x;
            foreach (var positionOnRoad in positionsOnRoad)
            {
                var random = new System.Random();
                if (random.NextDouble() >= 0.5f)
                {
                    continue;
                }

                var currentPositionOnRoad = positionOnRoad;
                currentPositionOnRoad.x = xPosition;
                var ray = new Ray(currentPositionOnRoad + new Vector3(default, 5.0f, default), Vector3.down);
                var isHit = Physics.Raycast(ray, 10_000f, 1 << Layers.PoliceCar);
                if (!isHit)
                {
                    var police = Instantiate(policeCar, transform);
                    _currentActivePolice++;

                    police.transform.position = currentPositionOnRoad;
                    break;
                }
            }
        }
    }

    private IEnumerator DamagePlayer()
    {
        while (true)
        {
            yield return new WaitForSeconds(1.0f);

            var damageToPlayer = damage * _currentActivePolice;
            _playerCar.TakeDamage(damageToPlayer);
        }
    }
}