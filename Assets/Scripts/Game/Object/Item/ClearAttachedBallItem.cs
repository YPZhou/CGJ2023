namespace CGJ2023
{
	/// <summary>
	/// 异色球消除道具
	/// </summary>
	public class ClearAttachedBallItem : BaseItem
	{
        protected override string DebugString => $"ClearAttachedBallItem: Clearing all attached balls";
        public override void ApplyEffect(PlayerBall player)
        {
            base.ApplyEffect(player);
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
