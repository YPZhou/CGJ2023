using UnityEngine;

namespace CGJ2023
{
	public class TouchHint : MonoBehaviour
	{
		void Start()
		{
			startTime = Time.time;
		}

		void Update()
		{
			if (Time.time - startTime < 0.5f)
			{
				transform.localScale = Vector3.one * (1 + Time.time - startTime);
			}
			else
			{
				Destroy(gameObject);
			}
		}

		float startTime;
	}
}
