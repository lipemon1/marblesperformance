using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(ActorContainer))]
public class MarbleContainer : MonoBehaviour
{
    [SerializeField]
    private MarbleBehavior marblePrefab;

    private readonly Dictionary<Guid,MarbleBehavior> _marbles = new Dictionary<Guid, MarbleBehavior>();

    public void StartContainer(int startMarblesAmount, int runtimeMarblesAmount)
    {
        StopAllCoroutines();
        for( var i = 0; i < startMarblesAmount; i++ )
        {
            GenerateMarble();
        }

        StartCoroutine(SpawnMarbles(runtimeMarblesAmount));
    }

    IEnumerator SpawnMarbles(int runtimeMarblesAmount)
    {
        while( true )
        {
            if( _marbles.Values.Count < runtimeMarblesAmount )
            {
                for( var i = 0; i < 25; i++ )
                {
                   GenerateMarble();
                }
            }
            yield return new WaitForEndOfFrame();
            //yield return new WaitForSeconds( 0.5f );
        }
    }

    private void GenerateMarble()
    {
        var newMarble = Instantiate( marblePrefab, new Vector3( Random.value, Random.value, Random.value ), Quaternion.identity );
        newMarble.Id = Guid.NewGuid();
        newMarble.transform.parent = this.transform;
        newMarble.transform.position = Random.insideUnitSphere * 100f;
        _marbles.Add( newMarble.Id, newMarble );
    }

    public void ClaimMarble( MarbleBehavior marble )
    {
        if( _marbles.ContainsKey( marble.Id ) )
        {
            _marbles.Remove( marble.Id );
        }
        marble.WasClaimed = true;
    }

    public MarbleBehavior GetCloseMarbleToPosition( Vector3 position )
    {
        return _marbles
            .Values
            .OrderBy( m => ( position - m.transform.position ).magnitude )
            .Take(10)
            .OrderBy( m => m.Id )
            .FirstOrDefault( m => !m.WasClaimed );
    }
}
