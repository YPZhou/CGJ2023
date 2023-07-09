using UnityEngine;

namespace CGJ2023
{
	public abstract class BaseItem : BaseGameObject
	{
        public abstract int SpawnProb { get; }
        protected virtual bool DestoryAtOnce => true;

        public void ApplyEffect(PlayerBall player)
        {
            if (DebugString is not null)
            {
                Debug.Log(DebugString);
            }
            ApplyEffectCore(player);

            if (DestoryAtOnce)
            {
                Destroy(gameObject);
            }
        }

        protected abstract void ApplyEffectCore(PlayerBall player);

        protected virtual string DebugString => null;
    }
}
