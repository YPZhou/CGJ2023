using UnityEngine;

namespace CGJ2023
{
	/// <summary>
	/// Íæ¼Ò½ÇÉ«Çò
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

		void OnTriggerEnter2D(Collider2D collider)
		{
			var colliderGameObject = collider.gameObject;
			var ball = colliderGameObject.GetComponent<CollectableBall>();
			var item = colliderGameObject.GetComponent<BaseItem>();

			if (ball != null)
			{
				var color = ball.BallColor;
				var ballTransform = colliderGameObject.transform;

				ballTransform.parent = transform;
				ballTransform.localPosition = ballTransform.localPosition.normalized;
				
				//Destroy(collider.gameObject);
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
