using UnityEngine;

namespace Road
{
    public sealed class RoadManager : MonoBehaviour
    {
        [SerializeField] private Chunk[] chunks;

        private void Awake()
        {
            chunks = GetComponentsInChildren<Chunk>(true);
        }

        private void Start()
        {
            
        }

        private void Update()
        {
            
        }
    }
}