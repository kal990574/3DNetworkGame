using UnityEngine;

public class BearIdleState : IState
{
    private readonly EnemyController _controller;
    private float _idleTimer;
    private float _idleDuration;

    public BearIdleState(EnemyController controller)
    {
        _controller = controller;
    }

    public void Enter()
    {
        _controller.Agent.isStopped = true;
        _controller.Animator.SetBool("IsMoving", false);
        _controller.Animator.SetBool("IsRunning", false);
        _idleTimer = 0f;
        _idleDuration = Random.Range(2f, 5f);
    }

    public void Execute()
    {
        Transform target = _controller.FindNearestPlayer();
        if (target != null)
        {
            _controller.ChangeState(EEnemyState.Chase);
            return;
        }

        _idleTimer += Time.deltaTime;
        if (_idleTimer >= _idleDuration)
        {
            _controller.ChangeState(EEnemyState.Patrol);
        }
    }

    public void Exit() { }
}