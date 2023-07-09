using System;
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
				UpdateRemainingPush();
				UpdateScore();
				UpdateTargetScore();
				UpdateCombo();
				room.ClearChanges();
			}

			UpdateTimer();
		}

		void UpdateRemainingPush()
		{
			if (remainingPush != null)
			{
				remainingPush.text = room.RemainingPushCount.ToString();
			}
		}

		void UpdateCombo()
        {
			combo.text = room.ComboCount.ToString();
        }

		void UpdateScore()
		{
			if (score != null)
			{
				score.text = room.Score.ToString();
			}
		}

		void UpdateTargetScore()
		{
			if (targetScore != null)
			{
				targetScore.text = room.TargetScore.ToString();
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
		Text score;

		[SerializeField]
		Text targetScore;

		[SerializeField]
		Text remainingPush;

		[SerializeField]
		Text timer;

		[SerializeField]
		Text combo;

		[SerializeField]
		Room room;
	}
}
