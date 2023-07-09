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
			Tips.SetActive(false);
			var tipScal = Tips.transform.localScale;
			tipScal = new Vector3(1 / tipScal.x, 1 / tipScal.y, 1 / tipScal.z);

            TipsScore.transform.localScale = tipScal;
			TipsRemainingPush.transform.localScale = tipScal;
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
			UpdateTips();
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

        void UpdateTips()
        {
            shouUpRemain -= Time.deltaTime;
			if (shouUpRemain < 0.0f)
			{
                Tips.SetActive(false);
            }
        }

		public void ShouUpNextDifficultyTips(float second, float score, int remainingPush)
		{
            shouUpRemain = second;
            TipsScore.enabled = true;
            TipsRemainingPush.enabled = true;

			TipsScore.text = $"目标得分\r\n{score:f1}" ;
            TipsRemainingPush.text = $"剩余次数\r\n{remainingPush}"; ;

            Tips.SetActive(true);
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

		float shouUpRemain = -1;
		[SerializeField]
		GameObject Tips;
        [SerializeField]
        Text TipsScore;
        [SerializeField]
        Text TipsRemainingPush;
    }
}
