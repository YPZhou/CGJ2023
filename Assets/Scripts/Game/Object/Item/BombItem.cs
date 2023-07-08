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

        //Debug
        [SerializeField]
        int BombScore = 0;

        protected override bool DestoryAtOnce => false;

        float lifeTime = 0.04f;

        Collider2D Collider;

        public Room Room => room;

        protected override void ApplyEffectCore(PlayerBall player)
        {
            Collider.enabled = true;
        }

        protected override void StartCore()
        {
            Collider = transform.GetChild(1).GetComponentInChildren<Collider2D>();
            Collider.enabled = false;
        }

        protected override void UpdateCore()
        {
            if (Collider.enabled)
            {
                lifeTime -= Time.deltaTime;
            }
            
            if (lifeTime <= 0)
            {
                Destroy(gameObject);
            }
        }
    }
}
