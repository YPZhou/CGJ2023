using UnityEngine;

namespace CGJ2023
{
	/// <summary>
	/// ��ҽ�ɫ��
	/// </summary>
	public class PlayerBall : BaseBall
	{
		protected override void StartCore()
		{
		}

		protected override void UpdateCore()
		{
			var moveDirection = Vector3.zero;
			if (Input.GetKey(KeyCode.LeftArrow))
			{
				moveDirection += Vector3.left;
			}

			if (Input.GetKey(KeyCode.RightArrow))
			{
				moveDirection += Vector3.right;
			}

			if (Input.GetKey(KeyCode.UpArrow))
			{
				moveDirection += Vector3.up;
			}

			if (Input.GetKey(KeyCode.DownArrow))
			{

				moveDirection += Vector3.down;
			}

			transform.position = transform.position + moveDirection.normalized * moveSpeed * Time.deltaTime;
		}

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
					ball.AttachTo(this);
				}
			}
			else if (item != null)
			{
				// Apply item effect
				// ...
			}
		}

		[SerializeField]
		float moveSpeed;
	}
}
