using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using static CGJ2023.Enums;

namespace CGJ2023
{
	public class Room : MonoBehaviour
	{
		public static readonly System.Random random = new System.Random();

		#region create balls
		GameObject PlayerBall;

		public List<GameObject> collectableBalls = new List<GameObject>();

		const int birthRate = 3;
		const int initBirthCount = 20;
		const int birthRateChangeDT = 10;
		const int birthCountChangeDelta = 10;
		int birthTimes = 0;
		float spendTime = 0;
		const float Top = 3.5f;
		const float Bottom = -4.7f;
		const float Left = -8.6f;
		const float Right = 8.6f;
		const float BallRadius = 0.25f;
		const float RandomRange = 0.15f;
		int PositionsPerLine = Mathf.FloorToInt((Right - Left) / ((BallRadius + RandomRange) * 2)) + 1;
		int positionsPerColumn = Mathf.FloorToInt((Top - Bottom) / ((BallRadius + RandomRange) * 2)) + 1;
		List<GameObject> availableIndicators = new List<GameObject>();

		public int BallBirthSpeedModifier = 0;
		#endregion

		#region item management
		ItemSpawner itemSpawner = null;
		public ItemSpawner ItemSpawner => itemSpawner;
		#endregion

		GUIController gUIController = null;

		void Start()
		{
			ThemeColor = BallColor.Green;
			Score = 0;
			ThemeScore = 0;

			InitAvailablePositions();
			CreatePlayerBall();

			itemSpawner = GetComponent<ItemSpawner>();
            gUIController = GameObject.Find("Canvas").GetComponent<GUIController>();

            difficultyLevel = 0;
			SetGoalByDifficulty();
        }

		void FixedUpdate()
		{
			TryCreateBalls();

			TryCreatItems();
		}

		public void FinishCurrentPush()
		{
			RemainingPushCount -= 1;
			if (RemainingPushCount == 0)
			{
				if (Score >= TargetScore)
				{
					difficultyLevel += 1;
					SetGoalByDifficulty();
				}
				else
				{
					GameOver();
				}
			}
		}

		void SetGoalByDifficulty()
		{
			RemainingPushCount += 5 + difficultyLevel;
			TargetScore += 5 + 5 * difficultyLevel;

			DoCreateBalls();
            gUIController.ShouUpNextDifficultyTips(2, TargetScore, RemainingPushCount);
        }

		void GameOver()
		{
			var gameScene = GetComponent<GameScene>();
			if (gameScene != null)
			{
				gameScene.TransitToResultScene();
			}
			else
			{
				SceneManager.LoadScene("ResultScene");
			}
		}

		int difficultyLevel;
		public int RemainingPushCount
		{
			get => remainingPushCount;
			private set
			{
				if (remainingPushCount != value)
				{
					remainingPushCount = value;
					HasChanges = true;
				}
			}
		}

		int remainingPushCount;

		public int TargetScore
		{
			get => targetScore;
			private set
			{
				if (targetScore != value)
				{
					targetScore = value;
					HasChanges = true;
				}
			}
		}

		int targetScore;

		private void TryCreatItems()
		{
			//if (itemSpawner.CanSpawnNow())
			//{
			//    var availables = GetAvailablePositions();
			//    if (availables.Count == 0)
			//    {
			//        return;
			//    }

			//    var index = availables.ElementAt<int>(random.Next(availables.Count));
			//    var pos = GetRandomPositionByIndex(index, true);
			//    itemSpawner.SpawnItem(pos);
			//    availables.Remove(index);
			//}
		}

		#region create balls

		void CreatePlayerBall()
		{
			if (playerBallPrefab != null)
			{
				PlayerBall = GameObject.Instantiate(playerBallPrefab, new Vector2(0, 0), Quaternion.identity);
				PlayerBall.name = "PlayerBall";
			}
		}

		void InitAvailablePositions()
		{
			if (indicatorPrefab != null)
			{
				for (var i = 0; i < PositionsPerLine; i++)
				{
					for (var j = 0; j < positionsPerColumn; j++)
					{
						var position = GetRandomPositionByIndex(i * PositionsPerLine + j, false);
						var indicator = GameObject.Instantiate(indicatorPrefab, position, Quaternion.identity);
						indicator.name = string.Format("Indicator_{0}_{1}", i, j);
						indicator.transform.localScale = new Vector3((BallRadius + RandomRange) * 2, (BallRadius + RandomRange) * 2, 1);
						indicator.GetComponent<AvailableIndicator>().Index = i * PositionsPerLine + j;
						availableIndicators.Add(indicator);
					}
				}
			}
		}

		void TryCreateBalls()
		{
			//spendTime += Time.deltaTime * (1+(float)BallBirthSpeedModifier/100);

			//if (birthTimes < (spendTime / birthRate))
			//{
			//    DoCreateBalls();
			//}
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
			//birthTimes += 1;
			var availables = GetAvailablePositions();
			for (var i = 0; i < CurrentBirthRate && availables.Count > 0; i++)
			{
				var index = availables.ElementAt<int>(random.Next(availables.Count));
				availables.Remove(index);
				DoCreateOneBall(index);
			}
		}

