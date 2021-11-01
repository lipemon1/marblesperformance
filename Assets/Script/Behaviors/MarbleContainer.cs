using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Marbles.Pools;
using UnityEngine;
using UnityEngine.Profiling;
using Random = UnityEngine.Random;

[RequireComponent(typeof(ActorContainer))]
public class MarbleContainer : MonoBehaviour
{
    [SerializeField] bool _debugValues;
    [SerializeField]
    private MarbleBehavior marblePrefab;

    [Header("Debug Values")]
    private Dictionary<Guid,MarbleBehavior> _marbles = new Dictionary<Guid, MarbleBehavior>();
    [SerializeField] int _currentMarbles;
    [SerializeField] int _currentMarblesAvailable;
    [SerializeField] int _marbleDeactivate;

    public void StartContainer(int startMarblesAmount, int runtimeMarblesAmount, int marblesOffset)
    {
        StopAllCoroutines();

        PoolController.StartMarblesPool(marblePrefab, this.transform, (runtimeMarblesAmount + marblesOffset), ref _marbles);

        StartCoroutine(SpawnMarbles(runtimeMarblesAmount, marblesOffset));
    }

    IEnumerator SpawnMarbles(int runtimeMarblesAmount, int marblesOffset)
    {
        while( true )
        {
            if( _marbles.Values.Count < runtimeMarblesAmount )
            {
                PoolController.GetNewMarbles(marblesOffset, ref _marbles);
            }
            //yield return new WaitForEndOfFrame();
            yield return new WaitForSeconds( 0.1f );
        }
    }

    public void ClaimMarble( MarbleBehavior marble )
    {
        Profiler.BeginSample("Checking Key");
        if( _marbles.ContainsKey( marble.Id ) )
        {
            Profiler.BeginSample("Removing Key");
            _marbles.Remove( marble.Id );
            Profiler.EndSample();
        }
        Profiler.EndSample();
        
        Profiler.BeginSample("Claim Marble");
        marble.ClaimThisMarble();
        Profiler.EndSample();

        Profiler.BeginSample("Debug Values");
        if(_debugValues)
            DebugValues();
        Profiler.EndSample();
    }

    public Dictionary<Guid, MarbleBehavior>.ValueCollection GetAllMarbles()
    {
        return _marbles.Values;
    }

    private void DebugValues()
    {
        _currentMarbles = _marbles.Values.Count;
        _currentMarblesAvailable = _marbles.Values.Count(m => !m.WasClaimed);
        _marbleDeactivate = _marbles.Values.Count(m => !m.gameObject.activeInHierarchy);
    }
}
