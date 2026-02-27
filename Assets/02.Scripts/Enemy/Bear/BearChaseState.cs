using UnityEngine;

public class BearChaseState : IState
{
    private readonly EnemyController _controller;
    private readonly float _loseRange;

    public BearChaseState(EnemyController controller)
    {
        _controller = controller;
        _loseRange = controller.Stat.DetectionRange * 1.5f;
    }

    public void Enter()
    {
        _controller.Agent.isStopped = false;
        _controller.Agent.speed = _controller.Stat.ChaseSpeed;
        _controller.Animator.SetBool("IsMoving", true);
        _controller.Animator.SetBool("IsRunning", true);
    }

    public void Execute()
    {
        Transform target = _controller.FindNearestPlayer();

        if (target == null || Vector3.Distance(_controller.transform.position, target.position) > _loseRange)
        {
            _controller.ChangeState(EEnemyState.Idle);
            return;
        }

        float distance = Vector3.Distance(_controller.transform.position, target.position);
        if (distance <= _controller.Stat.AttackRange)
        {
            _controller.ChangeState(EEnemyState.Attack);
            return;
        }

        _controller.Agent.SetDestination(target.position);
    }

    public void Exit()
    {
        _controller.Animator.SetBool("IsMoving", false);
        _controller.Animator.SetBool("IsRunning", false);
    }
}