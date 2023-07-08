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
			PushToAttachedBall();
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

		void PushToAttachedBall()
		{
			if (IsAttached && transform.localPosition.sqrMagnitude > 1f)
			{
				var rigidBody = GetComponent<Rigidbody2D>();
				if (rigidBody != null)
				{
					rigidBody.AddForce(-transform.localPosition.normalized * 0.1f);
				}

				transform.localRotation = Quaternion.identity;
			}
		}

		public void AttachTo(BaseBall other)
		{
			transform.parent = other.transform;
			transform.localPosition = transform.localPosition.normalized;
			attachedBall = other;
		}

		BaseBall attachedBall;

		public bool IsAttached => attachedBall != null;

		[SerializeField]
		public BallColor BallColor;
	}
}
