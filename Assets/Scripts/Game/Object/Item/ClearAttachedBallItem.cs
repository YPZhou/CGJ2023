namespace CGJ2023
{
	/// <summary>
	/// 异色球消除道具
	/// </summary>
	public class ClearAttachedBallItem : BaseItem
	{

        public override void ApplyEffect(PlayerBall player)
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
