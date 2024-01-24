using UnityEngine;

namespace Road
{
    public sealed class CanisterController : MonoBehaviour
    {
        [field: Header("Info")]
        [field: SerializeField]
        public int healAmount { get; private set; } = 30;
    }
}