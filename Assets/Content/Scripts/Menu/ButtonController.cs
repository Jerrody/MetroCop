using Audio;
using UnityEngine;
using UnityEngine.UI;

namespace Ui
{
    public sealed class ButtonController : MonoBehaviour
    {
        [Header("References")] [SerializeField]
        private AudioSourceController audioSourceController;

        [Header("Info")] [SerializeField] private AudioClip clickSound;

        private void Awake()
        {
            var button = GetComponent<Button>();
            button.onClick.AddListener(OnClick);
        }

        private void OnClick()
        {
            audioSourceController.PlaySound(clickSound);
        }
    }
}