using UnityEngine;

namespace CGJ2023
{
	/// <summary>
	/// ��ɫ����������
	/// </summary>
	public class ClearAttachedBallItem : BaseItem
	{
        [SerializeField]
        int spawnProb = 10;
        public override int SpawnProb => spawnProb;

        protected override string DebugString => $"ClearAttachedBallItem: Clearing all attached balls";
        protected override void ApplyEffectCore(PlayerBall player)
        {
            player.ClearAttachedBalls();
        }

        protected override void StartCore()
		{
		}

		protected override void UpdateCore()
		{
		}
    }
}
