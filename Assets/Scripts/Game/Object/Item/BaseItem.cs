using UnityEngine;

namespace CGJ2023
{
	public abstract class BaseItem : BaseGameObject
	{
        public abstract int SpawnProb { get; }

        public void ApplyEffect(PlayerBall player)
        {
            if (DebugString is not null)
            {
                Debug.Log(DebugString);
            }
            ApplyEffectCore(player);
        }

        protected abstract void ApplyEffectCore(PlayerBall player);

        protected virtual string DebugString => null;
    }
}
