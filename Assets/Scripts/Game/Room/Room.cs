using UnityEngine;
using static CGJ2023.Enums;

namespace CGJ2023
{
	public class Room : MonoBehaviour
	{
		void Start()
		{
			themeColor = BallColor.Red;
			Score = 0;
			ThemeColor = 0;

			HasChanges = false;
		}

		void Update()
		{
		}

		public BallColor ThemeColor
		{
			get => themeColor;
			private set
			{
				if (themeColor != value)
				{
					themeColor = value;
					HasChanges = true;
				}
			}
		}

		BallColor themeColor;

		public int Score
		{
			get => score;
			private set
			{
				if (score != value)
				{
					score = value;
					HasChanges = true;
				}
			}
		}

		int score;

		public int ThemeScore
		{
			get => themeScore;
			private set
			{
				if (themeScore != value)
				{
					themeScore = value;
					HasChanges = true;
				}
			}
		}

		int themeScore;

		public bool HasChanges { get; private set; }

		public void ClearChanges()
		{
			HasChanges = false;
		}
	}
}
