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
				Debug.LogError($"{gameObject.name}ȱ��SpriteRenderer���");
			}

			var scriptsObject = GameObject.Find("SceneScripts");
			if (scriptsObject != null )
			{
				var room = scriptsObject.GetComponent<Room>();
				if (room == null)
				{
					Debug.LogError($"{scriptsObject.name}ȱ��Room���");
				}
			}
			else
			{
				Debug.LogError("����ȱ��SceneScriptsԤ����");
			}
		}

		protected SpriteRenderer spriteRenderer;
		protected Room room;
	}
}
