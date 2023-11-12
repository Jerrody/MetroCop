using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI
{
    public sealed class MenuController : MonoBehaviour
    {
        [Header("Images")] [SerializeField] private Image background;

        [Header("Buttons")] [SerializeField] private Button playButton;
        [SerializeField] private Button exitButton;

        [Header("Camera")] [SerializeField] private Vector3 cameraEndPosition;
        [SerializeField] private float speed = 0.2f;

        private Camera _camera;

        private Vector3 _cameraStartPosition;
        private Vector3 _currentCameraVelocity;

        private void Awake()
        {
            _camera = GetComponentInChildren<Camera>();
            _cameraStartPosition = _camera.transform.position;

            playButton.onClick.AddListener(OnPlay);
            playButton.onClick.AddListener(OnExit);
        }

        private void Start()
        {
            background.DOFade(default, 1.0f);
        }

        private void Update()
        {
            var cameraTransform = _camera.transform;
            cameraTransform.position = Vector3.SmoothDamp(cameraTransform.position, cameraEndPosition,
                ref _currentCameraVelocity, speed * Time.deltaTime);

            if (Vector3.Distance(cameraTransform.position, cameraEndPosition) < 0.1f)
            {
                enabled = false;

                cameraTransform.position = cameraEndPosition;
                background.enabled = true;
                background.DOFade(1.0f, 1.0f).OnComplete(() =>
                {
                    background.DOFade(default, 1.0f);
                    _camera.transform.position = _cameraStartPosition;
                    enabled = true;
                });
            }
        }

        private void OnPlay()
        {
            SceneManager.LoadScene(Scenes.Loading);
        }

        private void OnExit()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}