using System.Collections;
using UnityEngine;

public class SofaIncoming : MonoBehaviour
{
    private Rigidbody incomingObjectRB;
    [SerializeField] private float speed = 5f;

    private void Start()
    {
        incomingObjectRB = gameObject.GetComponent<Rigidbody>();
        incomingObjectRB.linearVelocity = transform.forward * -speed;
        StartCoroutine(DestroyObjectAfterTime(12f));
    }

    private IEnumerator DestroyObjectAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }


}
