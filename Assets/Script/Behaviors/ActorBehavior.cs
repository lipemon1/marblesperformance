using System;
using System.Collections;
using Marbles.Behaviors.Containers;
using UnityEngine;
using UnityEngine.Profiling;

namespace Marbles.Behaviors
{
 public class ActorBehavior : MonoBehaviour
{
    enum State
    {
        Idle,
        Hunting,
    }

    public MarbleContainer ContainerReference { get; set; }
    
    private State _currentState;
    private MarbleBehavior _currentTarget;
    [SerializeField] private MarbleDetectorBehavior _marbleDetectorBehavior;

    
    //cache
    Vector3 thisToTarget;
    Vector3 thisToTargetDirection;

    void Update()
    { 
        Profiler.BeginSample("Switch Case");
        switch( _currentState )
        {
            case State.Idle:
                UpdateIdle();
                break;
            case State.Hunting:
                UpdateMoving();
                break;
        }
        Profiler.EndSample();
    }


    private void UpdateIdle()
    {
        if(!_marbleDetectorBehavior.gameObject.activeInHierarchy)
            _marbleDetectorBehavior.gameObject.SetActive(true);
        
        Profiler.BeginSample("Update Idle");
        if( _currentTarget == null )
            FindNewTarget();
        Profiler.EndSample();
    }

    void ClearTargetDelegate()
    {
        Profiler.BeginSample("ClearTargetDelegate");
        if (_currentTarget != null)
        {
            _currentTarget.OnMarbleClaimed -= ClearTargetDelegate;   
        }

        ResetTarget();
        Profiler.EndSample();
    }

    void FindNewTarget()
    {
        Profiler.BeginSample("FindNewTarget");
        Profiler.BeginSample("GetClosesMarble");
        _currentTarget = _marbleDetectorBehavior.GetClosestMarble(ContainerReference);
        Profiler.EndSample();
        
        if (_currentTarget != null)
        {
            _currentTarget.TargetThisMarble();
            _currentTarget.OnMarbleClaimed += ClearTargetDelegate;
            ChangeState(State.Hunting);
        }
        Profiler.EndSample();
    }

    private void UpdateMoving()
    {
        if (_currentTarget == null)
        {
            ResetTarget();
            return;
        }
        
        if(_marbleDetectorBehavior.gameObject.activeInHierarchy)
            _marbleDetectorBehavior.gameObject.SetActive(false);

        Profiler.BeginSample("Update Moving");
        Profiler.BeginSample("Was Claimed");
        if( _currentTarget.WasClaimed )
        {
            ResetTarget();
            Profiler.EndSample();
            Profiler.EndSample();
            return;
        }
        Profiler.EndSample();
        
        Profiler.BeginSample("Not Claimed");
        Profiler.BeginSample("Target Calculation");
        thisToTarget = _currentTarget.transform.position - this.transform.position;
        thisToTargetDirection = thisToTarget.normalized;
        Profiler.EndSample();
        
        Profiler.BeginSample("Target Movement");
        this.transform.position += thisToTargetDirection *10* Time.deltaTime;
        Profiler.EndSample();

        Profiler.BeginSample("Target Distance Check");
        if( thisToTarget.magnitude < 0.1f )
        {
            Profiler.BeginSample("Claim Marble");
            ContainerReference.ClaimMarble( _currentTarget );
            Profiler.EndSample();
            
            Profiler.BeginSample("Reseting Target");
            ResetTarget();
            Profiler.EndSample();
        }
        Profiler.EndSample();
        Profiler.EndSample();
        Profiler.EndSample();
    }

    private void ResetTarget()
    {
        _currentTarget = null;
        ChangeState(State.Idle);
    }

    void ChangeState(State newState)
    {
        _currentState = newState;
    }

    public void SetDetectorSize(float detectorSize)
    {
        _marbleDetectorBehavior.transform.localScale = new Vector3(detectorSize, detectorSize, detectorSize);
    }

    public bool IsHunting()
    {
        return _currentState == State.Hunting;
    }
}   
}
