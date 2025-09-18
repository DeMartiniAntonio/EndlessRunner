using System.Collections;
using UnityEngine;

public class RoadMovement : MonoBehaviour
{
    private Rigidbody incomingObjectRB;
    [SerializeField] private float speed = 4f;

    private void Start()
    {
        incomingObjectRB = gameObject.GetComponent<Rigidbody>();
        incomingObjectRB.linearVelocity = transform.forward * -speed;
    }


}
