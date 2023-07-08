using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CGJ2023
{
    public class ItemSpawnFasterItem : BaseItem
    {
        [SerializeField]
        int spawnProb = 10;
        public override int SpawnProb => spawnProb;

        [SerializeField]
        float time;

        protected override void StartCore()
        {
        }

        protected override void UpdateCore()
        {
        }

        protected override void ApplyEffectCore(PlayerBall player)
        {
            room.ItemSpawner.NextSpawnTime -= time;
        }
    }
}
