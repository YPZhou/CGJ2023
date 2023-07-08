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

        public override void ApplyEffect(PlayerBall player)
        {
            base.ApplyEffect(player);

        }

        protected override void StartCore()
        {
        }

        protected override void UpdateCore()
        {
        }
    }
}
