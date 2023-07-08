using System.Collections.Generic;
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
			if (shouldDestroy)
			{
				room.collectableBalls.Remove(gameObject);
				Destroy(gameObject);
			}

			SetSpriteColorToBallColor();
			MarkForDestroy();
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

		void MarkForDestroy()
		{
			if (!shouldDestroy)
			{
				var adjacentBallsWithSameColor = new List<CollectableBall>();
				for (var i = 0; i < room.collectableBalls.Count; i++)
				{
					var obj = room.collectableBalls[i];
						var ball = obj.GetComponent<CollectableBall>();
					if (ball.IsAttached && (transform.position - obj.transform.position).sqrMagnitude <= 0.5f)
					{
						if (ball != null)
						{
							if (ball.BallColor == BallColor)
							{
								if (ball.shouldDestroy)
								{
									shouldDestroy = true;
									break;
								}
								else
								{
									adjacentBallsWithSameColor.Add(ball);
									if (adjacentBallsWithSameColor.Count >= 3)
									{
										shouldDestroy = true;
										foreach (var adjacentBall in adjacentBallsWithSameColor)
										{
											adjacentBall.shouldDestroy = true;
										}

										break;
									}
								}
							}
						}
					}
				}
			}
		}

		bool shouldDestroy;	// mark for destroy in next frame

		void PushToAttachedBall()
		{
			if (IsAttached && transform.localPosition.sqrMagnitude > 1f)
			{
				var rigidBody = GetComponent<Rigidbody2D>();
				if (rigidBody != null)
				{
					rigidBody.AddForce(-transform.localPosition * 0.1f);
				}

				transform.localRotation = Quaternion.identity;
			}
		}

		public void AttachTo(BaseBall other)
		{
			transform.parent = other.transform;
			//transform.localPosition = transform.localPosition.normalized;
			attachedBall = other;
		}

		BaseBall attachedBall;

		public bool IsAttached => attachedBall != null;

		void OnCollisionEnter2D(Collision2D collision)
		{
			var colliderGameObject = collision.gameObject;
			var ball = colliderGameObject.GetComponent<CollectableBall>();

			if (ball != null
				&& BallColor == ball.BallColor
				&& (IsAttached || ball.IsAttached))
			{
				var playerBall = GameObject.Find("PlayerBall")?.GetComponent<PlayerBall>();
				if (playerBall != null)
				{
					ball.AttachTo(playerBall);
				}
			}
		}

		[SerializeField]
		public BallColor BallColor;
	}
}
