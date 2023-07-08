using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using static CGJ2023.Enums;

namespace CGJ2023
{
	public class Room : MonoBehaviour
	{
        private static readonly System.Random random = new System.Random();

		#region create balls
		GameObject PlayerBall;

		const int birthRate = 5;
		const int initBirthCount = 10;
		const int birthRateChangeDT = 30;
		const int birthCountChangeDelta = 10;
		int birthTimes = 0;
		float spendTime = 0;
		const float Top = 4.4f;
		const float Bottom = -4.4f;
		const float Left = -7.3f;
		const float Right = 7.3f;
		const float BallRadius = 0.25f;
		const float RandomRange = 0.15f;
		int PositionsPerLine = Mathf.FloorToInt((Right - Left) / ((BallRadius + RandomRange) * 2));
		int positionsPerColumn = Mathf.FloorToInt((Top - Bottom) / ((BallRadius + RandomRange) * 2));
		List<GameObject> availableIndicators = new List<GameObject>();
		#endregion

		void Start()
		{
			ThemeColor = BallColor.Red;
			Score = 0;
			ThemeScore = 0;

			InitAvailablePositions();
			CreatePlayerBall();
		}

		void Update()
		{
			if (Input.GetKeyDown(KeyCode.Escape))
			{
				SceneManager.LoadScene("ResultScene");
			}
		}

        void FixedUpdate()
        {
			TryCreateBalls();
        }

		#region create balls

		void CreatePlayerBall()
        {
			var collentableBallPrefab = AssetDatabase.LoadAssetAtPath("Assets/Prefabs/Player.prefab", typeof(GameObject)) as GameObject;
			PlayerBall = GameObject.Instantiate(collentableBallPrefab, new Vector2(0, 0), Quaternion.identity);
		}

		void InitAvailablePositions()
        {
			var availableIndicator = AssetDatabase.LoadAssetAtPath("Assets/Prefabs/AvailableIndicator.prefab", typeof(GameObject)) as GameObject;

			for (var i = 0; i < PositionsPerLine; i++)
            {
				for (var j = 0; j < positionsPerColumn; j++)
                {
					var position = GetRandomPositionByIndex(i * PositionsPerLine + j, false);
					var indicator = GameObject.Instantiate(availableIndicator, position, Quaternion.identity);
					indicator.name = string.Format("Indicator_{0}_{1}", i, j);
					indicator.transform.localScale = new Vector3((BallRadius + RandomRange) * 2, (BallRadius + RandomRange) * 2, 1);
					indicator.GetComponent<AvailableIndicator>().Index = i * PositionsPerLine + j;
					availableIndicators.Add(indicator);
				}
            }
        }

		void TryCreateBalls()
        {
			spendTime += Time.deltaTime;
			if (birthTimes < (spendTime / birthRate))
            {
				DoCreateBalls();
            }
		}
		
		List<int> GetAvailablePositions()
        {
			List<int> availables = new List<int>();
			availableIndicators.ForEach(indicator =>
			{
				if (indicator.GetComponent<AvailableIndicator>().IsAvailable)
				{
					availables.Add(indicator.GetComponent<AvailableIndicator>().Index);
				}
			});
			return availables;
        }

		void DoCreateBalls()
        {
			birthTimes += 1;
			var availables = GetAvailablePositions();
			if (availables.Count == 0)
            {
				Debug.Log("Game Over!");
				return;
            }
			for (var i=0; i<CurrentBirthRate && availables.Count>0; i++)
            {
				var index = availables.ElementAt<int>(random.Next(availables.Count));
				availables.Remove(index);
				DoCreateOneBall(index);
            }
        }

		GameObject DoCreateOneBall(int index)
        {
			var position = GetRandomPositionByIndex(index, true);
			var collentableBallPrefab = AssetDatabase.LoadAssetAtPath("Assets/Prefabs/Ball.prefab", typeof(GameObject)) as GameObject;
			var ball = GameObject.Instantiate(collentableBallPrefab, position, Quaternion.identity);
			if (Random.Range(0.0f, 1.0f) > 0.6)
            {
				switch (ThemeColor)
                {
					case BallColor.Red:
						if (Random.Range(0.0f, 1.0f) > 0.5f)
                        {
							ball.GetComponent<CollectableBall>().BallColor = BallColor.Blue;
						}
                        else
                        {
							ball.GetComponent<CollectableBall>().BallColor = BallColor.Green;
						}
						break;
					case BallColor.Green:
						if (Random.Range(0.0f, 1.0f) > 0.5f)
						{
							ball.GetComponent<CollectableBall>().BallColor = BallColor.Blue;
						}
						else
						{
							ball.GetComponent<CollectableBall>().BallColor = BallColor.Red;
						}
						break;
					case BallColor.Blue:
						if (Random.Range(0.0f, 1.0f) > 0.5f)
						{
							ball.GetComponent<CollectableBall>().BallColor = BallColor.Red;
						}
						else
						{
							ball.GetComponent<CollectableBall>().BallColor = BallColor.Green;
						}
						break;
                }
            }
            else
            {
				ball.GetComponent<CollectableBall>().BallColor = ThemeColor;
			}
			return ball;
		}

		Vector2 GetRandomPositionByIndex(int index, bool random)
        {
			int line = index % PositionsPerLine;
			int cloumn = Mathf.FloorToInt(index / PositionsPerLine);
			var centerX = (BallRadius + RandomRange) * 2 * cloumn + (BallRadius + RandomRange) + Left;
			var centerY = (BallRadius + RandomRange) * 2 * line + (BallRadius + RandomRange) + Bottom;
			if (random)
            {
				return new Vector2(centerX + Random.Range(-RandomRange, RandomRange), centerY + Random.Range(-RandomRange, RandomRange));
			}
            else
            {
				return new Vector2(centerX, centerY);
            }
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
