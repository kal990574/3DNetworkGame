using UnityEngine;
using UnityEngine.AI;

public class BearPatrolState : IState
{
    private readonly EnemyController _controller;
    private const float PATROL_RADIUS = 10f;

    public BearPatrolState(EnemyController controller)
    {
        _controller = controller;
    }

    public void Enter()
    {
        _controller.Agent.isStopped = false;
        _controller.Agent.speed = _controller.Stat.MoveSpeed;
        _controller.Animator.SetBool("IsMoving", true);
        _controller.Animator.SetBool("IsRunning", false);
        SetRandomPatrolPoint();
    }

    public void Execute()
    {
        Transform target = _controller.FindNearestPlayer();
        if (target != null)
        {
            _controller.ChangeState(EEnemyState.Chase);
            return;
        }

        if (!_controller.Agent.pathPending && _controller.Agent.remainingDistance <= _controller.Agent.stoppingDistance)
        {
            _controller.ChangeState(EEnemyState.Idle);
        }
    }

    public void Exit()
    {
        _controller.Animator.SetBool("IsMoving", false);
    }

    private void SetRandomPatrolPoint()
    {
        Vector3 randomDirection = Random.insideUnitSphere * PATROL_RADIUS;
        randomDirection += _controller.transform.position;

        if (NavMesh.SamplePosition(randomDirection, out NavMeshHit hit, PATROL_RADIUS, NavMesh.AllAreas))
        {
            _controller.Agent.SetDestination(hit.position);
        }
    }
}