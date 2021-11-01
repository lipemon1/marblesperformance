using System;
using System.Collections.Generic;
using Marbles.Behaviors;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Marbles.Pools
{
    public class PoolController : MonoBehaviour
    {
        static List<MarbleBehavior> _poolItems;

        [SerializeField] bool _debugValues;
        [SerializeField] int _itemsOnPool;

        static MarbleBehavior _prefab;
        static Transform _parent;

        void Update()
        {
            if(_debugValues)
                _itemsOnPool = _poolItems.Count;
        }

        public static void StartMarblesPool(MarbleBehavior prefab, Transform parent, int amount, ref Dictionary<Guid, MarbleBehavior> marblesDic)
        {
            _prefab = prefab;
            _parent = parent;

            _poolItems = new List<MarbleBehavior>();
            
            for (int i = 0; i < amount; i++)
            {
                MarbleBehavior newMarble = NewMarbleInstance(); 
                marblesDic.Add(newMarble.Id, newMarble);
            }
        }

        static MarbleBehavior NewMarbleInstance()
        {
            MarbleBehavior newMarble = Instantiate(_prefab, new Vector3(Random.value, Random.value, Random.value ), Quaternion.identity);
            newMarble.Id = Guid.NewGuid();
            newMarble.transform.parent = _parent;
            newMarble.transform.position = Random.insideUnitSphere * 100f;
            return newMarble;
        }

        static MarbleBehavior GetNewMarble()
        {
            if (_poolItems.Count >= 1)
            {
                MarbleBehavior marbleToReturn = _poolItems[0];
                _poolItems.Remove(marbleToReturn);
                marbleToReturn.gameObject.SetActive(true);
                return marbleToReturn;
            }
            else
            {
                MarbleBehavior newMarble = NewMarbleInstance();
                return newMarble;
            }
        }

        public static void GetNewMarbles(int amount, ref Dictionary<Guid, MarbleBehavior> marblesDic)
        {
            for (int i = 0; i < amount; i++)
            {
                MarbleBehavior marble = GetNewMarble();
                marble.Spawn();
                marblesDic.Add(marble.Id, marble);
            }
        }

        public static void DespawnMarble(MarbleBehavior marbleBehavior)
        {
            if (_poolItems == null)
                _poolItems = new List<MarbleBehavior>();
            
            marbleBehavior.gameObject.SetActive(false);
            _poolItems.Add(marbleBehavior);
        }
    }
}
