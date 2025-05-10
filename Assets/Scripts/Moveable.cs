using System;
using UnityEngine;

public class Moveable : MonoBehaviour
{
    public Transform CurrentTarget { get; private set; }
    public bool IsMoving { get; private set; } = false;
    public float MovementSpeed { get; set; } = 20f;

    void Update()
    {
        if (IsMoving && CurrentTarget != null) {
            Vector3 direction = CurrentTarget.position - transform.position;
            transform.Translate(direction.normalized * MovementSpeed * Time.deltaTime, Space.World);
        }
    }

    public void MoveAgainst(Transform target) {
        CurrentTarget = target;
        IsMoving = true;
    }

    public void Stop() {
        CurrentTarget = null;
        IsMoving = false;
    }
}
