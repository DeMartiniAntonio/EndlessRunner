using UnityEngine;

public class CharacterPickRotation : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 30f;

    void Update()
    {
        transform.Rotate(0, rotationSpeed, 0);
    }
}