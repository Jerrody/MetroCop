using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Global
{
    public sealed class LoadingController : MonoBehaviour
    {
        public static int TargetScene = Scenes.Game;

        [Header("Images")] [SerializeField] private Image loadingBar;

        private void Start()
        {
            StartCoroutine(LoadScene());
        }

        private IEnumerator LoadScene()
        {
            var operation = SceneManager.LoadSceneAsync(TargetScene);

            while (!operation.isDone)
            {
                loadingBar.fillAmount = operation.progress;

                yield return null;
            }
        }
    }
}