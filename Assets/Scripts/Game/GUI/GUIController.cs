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
				UpdateThemeScore();
				UpdateCombo();
				room.ClearChanges();
			}

			UpdateTimer();
		}

		void UpdateCombo()
        {
			combo.text = room.ComboCount.ToString();
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

		void UpdateTimer()
		{
			if (timer != null)
			{
				timer.text = $"{Time.timeSinceLevelLoad:f1}s";
			}
		}

		[SerializeField]
		Image themeColor;

		[SerializeField]
		Text score;

		[SerializeField]
		Text themeScore;

		[SerializeField]
		Text timer;

		[SerializeField]
		Text combo;

		[SerializeField]
		Room room;
	}
}
