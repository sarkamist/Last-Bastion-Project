using TMPro;
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
    #endregion

    void Start()
    {
        Agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if (RoundManager.Instance.IsGameover)
        {
            Stop();
        }
    }

    public void MoveTo(Transform target, float stoppingDistance = 0f)
    {
        Agent.destination = target.position;
        Agent.isStopped = false;
        Agent.stoppingDistance = stoppingDistance;
    }

    public void Stop()
    {
        Agent.isStopped = true;
    }
}
