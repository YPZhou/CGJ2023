using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CGJ2023
{
    public class MoreBallsItem : BaseItem
    {
        [SerializeField]
        int spawnProb = 10;
        public override int SpawnProb => spawnProb;

        [SerializeField]
        int SpeedModifier = 10;

        protected override void ApplyEffectCore(PlayerBall player)
        {
            room.BallBirthSpeedModifier += SpeedModifier;
        }

        protected override void StartCore()
        {
        }

        protected override void UpdateCore()
        {
        }
    }
}
