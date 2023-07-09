using System.Collections.Generic;
using UnityEngine;

namespace CGJ2023
{
	/// <summary>
	/// 玩家角色球
	/// </summary>
	public class PlayerBall : BaseBall
	{
		protected override void StartCore()
		{
			canPush = true;
		}

		protected override void UpdateCore()
		{
			//var moveDirection = Vector3.zero;
			//if (Input.GetKey(KeyCode.LeftArrow))
			//{
			//    moveDirection += Vector3.left;
			//}

			//if (Input.GetKey(KeyCode.RightArrow))
			//{
			//    moveDirection += Vector3.right;
			//}

			//if (Input.GetKey(KeyCode.UpArrow))
			//{
			//    moveDirection += Vector3.up;
			//}

			//if (Input.GetKey(KeyCode.DownArrow))
			//{

			//    moveDirection += Vector3.down;
			//}

			//transform.position = transform.position + moveDirection.normalized * moveSpeed * Time.deltaTime;

			//if (transform.position.x < -8.6f)
			//{
			//	var rigidBody = GetComponent<Rigidbody2D>();
			//	if (rigidBody != null)
			//	{
			//		rigidBody.AddForce(Vector2.right * Mathf.Abs(rigidBody.velocity.x), ForceMode2D.Impulse);
			//	}
			//}

			//if (transform.position.x > 8.6f)
			//{
			//	var rigidBody = GetComponent<Rigidbody2D>();
			//	if (rigidBody != null)
			//	{
			//		rigidBody.AddForce(Vector2.left * Mathf.Abs(rigidBody.velocity.x), ForceMode2D.Impulse);
			//	}
			//}

			//if (transform.position.y > 3.5f)
			//{
			//	var rigidBody = GetComponent<Rigidbody2D>();
			//	if (rigidBody != null)
			//	{
			//		rigidBody.AddForce(Vector2.down * Mathf.Abs(rigidBody.velocity.y), ForceMode2D.Impulse);
			//	}
			//}

			//if (transform.position.y < -4.7f)
			//{
			//	var rigidBody = GetComponent<Rigidbody2D>();
			//	if (rigidBody != null)
			//	{
			//		rigidBody.AddForce(Vector2.up * Mathf.Abs(rigidBody.velocity.y), ForceMode2D.Impulse);
			//	}
			//}

			var rigidBody = GetComponent<Rigidbody2D>();
			if (canPush)
			{
				if (Input.GetMouseButtonDown(0))
				{
					if (rigidBody != null)
					{
						var pushDirection = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
						rigidBody.AddForce(pushDirection.normalized * pushForce, ForceMode2D.Impulse);
						canPush = false;
					}
				}
			}

			if (!canPush)
			{
				canPush = rigidBody.velocity.sqrMagnitude < 0.01f;
				if (canPush)
				{
					room.FinishCurrentPush();
				}
			}

			if (playerIcon != null)
			{
				if (canPush)
				{
					playerIcon.transform.localPosition = new Vector3(0f, Mathf.Cos(Time.time * 5f) * 0.25f + 1.5f);
					playerIcon.color = Color.green;
				}
				else
				{
					playerIcon.transform.localPosition = new Vector3(0f, 1.5f);
					playerIcon.color = Color.red;
				}
			}
		}

		bool canPush;

		void OnCollisionEnter2D(Collision2D collision)
		{
			var colliderGameObject = collision.gameObject;
			var ball = colliderGameObject.GetComponent<CollectableBall>();
			var item = colliderGameObject.GetComponent<BaseItem>();

			if (ball != null)
			{
				var color = ball.BallColor;
				if (room.ThemeColor == color)
				{
					room.OnCollectBall(color);
					ball.DestroyBall();
				}
				else
				{
					room.ComboCount = 0;
					ball.AttachTo(this);
				}
				attachedBalls.Add(ball);
			}
			else if (item != null)
			{
				item.ApplyEffect(this); 
			}
		}

		#region ItemEffects
		public void ClearAttachedBalls()
		{
			foreach (var ball in attachedBalls)
			{
				ball.DestroyBall();
			}
			attachedBalls.Clear();
		}

		#endregion

		[SerializeField]
		float pushForce;

		[SerializeField]
		SpriteRenderer playerIcon;

		HashSet<CollectableBall> attachedBalls = new();
	}
}
