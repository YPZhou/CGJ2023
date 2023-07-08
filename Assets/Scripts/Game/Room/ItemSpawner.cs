using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace CGJ2023
{ 
    public class ItemSpawner : BaseGameObject
    {
        [SerializeField]
        bool Enabled = false;

        [SerializeField]
        float itemSpawnTime;
        public float SpawnTime => itemSpawnTime;

        [SerializeField]
        float nextSpawnTime;
        public float NextSpawnTime => nextSpawnTime;

        int probSum;
        List<int> probs = new List<int>();

        [SerializeField]
        List<GameObject> itemPrefabas = new();

        protected override void StartCore()
        {
            probSum = 0;
            foreach (var item in itemPrefabas)
            {
                var comp = item.GetComponent<BaseItem>();
                probs.Add(probSum);
                probSum += comp.SpawnProb;
            }
        }

        protected override void UpdateCore()
        {
            if (Enabled == false)
            {
                return;
            }
            nextSpawnTime -= Time.deltaTime;
        }

        public bool CanSpawnNow()
        {
            return Enabled ? nextSpawnTime < 0.0f : Enabled;
        }

        public void SpawnItem(Vector2 pos)
        {
            if (itemPrefabas.Count == 0)
            {
                return;
            }

            var type = Room.random.Next(probSum);
            type = probs.BinarySearch(type);

            if (type < 0)
            {
                type = (~type) - 1;
            }
            GameObject itemPrefab = itemPrefabas[type];
            var item = GameObject.Instantiate(itemPrefab, pos, Quaternion.identity);
            nextSpawnTime = SpawnTime;

            Debug.Log($"ItemSpawner: Spawning item: {item.GetComponent<BaseItem>().GetType().Name}");
        }

    }
}
