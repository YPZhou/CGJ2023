using System;
using UnityEngine;

namespace CGJ2023
{
	/// <summary>
	/// Õ¨µ¯µÀ¾ß
	/// </summary>
	public class ComboCountItem : BaseItem
	{
        [SerializeField]
        int spawnProb = 10;
        public override int SpawnProb => spawnProb;

        [SerializeField]
        int comboxAddup;

        protected override string DebugString => $"ComboCountItem: Adding combo";

        public override void ApplyEffect(PlayerBall player)
        {
            base.ApplyEffect(player);
            room.ComboCount += comboxAddup;
        }

        protected override void StartCore()
        {
        }

        protected override void UpdateCore()
        {
        }
    }
}
