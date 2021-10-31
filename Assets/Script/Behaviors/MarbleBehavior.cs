using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Marbles.Pools;
using UnityEngine;

public class MarbleBehavior : MonoBehaviour, IPoolItem
{
    bool poolActive;
    const int Steps = 60;
    public Guid Id { get; set; }
    private bool _wasClaimed;
    public bool WasClaimed
    {
        get
        {
            return _wasClaimed;
        }
        set
        {
            if (!poolActive || !this.gameObject.activeInHierarchy) return;
            
            if( !_wasClaimed && value )
            {
                StartCoroutine( DisplayScore(Steps) );
            }
            _wasClaimed = value;
        }
    }

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
        poolActive = true;
        float value = UnityEngine.Random.value * 100f - 25f;
        _textmesh.text = value.ToString( "##.#" );
        WasClaimed = false;
    }

    public void Despawn()
    {
        poolActive = false;
        StopAllCoroutines();
        PoolController.DespawnMarble(this);
        // Destroy(this.gameObject);
    }
}
