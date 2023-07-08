using UnityEngine;
using UnityEngine.UI;
using static CGJ2023.Enums;

namespace CGJ2023
{
	public class GUIController : MonoBehaviour
	{
		void Start()
		{
		}

		void Update()
		{
			if (room != null && room.HasChanges)
			{
				UpdateThemeColor();
				UpdateScore();

				room.ClearChanges();
			}
		}

		void UpdateThemeColor()
		{
			if (themeColor != null)
			{
				switch (room.ThemeColor)
				{
					case BallColor.Red:
						themeColor.color = Color.red;
						break;
					case BallColor.Green:
						themeColor.color = Color.green;
						break;
					case BallColor.Blue:
						themeColor.color = Color.blue;
						break;
				}
			}
		}

		void UpdateScore()
		{
			if (score != null)
			{
				score.text = room.Score.ToString();
			}
		}

		void UpdateThemeScore()
		{
			if (themeScore != null)
			{
				themeScore.text = room.ThemeScore.ToString();
			}
		}

		[SerializeField]
		Image themeColor;

		[SerializeField]
		Text score;

		[SerializeField]
		Text themeScore;

		[SerializeField]
		Room room;
	}
}
