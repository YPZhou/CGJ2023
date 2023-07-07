using UnityEngine;

namespace CGJ2023
{
	public abstract class BaseGameObject : MonoBehaviour
	{
		void Start()
		{
			CacheComponents();
			StartCore();
		}

		protected abstract void StartCore();

		void Update()
		{
			UpdateCore();
		}

		protected abstract void UpdateCore();

		void CacheComponents()
		{
			spriteRenderer = GetComponent<SpriteRenderer>();
			if (spriteRenderer == null)
			{
				Debug.LogError($"{gameObject.name}È±ÉÙSpriteRenderer×é¼þ");
			}
		}

		SpriteRenderer spriteRenderer;
	}
}
