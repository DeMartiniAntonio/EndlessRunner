using UnityEngine;

public class CharacterPickRotation : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 0.35f;

    void Update()
    {
        transform.Rotate(0, rotationSpeed/2, 0);
    }
}