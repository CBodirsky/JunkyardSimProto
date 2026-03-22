using System.Collections.Generic;
using UnityEngine;

public class Conveyor : MonoBehaviour
{
    public Vector3 localDirection = Vector3.forward; // direction relative to the conveyor
    public float speed = .25f;
    private static HashSet<Rigidbody> pushedThisFrame = new HashSet<Rigidbody>();

    void FixedUpdate()
    {
        pushedThisFrame.Clear();
    }

    private void OnTriggerStay(Collider other)
    {
        var rb = other.attachedRigidbody;
        if (rb == null) return;

        if (pushedThisFrame.Contains(rb)) return;
        pushedThisFrame.Add(rb);

        Vector3 dir = transform.TransformDirection(localDirection).normalized;
        rb.MovePosition(rb.position + dir * speed * Time.deltaTime);
    }

}
