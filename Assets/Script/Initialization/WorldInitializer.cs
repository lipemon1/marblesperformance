using Marbles.Behaviors.Containers;
using UnityEngine;

namespace Marbles.Initialization
{
    [RequireComponent(typeof(MarbleContainer))]
    [RequireComponent(typeof(ActorContainer))]
    public class WorldInitializer : MonoBehaviour
    {
        private MarbleContainer marbleContainer;
        private ActorContainer actorContainer;
        [SerializeField] private WorldDataScriptable worldData;

        void Awake()
        {
            if (marbleContainer == null)
                marbleContainer = GetComponent<MarbleContainer>();
            
            if (actorContainer == null)
                actorContainer = GetComponent<ActorContainer>();
        }

        void Start()
        {
            marbleContainer.StartContainer(worldData);
            actorContainer.StartContainer(worldData);
        }
    }   
}
