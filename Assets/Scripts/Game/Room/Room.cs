using UnityEngine;
using static CGJ2023.Enums;

namespace CGJ2023
{
	public class Room : MonoBehaviour
	{

		#region create balls
		const int birthRate = 5;
		const int initBirthCount = 10;
		const int birthRateChangeDT = 30;
		const int birthCountChangeDelta = 10;
		int birthTimes = 0;
		float spendTime = 0;
		#endregion
		
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

        void FixedUpdate()
        {
			TryCreateBalls();
        }

		#region create balls
		
		void TryCreateBalls()
        {
			spendTime += Time.deltaTime;
			if (birthTimes < (spendTime / birthRate))
            {
				DoCreateBalls();
            }
		}
		
		void DoCreateBalls()
        {
			birthTimes += 1;
			Debug.Log(string.Format("Create Balls, count: {0}", CurrentBirthRate));
        }

		int CurrentBirthRate
		{
			get
			{
				return ((int)(spendTime / birthRateChangeDT)) * birthCountChangeDelta + initBirthCount;
			}
		}
			
		#endregion


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
