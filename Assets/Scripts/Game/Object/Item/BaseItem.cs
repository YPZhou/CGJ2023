using UnityEngine;

namespace CGJ2023
{
	public abstract class BaseItem : BaseGameObject
	{
        public virtual void ApplyEffect(PlayerBall player)
        {
            if (DebugString is not null)
            {
                Debug.Log(DebugString);
            }
        }

        protected virtual string DebugString => null;
    }
}
