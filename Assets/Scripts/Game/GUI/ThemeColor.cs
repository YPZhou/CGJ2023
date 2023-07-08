using UnityEngine;
using UnityEngine.UI;
using static CGJ2023.Enums;

namespace CGJ2023
{
	public class ThemeColor : MonoBehaviour
	{
		void Start()
		{
			image = GetComponent<Image>();
			if (image == null)
			{
				Debug.LogError($"{gameObject.name}È±ÉÙImage×é¼þ");
			}
		}

		void Update()
		{
			switch (room.ThemeColor)
			{
				case BallColor.Red:
					image.color = Color.red;
					break;
				case BallColor.Green:
					image.color = Color.green;
					break;
				case BallColor.Blue:
					image.color = Color.blue;
					break;
			}
		}

		Image image;

		[SerializeField]
		Room room;
	}
}
