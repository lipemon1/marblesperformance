using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(MarbleContainer))]
public class ActorContainer : MonoBehaviour
{
    public ActorBehavior ActorPrefab;

    private readonly List<ActorBehavior> _actors = new List<ActorBehavior>();
    private MarbleContainer _containerReference;

    public void StartContainer(int worldDataActorsOnStart, float worldDataDetectorSize)
    {
        _containerReference = this.gameObject.GetComponent<MarbleContainer>();
        for( int i = 0; i < worldDataActorsOnStart; i++ )
        {
            ActorBehavior newActor = Instantiate( ActorPrefab, this.transform );
            newActor.ContainerReference = _containerReference;
            newActor.transform.position = Random.insideUnitSphere * 100f;
            newActor.SetDetectorSize(worldDataDetectorSize);
            _actors.Add( newActor );
        }
    }
}