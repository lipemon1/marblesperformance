using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Marbles.Pools
{
    public class PoolController : MonoBehaviour
    {
        static List<MarbleBehavior> _poolItems;

        static MarbleBehavior _prefab;
        static Transform _parent;

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
            MarbleBehavior marbleToReturn;

            if (_poolItems.Count >= 1)
            {
                marbleToReturn = _poolItems[0];
                _poolItems.RemoveAt(0);
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
                marblesDic.Add(marble.Id, marble);
                marble.Spawn();
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
