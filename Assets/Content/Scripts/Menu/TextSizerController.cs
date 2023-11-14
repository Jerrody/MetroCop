using TMPro;
using UnityEngine;

namespace UI
{
    public sealed class TextSizerController : MonoBehaviour
    {
        [Header("Info")] [SerializeField] private float minSize;
        [SerializeField] private float maxSize;

        private TMP_Text _text;

        private void Awake()
        {
            _text = GetComponent<TMP_Text>();
            _text.fontSize = minSize;
        }

        private void Update()
        {
            _text.fontSize = Mathf.Lerp(minSize, maxSize, Mathf.PingPong(Time.time, 1.0f));
        }
    }
}