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
        switch( _currentState )
        {
            case State.Idle:
                UpdateIdle();
                break;
            case State.Hunting:
                UpdateMoving();
                break;
        }
    }


    private void UpdateIdle()
    {
        if(!_marbleDetectorBehavior.gameObject.activeInHierarchy)
            _marbleDetectorBehavior.gameObject.SetActive(true);
        
        if( _currentTarget == null )
            FindNewTarget();
    }

    void ClearTargetDelegate()
    {
        if (_currentTarget != null)
            _currentTarget.OnMarbleClaimed -= ClearTargetDelegate;   

        ResetTarget();
    }

    void FindNewTarget()
    {
        _currentTarget = _marbleDetectorBehavior.GetClosestMarble(ContainerReference);

        if (_currentTarget != null)
        {
            _currentTarget.TargetThisMarble();
            _currentTarget.OnMarbleClaimed += ClearTargetDelegate;
            ChangeState(State.Hunting);
        }
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
        
        if( _currentTarget.WasClaimed )
        {
            ResetTarget();
            return;
        }
        
        thisToTarget = _currentTarget.transform.position - this.transform.position;
        thisToTargetDirection = thisToTarget.normalized;
        this.transform.position += thisToTargetDirection *10* Time.deltaTime;
        
        if( thisToTarget.magnitude < 0.1f )
        {
            ContainerReference.ClaimMarble( _currentTarget );
            ResetTarget();
        }
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
