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
				Debug.LogError($"{gameObject.name}缺少SpriteRenderer组件");
			}

			var scriptsObject = GameObject.Find("SceneScripts");
			if (scriptsObject != null )
			{
				var room = scriptsObject.GetComponent<Room>();
				if (room == null)
				{
					Debug.LogError($"{scriptsObject.name}缺少Room组件");
				}
			}
			else
			{
				Debug.LogError("场景缺少SceneScripts预制体");
			}
		}

		protected SpriteRenderer spriteRenderer;
		protected Room room;
	}
}
