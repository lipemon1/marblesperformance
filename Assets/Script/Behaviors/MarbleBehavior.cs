using System;
using System.Collections;
using Marbles.Pools;
using UnityEngine;
using Random = UnityEngine.Random;

public class MarbleBehavior : MonoBehaviour, IPoolItem
{
    const int Steps = 60;
    public Guid Id { get; set; }
    private bool _wasClaimed;
    public bool WasClaimed => _wasClaimed;
    private bool _beingTarget;
    public bool BeingTarget => _beingTarget;

    public delegate void OnMarbleClaimedDelegate();
    public OnMarbleClaimedDelegate OnMarbleClaimed;

    [SerializeField]
    private Transform _textboxContainer;
    
    [SerializeField]
    private TextMesh _textmesh;

    void Awake()
    {
        if(_textboxContainer == null)
            _textboxContainer = this.transform.Find("TextboxContainer");
        
        if(_textmesh == null)
            _textmesh = this.transform.Find("TextboxContainer/Textbox/ScoreText").gameObject.GetComponent<TextMesh>();
        
        if(!_textboxContainer.gameObject.activeInHierarchy)
            _textboxContainer.gameObject.SetActive( false );
    }

    private IEnumerator DisplayScore(int steps)
    {
        _textboxContainer.localScale = Vector3.zero;
        _textboxContainer.gameObject.SetActive( true );
        for( var i = 0; i < steps; i++ )
        {
            _textboxContainer.localScale += Vector3.one / steps;
            yield return new WaitForEndOfFrame();
        }
        
        Despawn();
    }

    public void Spawn()
    {
        transform.position = Random.insideUnitSphere * 100f;
        float value = Random.value * 100f - 25f;
        _textmesh.text = value.ToString( "##.#" );
        _wasClaimed = false;
        _beingTarget = false;
    }

    public void ClaimThisMarble()
    {
        if (!_wasClaimed)
        {
            _wasClaimed = true;

            OnMarbleClaimed?.Invoke();
            
            if(gameObject.activeInHierarchy)
                StartCoroutine( DisplayScore(Steps) );
        }
        else
        {
            Debug.LogError("Trying to claim an already claimed marble");
        }
    }

    public void TargetThisMarble()
    {
        _beingTarget = true;
    }

    public void Despawn()
    {
        StopAllCoroutines();
        PoolController.DespawnMarble(this);
        // Destroy(this.gameObject);
    }
}