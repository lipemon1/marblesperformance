using System.Collections.Generic;
using Marbles.Initialization;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Marbles.Behaviors.Containers
{
    [RequireComponent(typeof(MarbleContainer))]
    public class ActorContainer : MonoBehaviour, IContainer
    {
        public ActorBehavior ActorPrefab;

        private readonly List<ActorBehavior> _actors = new List<ActorBehavior>();
        private MarbleContainer _containerReference;

        public void StartContainer(IDataProvider dataProvider)
        {
            _containerReference = this.gameObject.GetComponent<MarbleContainer>();
            for( int i = 0; i < dataProvider.GetActorsOnStart(); i++ )
            {
                ActorBehavior newActor = Instantiate( ActorPrefab, this.transform );
                newActor.ContainerReference = _containerReference;
                newActor.transform.position = Random.insideUnitSphere * 100f;
                newActor.SetDetectorSize(dataProvider.GetDetectorSize());
                _actors.Add( newActor );
            }
        }
    }   
}