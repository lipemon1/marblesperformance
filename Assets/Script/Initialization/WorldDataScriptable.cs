using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Marbles.Initialization
{
    [CreateAssetMenu(fileName = "World Data", menuName = "Scriptables/New World Data")]
    public class WorldDataScriptable : ScriptableObject
    {
        public int ActorsOnStart;
        public int MarblesOnStart;
        public int MarblesOnRuntime;
        public int MarblesOffset;
    }   
}
