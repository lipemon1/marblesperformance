using System.Collections.Generic;
using System.Linq;
using Marbles.Behaviors.Containers;
using UnityEngine;
using UnityEngine.Profiling;

namespace Marbles.Behaviors
{
    public class MarbleDetectorBehavior : MonoBehaviour
    {
        [SerializeField] public List<MarbleBehavior> MarblesInRange = new List<MarbleBehavior>();
        [SerializeField] private Collider _collider;

        //cache
        MarbleBehavior newTargetBehavior;

        void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Marble"))
            {
                MarbleBehavior marbleBehavior = other.GetComponent<MarbleBehavior>();
                if (marbleBehavior != null)
                    MarblesInRange.Add(marbleBehavior);
            }
        }

        void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Marble"))
            {
                MarbleBehavior marbleBehavior = other.GetComponent<MarbleBehavior>();

                if (marbleBehavior != null)
                    if (MarblesInRange.Contains(marbleBehavior))
                        MarblesInRange.Remove(marbleBehavior);
            }
        }

        public MarbleBehavior GetClosestMarble(MarbleContainer marbleContainer)
        {
            if (MarblesInRange.Count > 0)
                newTargetBehavior = GetMarbleFromList(MarblesInRange, transform.position);

            if (newTargetBehavior == null)
                newTargetBehavior = GetMarbleFromList(marbleContainer.GetAllMarbles(), transform.position);

            return newTargetBehavior;
        }

        static MarbleBehavior GetMarbleFromList(IEnumerable<MarbleBehavior> marbles, Vector3 position)
        {
            return marbles
                .OrderBy(m => (position - m.transform.position).magnitude)
                .FirstOrDefault(m => !m.WasClaimed && !m.BeingTarget);
        }
    }
}