using Photon.Pun;
using UnityEngine;

public class BearAttackState : IState
{
    private readonly EnemyController _controller;
    private float _attackTimer;

    public BearAttackState(EnemyController controller)
    {
        _controller = controller;
    }

    public void Enter()
    {
        _controller.Agent.isStopped = true;
        _controller.Animator.SetBool("IsMoving", false);
        _controller.Animator.SetBool("IsRunning", false);
        _attackTimer = _controller.Stat.AttackCooldown;
    }

    public void Execute()
    {
        Transform target = _controller.FindNearestPlayer();

        if (target == null)
        {
            _controller.ChangeState(EEnemyState.Idle);
            return;
        }

        float distance = Vector3.Distance(_controller.transform.position, target.position);
        if (distance > _controller.Stat.AttackRange)
        {
            _controller.ChangeState(EEnemyState.Chase);
            return;
        }

        LookAtTarget(target);

        _attackTimer += Time.deltaTime;
        if (_attackTimer >= _controller.Stat.AttackCooldown)
        {
            _attackTimer = 0f;
            Attack();
        }
    }

    public void Exit()
    {
        _controller.DeActiveAttackCollider();
    }

    private void LookAtTarget(Transform target)
    {
        Vector3 direction = (target.position - _controller.transform.position).normalized;
        direction.y = 0f;

        if (direction == Vector3.zero) return;

        Quaternion lookRotation = Quaternion.LookRotation(direction);
        _controller.transform.rotation = Quaternion.Slerp(
            _controller.transform.rotation, lookRotation, Time.deltaTime * 5f);
    }

    private void Attack()
    {
        int attackIndex = Random.Range(1, 4);
        _controller.photonView.RPC(nameof(EnemyController.PlayAttackAnimation), RpcTarget.All, attackIndex);
    }
}