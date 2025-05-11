using System;
using UnityEngine;

public class Moveable : MonoBehaviour
{
    #region Properties
    [SerializeField, ReadOnly]
    [Header("Targeting")]
    public Transform _currentWaypoint = null;
    public Transform CurrentWaypoint {
        get => _currentWaypoint;
        private set => _currentWaypoint = value; }

    [Header("Movement Parameters")]
    [SerializeField, ReadOnly]
    public bool _isMoving = false;
    public bool IsMoving { 
        get => _isMoving;
        private set => _isMoving = value;
    }

    [SerializeField]
    public float _movementSpeed = 20f;
    public float MovementSpeed {
        get => _movementSpeed;
        set => _movementSpeed = value;
    }
    #endregion

    void Update()
    {
        if (IsMoving && CurrentWaypoint != null) {
            Vector3 direction = CurrentWaypoint.position - transform.position;
            transform.Translate(MovementSpeed * Time.deltaTime * direction.normalized, Space.World);
        }
    }

    public void MoveAgainst(Transform target) {
        CurrentWaypoint = target;
        IsMoving = true;
    }

    public void Stop() {
        CurrentWaypoint = null;
        IsMoving = false;
    }
}
