using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CGJ2023
{
    public class BombCollider : MonoBehaviour
    {
        BombItem Item;

        private void Start()
        {
            Item = GetComponentInParent<BombItem>();
        }

        private void OnCollisionStay2D(Collision2D collision)
        {
            HandleCollision(collision);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            HandleCollision(collision);
        }

        void HandleCollision(Collision2D collision)
        {
            var ball = collision.gameObject.GetComponent<CollectableBall>();
            if (ball is null)
            {
                return;
            }

            if (ball.IsAttached)
            {
                return;
            }
            Item.Room.Score += 1.0f;
            ball.DestroyBall();

            Debug.Log("Bomb: Get Score!");
        }
    }
}
