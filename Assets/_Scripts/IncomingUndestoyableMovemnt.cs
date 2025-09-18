using System.Collections;
using UnityEngine;

public class IncomingUndestoyableMovemnt : MonoBehaviour
{
    private Rigidbody incomingObjectRB;
    [SerializeField] private float speed = -5f;

    private void Start()
    {
        incomingObjectRB = gameObject.GetComponent<Rigidbody>();
        incomingObjectRB.linearVelocity = transform.forward * -speed;
        
    }

}