		GameObject DoCreateOneBall(int index)
		{
			var position = GetRandomPositionByIndex(index, true);

			if (collectableBallPrefab != null)
			{
				var ball = GameObject.Instantiate(collectableBallPrefab, position, Quaternion.identity);
				collectableBalls.Add(ball);
				var collectableBall = ball.GetComponent<CollectableBall>();
				collectableBall.InitializeBall();

				if (Random.Range(0.0f, 1.0f) > 0.6)
				{
					switch (ThemeColor)
					{
						case BallColor.Red:
							if (Random.Range(0.0f, 1.0f) > 0.5f)
							{
								collectableBall.BallColor = BallColor.Blue;
							}
							else
							{
								collectableBall.BallColor = BallColor.Green;
							}
							break;
						case BallColor.Green:
							if (Random.Range(0.0f, 1.0f) > 0.5f)
							{
								collectableBall.BallColor = BallColor.Blue;
							}
							else
							{
								collectableBall.BallColor = BallColor.Red;
							}
							break;
						case BallColor.Blue:
							if (Random.Range(0.0f, 1.0f) > 0.5f)
							{
								collectableBall.BallColor = BallColor.Red;
							}
							else
							{
								collectableBall.BallColor = BallColor.Green;
							}
							break;
					}
				}
				else
				{
					collectableBall.BallColor = ThemeColor;
				}

				return ball;
			}

			return null;
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

		#region Theam Score full

		void OnTheamScoreFull()
		{
			switch (ThemeColor)
			{
				case BallColor.Red:
					OnRedTheamScoreFull();
					break;
				case BallColor.Blue:
					OnBlueTheamScoreFull();
					break;
				case BallColor.Green:
					OnGreenTheamScoreFull();
					break;
			}
		}

		void OnRedTheamScoreFull()
		{
			int redNum = 0;
			int blueNum = 0;
			int greenNum = 0;
			collectableBalls.ForEach(ball =>
			{
				switch (ball.GetComponent<CollectableBall>().BallColor)
				{
					case BallColor.Red:
						redNum += 1;
						break;
					case BallColor.Blue:
						blueNum += 1;
						break;
					case BallColor.Green:
						greenNum += 1;
						break;
				}
			});
			
			PlayerBall.GetComponent<PlayerBall>()?.ClearAttachedBalls();
			while(collectableBalls.Count > 0)
			{
				var ball = collectableBalls[0];
				ball.GetComponent<CollectableBall>()?.DestroyBall();
			}

			switch (themeColor)
			{
				case BallColor.Red:
					for (var i=0; i<redNum; i++)
					{
						OnCollectBall(BallColor.Red);
					}
					redNum = 0;
					break;
				case BallColor.Blue:
					for (var i = 0; i < blueNum; i++)
					{
						OnCollectBall(BallColor.Blue);
					}
					blueNum = 0;
					break;
				case BallColor.Green:
					for (var i = 0; i < greenNum; i++)
					{
						OnCollectBall(BallColor.Green);
					}
					greenNum = 0;
					break;
			}
			for (var i = 0; i < redNum; i++)
			{
				OnCollectBall(BallColor.Red);
			}
			for (var i = 0; i < greenNum; i++)
			{
				OnCollectBall(BallColor.Green);
			}
			for (var i = 0; i < blueNum; i++)
			{
				OnCollectBall(BallColor.Blue);
			}
		}

		void OnGreenTheamScoreFull()
		{
			var availables = GetAvailablePositions();
			if (availables.Count == 0)
			{
				return;
			}

			var index = availables.ElementAt<int>(random.Next(availables.Count));
			var pos = GetRandomPositionByIndex(index, true);
			itemSpawner.SpawnItem(pos);
			availables.Remove(index);
		}

		void OnBlueTheamScoreFull()
		{
			Score *= 2;
		}
		
		#endregion

		public void OnCollectBall(BallColor color)
		{
			//if (LastCollectedBallColor == color)
			//{
			//	comboCount += 1;
			//}
			//else
			//{
			//	comboCount = 1;
			//	LastCollectedBallColor = color;
			//}
			//Score += ScoreOfCombo;
			//if (color == ThemeColor)
			//{
			//	ThemeScore += ScoreOfCombo;
			//}
			Score += 1;
		}

		public float ScoreOfCombo
		{
			get
			{
				if (ComboCount == 1)
				{
					return 0.5f;
				}
				else if (ComboCount <= 10)
				{
					return 1.0f;
				}
				else if (ComboCount <= 30)
				{
					return 3.0f;
				}
				else
				{
					return 5.0f;
				}
			}
		}

		public int ComboCount
		{
			get { return comboCount; }
			set
			{
				if (comboCount != value)
				{
					comboCount = value;
					HasChanges = true;
				}
			}
		}
		int comboCount;

		BallColor LastCollectedBallColor
		{
			get { return lastCollectedBallColor; }
			set { lastCollectedBallColor = value; }
		}
		BallColor lastCollectedBallColor;

		public BallColor ThemeColor
		{
			get => themeColor;
			set
			{
				if (themeColor != value)
				{
					themeColor = value;
					HasChanges = true;
				}
			}
		}

		BallColor themeColor;

		public float Score
		{
			get => score;
			set
			{
				if (score != value)
				{
					score = value;
					HasChanges = true;
				}
			}
		}

		float score;

		public float ThemeScore
		{
			get => themeScore;
			set
			{
				if (themeScore != value)
				{
					themeScore = value;
					HasChanges = true;
				}
				if (themeScore >= 100)
				{
					OnTheamScoreFull();
					themeScore = 0;
				}
			}
		}

		float themeScore;

		public bool HasChanges { get; private set; }

		public void ClearChanges()
		{
			HasChanges = false;
		}

		[SerializeField]
		GameObject playerBallPrefab;

		[SerializeField]
		GameObject collectableBallPrefab;

		[SerializeField]
		GameObject indicatorPrefab;
	}
}
