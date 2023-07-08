using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CGJ2023
{
    public class BombItem : BaseItem
    {
        [SerializeField]
        int spawnProb = 10;
        public override int SpawnProb => spawnProb;

        [SerializeField]
        float BombRadius = 3.0f;

        float lifeTime = 0.02f;

        Collider2D Collider;

        protected override void ApplyEffectCore(PlayerBall player)
        {
            Collider.enabled = true;
        }

        protected override void StartCore()
        {
            Collider = transform.GetChild(1).GetComponentInChildren<Collider2D>();
            Collider.enabled = false;
            if (Collider is CircleCollider2D circle)
            {
                circle.radius = BombRadius;
            }
        }

        protected override void UpdateCore()
        {
            if (Collider.enabled)
            {
                lifeTime -= Time.deltaTime;
            }
            
            if (lifeTime <= 0)
            {
                Destroy(this);
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            var ball = collision.gameObject.GetComponent<CollectableBall>();
            if (ball != null)
            {
                ball.DestroyBall();
            }

            room.Score += 1.0f;
        }
    }
}
