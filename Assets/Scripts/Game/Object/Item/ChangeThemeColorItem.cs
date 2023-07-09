
using System;
using UnityEngine;
using static CGJ2023.Enums;

namespace CGJ2023
{
	/// <summary>
	/// 主题色更换道具
	/// </summary>
	public class ChangeThemeColorItem : BaseItem
	{
        [SerializeField]
		BallColor color;

        [SerializeField]
        int spawnProb = 10;
        public override int SpawnProb => spawnProb;

        protected override string DebugString => $"ChangeThemeColorItem: Set theme color to {color}";

        protected override void ApplyEffectCore(PlayerBall player)
        {
			room.ThemeColor = color;
        }

        protected override void StartCore()
		{
			color = (BallColor)Room.random.Next((int)BallColor.MAX);
        }

		protected override void UpdateCore()
		{
		}
    }
}
