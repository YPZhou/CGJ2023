﻿using System.Collections.Generic;
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
            }
            else if (item != null)
            {
                item.ApplyEffect(this);
                Destroy(colliderGameObject);
            }
        }

        #region ItemEffects
        public void ClearAttachedBalls()
        {

        }

        #endregion

        [SerializeField]
        float moveSpeed;

        List<CollectableBall> attachedBalls = new();
    }
}
