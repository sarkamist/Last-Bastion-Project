using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Moveable : MonoBehaviour
{
    #region Properties
    [Header("Movement Parameters")]
    [SerializeField, ReadOnly]
    public NavMeshAgent _agent = null;
    public NavMeshAgent Agent
    {
        get => _agent;
        private set => _agent = value;
    }

    [SerializeField, ReadOnly]
    private Vector3 _currentTarget;
    public Vector3 CurrentTarget
    {
        get => _currentTarget;
        set => _currentTarget = value;
    }

    [SerializeField, ReadOnly]
    private bool _isMoving = false;
    public bool IsMoving
    {
        get => _isMoving;
        set => _isMoving = value;
    }
    #endregion

    void Start()
    {
        Agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if (RoundManager.Instance.IsGameover)
        {
            DisableMovement();
        }
    }

    public void MoveTo(Transform target, float stoppingDistance = 0f)
    {
        if (IsMoving && CurrentTarget == target.position) return;

        CurrentTarget = target.position;
        Agent.SetDestination(CurrentTarget);
        Agent.stoppingDistance = stoppingDistance;
        EnableMovement();
    }

    public void DisableMovement()
    {
        Agent.isStopped = true;
        IsMoving = false;
    }

    public void EnableMovement()
    {
        Agent.isStopped = false;
        IsMoving = true;
    }
}
