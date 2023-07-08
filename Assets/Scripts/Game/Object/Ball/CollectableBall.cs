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
			switch (BallColor)
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

		public void AttachTo(BaseBall other)
		{
			transform.parent = other.transform;
			transform.localPosition = transform.localPosition.normalized;
			IsAttached = true;
		}

		public bool IsAttached { get; private set; }

		[SerializeField]
		public BallColor BallColor;
	}
}
