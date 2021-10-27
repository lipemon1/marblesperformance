using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MarbleContainer))]
public class ActorContainer : MonoBehaviour
{
    public ActorBehavior ActorPrefab;

    private readonly List<ActorBehavior> _actors = new List<ActorBehavior>();
    private MarbleContainer _containerReference;

    void Start()
    {
        _containerReference = this.gameObject.GetComponent<MarbleContainer>();
        for( var i = 0; i < 1000; i++ )
        {
            var newActor = Instantiate( ActorPrefab, this.transform );
            newActor.ContainerReference = _containerReference;
            newActor.transform.position = Random.insideUnitSphere * 100f;
            _actors.Add( newActor );
        }
    }
}
