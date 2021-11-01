using UnityEngine;

namespace Marbles.Initialization
{
    [CreateAssetMenu(fileName = "World Data", menuName = "Scriptables/New World Data")]
    public class WorldDataScriptable : ScriptableObject, IDataProvider
    {
        public int ActorsOnStart;
        public int MarblesOnStart;
        public int MarblesOnRuntime;
        public int MarblesOffset;
        public float DetectorSize;
        public int GetActorsOnStart()
        {
            return ActorsOnStart;
        }

        public int GetMarblesOnStart()
        {
            return MarblesOnStart;
        }

        public int GetMarblesOnRuntime()
        {
            return MarblesOnRuntime;
        }

        public int GetMarblesOffset()
        {
            return MarblesOffset;
        }

        public float GetDetectorSize()
        {
            return DetectorSize;
        }
    }   
}
