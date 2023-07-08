using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using static CGJ2023.Enums;

namespace CGJ2023
{
    public class ItemSpawner : BaseGameObject
    {
        [SerializeField]
        float itemSpawnTime;
        public float SpawnTime => itemSpawnTime;

        [SerializeField]
        float nextSpawnTime;
        public float NextSpawnTime => nextSpawnTime;

        [SerializeField]
        List<string> ItemPrefabsPath = new List<string>();
        int probSum;
        List<int> probs = new List<int>();
        List<GameObject> itemPrefabas = new();

        protected override void StartCore()
        {

            probSum = 0;
            foreach (var itemPath in ItemPrefabsPath)
            {
                var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(itemPath);
                itemPrefabas.Add(prefab);
                var comp = prefab.GetComponent<BaseItem>();
                probs.Add(probSum);
                probSum += comp.SpawnProb;
            }
        }

        protected override void UpdateCore()
        {
            nextSpawnTime -= Time.deltaTime;
        }

        public bool CanSpawnNow()
        {
            return nextSpawnTime < 0.0f;
        }

        public void SpawnItem(Vector2 pos)
        {
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
