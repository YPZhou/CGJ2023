using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CGJ2023
{
	public class AvailableIndicator : MonoBehaviour
	{
		HashSet<GameObject> entered = new HashSet<GameObject>();

		int index;

		void Update()
		{
			if (IsAvailable)
			{
				GetComponent<SpriteRenderer>().color = new Color(0, 255, 0, 20);
			}
			else
			{
				GetComponent<SpriteRenderer>().color = new Color(255, 0, 255, 20);
			}
		}

		public int Index
        {
			get { return index; }
			set { index = value; }
        }

		public bool IsAvailable
        {
            get
            {
				return entered.Count == 0;
            }
        }

		void OnTriggerEnter2D(Collider2D collider)
		{
			var colliderGameObject = collider.gameObject;
			Debug.Log(colliderGameObject.name);
			entered.Add(colliderGameObject);
		}

		void OnTriggerExit2D(Collider2D collider)
		{
			var colliderGameObject = collider.gameObject;
			if (entered.Contains(colliderGameObject))
            {
				entered.Remove(colliderGameObject);
			}
		}
	}
}
