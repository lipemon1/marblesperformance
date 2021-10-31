using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Marbles.Pools
{
    public interface IPoolItem
    {
        void Spawn();
        void Despawn();
    }   
}
