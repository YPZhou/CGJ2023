using CGJ2023.Item;
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
        public static readonly System.Random random = new System.Random();

		#region create balls
		GameObject PlayerBall;

		public List<GameObject> collectableBalls = new List<GameObject>();

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

        #region item management
		ItemSpawner ItemSpawner = null;
        #endregion

        void Start()
		{
			ThemeColor = BallColor.Red;
			Score = 0;
			ThemeScore = 0;

			InitAvailablePositions();
			CreatePlayerBall();

            ItemSpawner = GetComponent<ItemSpawner>();

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

            TryCreatItems();
        }

        private void TryCreatItems()
        {
            if (ItemSpawner.CanSpawnNow())
            {
                var availables = GetAvailablePositions();
                for (var i = 0; i < CurrentBirthRate && availables.Count > 0; i++)
                {
                    var index = availables.ElementAt<int>(random.Next(availables.Count));
                    availables.Remove(index);
                    var position = GetRandomPositionByIndex(index, true);
                    ItemSpawner.SpawnItem(position);
                }
            }
        }

        #region create balls

        void CreatePlayerBall()
        {
			var collentableBallPrefab = AssetDatabase.LoadAssetAtPath("Assets/Prefabs/Player.prefab", typeof(GameObject)) as GameObject;
			PlayerBall = GameObject.Instantiate(collentableBallPrefab, new Vector2(0, 0), Quaternion.identity);
			PlayerBall.name = "PlayerBall";
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
			collectableBalls.Add(ball);
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

        }

		void OnGreenTheamScoreFull()
		{

		}
		void OnBlueTheamScoreFull()
		{

		}
		
		#endregion

		public void OnCollectBall(BallColor color)
        {
			if (LastCollectedBallColor == color)
            {
				comboCount += 1;
            }
            else
            {
				comboCount = 1;
				LastCollectedBallColor = color;
            }
			Score += ScoreOfCombo;
			if (color == ThemeColor)
            {
				ThemeScore += ScoreOfCombo;
            }
        }
				
		public float ScoreOfCombo
        {
            get
            {
				if (ComboCount == 1)
                {
					return 0.5f;
                }
				else if (ComboCount <= 10){
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
				if (themeScore == 100)
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
	}
}
