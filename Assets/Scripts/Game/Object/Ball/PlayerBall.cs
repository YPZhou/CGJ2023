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
            
            if (transform.position.x < -8.6f)
            {
                transform.position = new Vector3(-8.6f, transform.position.y);
            }

			if (transform.position.x > 8.6f)
			{
				transform.position = new Vector3(8.6f, transform.position.y);
			}

            if (transform.position.y > 4.7f)
            {
                transform.position = new Vector3(transform.position.x, 4.7f);
            }

			if (transform.position.y < -4.7f)
			{
				transform.position = new Vector3(transform.position.x, -4.7f);
			}

            if (playerIcon != null)
            {
                playerIcon.transform.localPosition = new Vector3(0f, Mathf.Cos(Time.time * 5f) * 0.25f + 1.5f);
            }
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
        float moveSpeed;

        [SerializeField]
        SpriteRenderer playerIcon;

        HashSet<CollectableBall> attachedBalls = new();
    }
}
