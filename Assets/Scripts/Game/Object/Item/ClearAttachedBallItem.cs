namespace CGJ2023
{
	/// <summary>
	/// ��ɫ����������
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
