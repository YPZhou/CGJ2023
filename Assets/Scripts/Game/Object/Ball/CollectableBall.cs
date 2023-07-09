using System.Collections;
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
				DestroyBall(shootBall: true);
			}

			if (IsShooting)
			{
				var rigidBody = GetComponent<Rigidbody2D>();
				if (rigidBody != null && rigidBody.velocity.sqrMagnitude < 0.1f)
				{
					IsShooting = false;
					DestroyBall();
				}
			}

			SetSpriteColorToBallColor();
			MarkForDestroy();
			//PushToAttachedBall();
		}
		 
		void SetSpriteColorToBallColor()
		{
			switch (BallColor)
			{
				case BallColor.Red:
					spriteRenderer.color = new Color(1f, 0f, 0f, spriteRenderer.color.a);
					break;
				case BallColor.Blue:
					spriteRenderer.color = new Color(0f, 0f, 1f, spriteRenderer.color.a);
					break;
				case BallColor.Green:
					spriteRenderer.color = new Color(0f, 1f, 0f, spriteRenderer.color.a);
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
							//if (ball.BallColor == BallColor)
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

		bool shouldDestroy; // mark for destroy in next frame
		bool isDestroying;

		void PushToAttachedBall()
		{
			if (IsAttached && transform.localPosition.sqrMagnitude > 0.5f)
			{
				var rigidBody = GetComponent<Rigidbody2D>();
				if (rigidBody != null)
				{
					var forceFactor = 0.2f;
					if (transform.localPosition.sqrMagnitude > 1f)
					{
						forceFactor = 1f;
					}

					rigidBody.AddForce(-transform.localPosition * forceFactor);
				}

				transform.localRotation = Quaternion.identity;
			}
		}

		public void AttachTo(BaseBall other)
		{
			transform.parent = other.transform;
			attachedBall = other;
		}

		BaseBall attachedBall;

		public bool IsAttached => attachedBall != null;

		public void InitializeBall()
		{
			if (spriteRenderer == null)
			{
				spriteRenderer = GetComponent<SpriteRenderer>();
			}

			StartCoroutine(FadeIn(0.5f));
		}

		IEnumerator FadeIn(float time)
		{
			if (time > 0f)
			{
				var startTime = Time.time;
				while (Time.time - startTime < time)
				{
					spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, (Time.time - startTime) / time);
					yield return null;
				}
			}
		}

		public void DestroyBall(bool shootBall = false)
		{
			if (shootBall)
			{
				var rigidBody = GetComponent<Rigidbody2D>();
				rigidBody.AddForce(transform.localPosition.normalized * 0.01f, ForceMode2D.Impulse);
				attachedBall = null;
				IsShooting = true;
			}
			else if (!isDestroying)
			{
				isDestroying = true;

				if (touchHint != null)
				{
					var randomAngle = Random.Range(-45, 45);
					Instantiate(touchHint, transform.position, Quaternion.AngleAxis(randomAngle, Vector3.back));
				}

				if (GameObject.Find("SoundManager") != null)
				{
					SoundManager.PlayAudio("Touch");
				}

				room.collectableBalls.Remove(gameObject);
				StartCoroutine(FadeOutAndDestroy(0.5f));
			}
		}

		public bool IsShooting { get; private set; }

		IEnumerator FadeOutAndDestroy(float time)
		{
			if (time > 0f)
			{
				var rigidBody = GetComponent<Rigidbody2D>();
				if (rigidBody != null)
				{
					rigidBody.Sleep();
				}

				var collider = GetComponent<Collider2D>();
				if (collider != null)
				{
					collider.enabled = false;
				}

				if (gameObject.transform.childCount > 0)
				{
					for (var i = 0; i < gameObject.transform.childCount; i++)
					{
						var childTransform = gameObject.transform.GetChild(i);
						Destroy(childTransform.gameObject);
					}
				}

				spriteRenderer.sortingOrder = 100;
				var startTime = Time.time;
				while (Time.time - startTime < time)
				{
					spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 1 - (Time.time - startTime) / time);

					var parentScale = Vector2.one;
					if (transform.parent != null)
					{
						parentScale = transform.parent.localScale;
					}

					transform.localScale = new Vector2((Time.time - startTime + 1f) / parentScale.x, (Time.time - startTime + 1f) / parentScale.y);
					yield return null;
				}
			}

			Destroy(gameObject);
		}

		void OnCollisionEnter2D(Collision2D collision)
		{
			var colliderGameObject = collision.gameObject;
			var ball = colliderGameObject.GetComponent<CollectableBall>();

			if (ball != null
				//&& BallColor == ball.BallColor
				&& (IsAttached || ball.IsAttached))
			{
				var playerBall = GameObject.Find("PlayerBall")?.GetComponent<PlayerBall>();
				if (playerBall != null)
				{
					ball.AttachTo(playerBall);
				}
			}

			if (ball != null)
			{
				if (IsShooting)
				{
					if (ball.BallColor == BallColor.Red)
					{
						room.OnCollectBall(ball.BallColor);
						ball.DestroyBall();
					}
				}
				else if ((IsAttached || ball.IsAttached))
					//&& BallColor == ball.BallColor)
				{
					var playerBall = GameObject.Find("PlayerBall")?.GetComponent<PlayerBall>();
					if (playerBall != null)
					{
						ball.AttachTo(playerBall);
					}
				}
			}

			//if (ball != null && !ball.IsAttached)
			//{
			//	var rigidBody = ball.GetComponent<Rigidbody2D>();
			//	if (rigidBody != null)
			//	{
			//		var pushDirection = colliderGameObject.transform.position - transform.position;
			//		rigidBody.AddForce(pushDirection.normalized * 4f, ForceMode2D.Impulse);
			//	}
			//}
		}

		[SerializeField]
		public BallColor BallColor;

		[SerializeField]
		GameObject touchHint;
	}
}
