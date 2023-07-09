using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace CGJ2023
{
	public class TitleScene : MonoBehaviour
	{
		void OnEnable()
		{
			SceneManager.sceneLoaded += OnSceneLoaded;
		}

		void OnDisable()
		{
			SceneManager.sceneLoaded -= OnSceneLoaded;
		}

		private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
		{
			StartCoroutine(FadeIn(0.5f));
		}

		IEnumerator FadeIn(float time)
		{
			if (fadeImage != null)
			{
				var fadeColor = fadeImage.color;
				var startTime = Time.time;
				while (Time.time - startTime < time)
				{
					fadeImage.color = new Color(fadeColor.r, fadeColor.g, fadeColor.b, 1f - (Time.time - startTime) / time);
					yield return null;
				}
			}
		}

		IEnumerator FadeOutAndLoadScene(float time)
		{
			if (fadeImage != null)
			{
				var fadeColor = fadeImage.color;
				var startTime = Time.time;
				while (Time.time - startTime < time)
				{
					fadeImage.color = new Color(fadeColor.r, fadeColor.g, fadeColor.b, (Time.time - startTime) / time);
					yield return null;
				}
			}

			SceneManager.LoadScene("GameScene");
		}

		public void StartGame()
		{
			StartCoroutine(FadeOutAndLoadScene(0.25f));
		}

		public void EndGame()
		{
#if UNITY_EDITOR
			UnityEditor.EditorApplication.isPlaying = false;
#else
			Application.Quit();
#endif
		}

		[SerializeField]
		Image fadeImage;
	}
}
