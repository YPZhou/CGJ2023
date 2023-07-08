
using System;
using UnityEngine;
using static CGJ2023.Enums;

namespace CGJ2023.Item
{
	/// <summary>
	/// 主题色更换道具
	/// </summary>
	public class ChangeThemeColorItem : BaseItem
	{
        [SerializeField]
		BallColor color;
        public override void ApplyEffect(PlayerBall player)
        {
            base.ApplyEffect(player);
			room.ThemeColor = color;
			Debug.Log($"ChangeThemeColorItem::Changing Main theme color to {color}");
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
