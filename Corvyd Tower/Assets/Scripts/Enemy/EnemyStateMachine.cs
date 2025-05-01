// STATE MACHINE - CONTEXT

using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class EnemyStateMachine : MonoBehaviour
{
    // Reference to the currently active state
    EnemyBaseState _currentState;

    // Initialization of each possible state
    public EnemyAliveState AliveState = new();
    public EnemyDyingState DyingState = new();

    [SerializeField] bool _isAlive;

    [SerializeField] public bool _isAtGate;
    public bool triggerAttack = false;

    [SerializeField] private Door _door;

    public bool IsAlive
    {
        get => _isAlive;
        set => _isAlive = value;
    }
    
    public NavMeshAgent _agent; 
    [SerializeField] private Transform _target; 
    public Animator animator;

    [SerializeField] private float attackInterval = 3f;
    [SerializeField] private int _damageAmount = 5;
    [SerializeField] private int _deathTime = 3;

    private void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _target = GameObject.FindGameObjectWithTag("Gate").transform;
        animator = GetComponent<Animator>();
        SetState(AliveState);
    }

    private void Update()
    {
        _currentState.UpdateState(this);
    }
    public void SetState(EnemyBaseState newState)
    {
        if (_currentState != null)
        {
            _currentState.ExitState(this);
        }
        _currentState = newState;
        _currentState.EnterState(this);
    }

    public void Death()
    {
        SetState(DyingState);
    }

    public IEnumerator Dying()
    {
        yield return new WaitForSeconds(_deathTime);
        Destroy(this.gameObject);
    }

    public void SetNavigation()
    {
        _agent.destination = _target.transform.position;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.GameObject().CompareTag("Gate"))
        {
            _isAtGate = true;
            _door = other.GetComponent<Door>();
            if (_isAlive)
            {
                StartCoroutine("Attack");
            }
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.GameObject().CompareTag("Gate"))
        {
            _isAtGate = false;
            _door = null;
            StopCoroutine("Attack");
        }
    }

    public IEnumerator Attack()
    {
        WaitForSeconds attackTime = new WaitForSeconds(attackInterval);
        while (_isAtGate)
        {
            animator.SetTrigger("Attack");
            DealDamage();
            yield return attackTime;
        }
    }

    private void DealDamage()
    {
        if (_door != null)
        {
            _door.Hurt(_damageAmount);
        }
    }
}
