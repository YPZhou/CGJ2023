using UnityEngine;
using static CGJ2023.Enums;

namespace CGJ2023
{
	/// <summary>
	/// ø… ’ºØ«Ú
	/// </summary>
	public class CollectableBall : BaseBall
	{
		protected override void StartCore()
		{
			SetSpriteColorToBallColor();
		}

		protected override void UpdateCore()
		{
			SetSpriteColorToBallColor();
		}

		void SetSpriteColorToBallColor()
		{
			switch (ballColor)
			{
				case BallColor.Red:
					spriteRenderer.color = Color.red;
					break;
				case BallColor.Blue:
					spriteRenderer.color = Color.blue;
					break;
				case BallColor.Green:
					spriteRenderer.color = Color.green;
					break;
			}
		}

		[SerializeField]
		BallColor ballColor;
	}
}
