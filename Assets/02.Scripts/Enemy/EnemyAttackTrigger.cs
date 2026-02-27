using UnityEngine;

public class EnemyAttackTrigger : MonoBehaviour
{
    private EnemyController _controller;

    private void Awake()
    {
        _controller = GetComponentInParent<EnemyController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        _controller.OnAttackTriggerEnter(other);
    }
}