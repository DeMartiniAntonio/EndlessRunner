using UnityEngine;

public class ScoreAdd : MonoBehaviour
{
    [SerializeField] private int score;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out PlayerMovement player)) {
            GameManager.instance.UpdateScore(score);
        }
    }
}
