using UnityEngine;
using UnityEngine.SceneManagement;

namespace CGJ2023
{
	public class TitleScene : MonoBehaviour
	{
		public void StartGame()
		{
			SceneManager.LoadScene("GameScene");
		}

		public void EndGame()
		{
#if UNITY_EDITOR
			UnityEditor.EditorApplication.isPlaying = false;
#else
			Application.Quit();
#endif
		}
	}
}
