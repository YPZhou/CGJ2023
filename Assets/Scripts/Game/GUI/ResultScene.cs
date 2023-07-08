using UnityEngine;
using UnityEngine.SceneManagement;

namespace CGJ2023
{
	public class ResultScene : MonoBehaviour
	{
		public void BackToTitle()
		{
			SceneManager.LoadScene("TitleScene");
		}
	}
}
