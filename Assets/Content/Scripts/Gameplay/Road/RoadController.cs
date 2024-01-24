using System;
using Player;
using Road;
using UnityEngine;
using Random = UnityEngine.Random;

public sealed class RoadController : MonoBehaviour
{
    [Header("Prefabs")] [SerializeField] private CanisterController canisterPrefab;

    [Header("References")] [SerializeField]
    private Transform[] canisterSpawnPoints;

    [field: Header("Road Settings")]
    [field: SerializeField]
    public float length { get; private set; }

    private PlayerCarController _playerCar;

    [NonSerialized] public bool IsActiveRoad;

    private CanisterController _canister;

    private void Start()
    {
        _playerCar = FindFirstObjectByType<PlayerCarController>();
        
        SpawnCanister();
    }

    private void Update()
    {
        /*if (IsActiveRoad && Vector3.Distance(_playerCar.transform.position, transform.position) > length * 1.5f)
        {
            RoadManager.MoveCarEvent.Invoke();
            SpawnCanister();
        }*/
    }

    private void SpawnCanister()
    {
        if (_canister)
        {
            Destroy(_canister.gameObject);
        }

        var spawnPoint = canisterSpawnPoints[Random.Range(default, canisterSpawnPoints.Length)];
        _canister = Instantiate(canisterPrefab, spawnPoint.position + new Vector3(default, 0.5f, default),
            Quaternion.Euler(new(default, Random.Range(default, 360.0f), default)));
    }
}