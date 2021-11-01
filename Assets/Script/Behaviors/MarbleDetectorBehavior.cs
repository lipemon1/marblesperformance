using System.Collections.Generic;
using System.Linq;
using Marbles.Behaviors.Containers;
using UnityEngine;

namespace Marbles.Behaviors
{
 public class MarbleDetectorBehavior : MonoBehaviour
 {
     [SerializeField]
     public List<MarbleBehavior> MarblesInRange = new List<MarbleBehavior>();
 
     void OnTriggerEnter(Collider other)
     {
         if (other.CompareTag("Marble"))
         {
             MarbleBehavior marbleBehavior = other.GetComponent<MarbleBehavior>();
             if(marbleBehavior != null)
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
         MarbleBehavior newTargetBehavior;
         if (MarblesInRange.Count > 0)
         {
             return GetMarbleFromList(MarblesInRange, transform.position);
         }
         else
             newTargetBehavior = GetMarbleFromList(marbleContainer.GetAllMarbles(), transform.position);
 
         if(newTargetBehavior == null)
             Debug.LogError("No new target found");
         
         return newTargetBehavior;
     }
 
     static MarbleBehavior GetMarbleFromList(IEnumerable<MarbleBehavior> marbles, Vector3 position)
     {
         return marbles
             .OrderBy( m => ( position - m.transform.position ).magnitude )
             .Take(10)
             .OrderBy( m => m.Id )
             .FirstOrDefault( m => !m.WasClaimed && !m.BeingTarget );
     }
 }   
}