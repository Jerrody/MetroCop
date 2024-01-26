using UnityEngine;
using Road;

public sealed class RoadController : MonoBehaviour
{
    [Header("Prefabs")] [SerializeField] private CanisterController canisterPrefab;

    [Header("References")] [SerializeField]
    private Transform[] canisterSpawnPoints;

    [field: Header("Road Settings")]
    [field: SerializeField]
    public float length { get; private set; }

    private CanisterController _canister;

    private void Start()
    {
        SpawnCanister();
    }
    
    public void MoveForward(Vector3 newPosition)
    {
        transform.position = newPosition;
        SpawnCanister();
    }

    private void SpawnCanister()
    {
        if (_canister)
        {
            Destroy(_canister.gameObject);
        }

        var spawnIndex = UnityEngine.Random.Range(0, canisterSpawnPoints.Length);
        var spawnPoint = canisterSpawnPoints[spawnIndex];
        _canister = Instantiate(canisterPrefab, spawnPoint.position + new Vector3(0, 0.5f, 0),
            Quaternion.Euler(0, UnityEngine.Random.Range(0, 360), 0));
    }
}