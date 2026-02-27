using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviourPunCallbacks, IPunObservable, IDamageable
{
    public EnemyStat Stat;
    public bool IsDead { get; private set; }

    public Animator Animator { get; private set; }
    public NavMeshAgent Agent { get; private set; }
    public EEnemyState CurrentStateType { get; private set; }

    [SerializeField] private LayerMask _playerLayer;
    [SerializeField] private Collider _attackCollider;
    [SerializeField] private Collider _bodyCollider;

    private StateMachine _stateMachine;
    private Dictionary<EEnemyState, IState> _states;

    private Vector3 _networkPosition;
    private Quaternion _networkRotation;
    private const float LERP_SPEED = 10f;

    private void Awake()
    {
        Animator = GetComponentInChildren<Animator>();
        Agent = GetComponent<NavMeshAgent>();
        _stateMachine = new StateMachine();
    }

    private void Start()
    {
        Stat.CurrentHp = Stat.MaxHp;
        DeActiveAttackCollider();

        if (PhotonNetwork.IsMasterClient)
        {
            InitializeBearStates();
        }
        else
        {
            Agent.enabled = false;
        }
    }

    private void InitializeBearStates()
    {
        _states = new Dictionary<EEnemyState, IState>
        {
            { EEnemyState.Idle, new BearIdleState(this) },
            { EEnemyState.Patrol, new BearPatrolState(this) },
            { EEnemyState.Chase, new BearChaseState(this) },
            { EEnemyState.Attack, new BearAttackState(this) },
            { EEnemyState.Dead, new BearDeadState(this) },
        };

        ChangeState(EEnemyState.Idle);
    }

    private void Update()
    {
        if (IsDead) return;

        if (PhotonNetwork.IsMasterClient)
        {
            _stateMachine.Update();
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, _networkPosition, Time.deltaTime * LERP_SPEED);
            transform.rotation = Quaternion.Lerp(transform.rotation, _networkRotation, Time.deltaTime * LERP_SPEED);
        }
    }

    public void ChangeState(EEnemyState stateType)
    {
        if (IsDead && stateType != EEnemyState.Dead) return;

        CurrentStateType = stateType;
        _stateMachine.ChangeState(_states[stateType]);
    }

    public Transform FindNearestPlayer()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, Stat.DetectionRange, _playerLayer);

        Transform nearest = null;
        float minDistance = float.MaxValue;

        foreach (Collider col in colliders)
        {
            PlayerController player = col.GetComponentInParent<PlayerController>();
            if (player == null || player.IsDead) continue;

            float distance = Vector3.Distance(transform.position, col.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                nearest = player.transform;
            }
        }

        return nearest;
    }

    public void ActiveAttackCollider()
    {
        _attackCollider.enabled = true;
    }

    public void DeActiveAttackCollider()
    {
        _attackCollider.enabled = false;
    }

    [PunRPC]
    public void PlayAttackAnimation(int attackIndex)
    {
        Animator.SetTrigger($"Attack{attackIndex}");
    }

    [PunRPC]
    public void PlayDeathAnimation()
    {
        Animator.SetTrigger("Death");
        if (_bodyCollider != null) _bodyCollider.enabled = false;
    }

    public void OnAttackTriggerEnter(Collider other)
    {
        if (!PhotonNetwork.IsMasterClient) return;
        if (IsDead) return;

        PlayerController player = other.GetComponentInParent<PlayerController>();
        if (player == null || player.IsDead) return;

        player.PhotonView.RPC(nameof(TakeDamage), RpcTarget.All, Stat.Damage, -1);
        DeActiveAttackCollider();
    }

    [PunRPC]
    public void TakeDamage(float damage, int attackerActorNumber)
    {
        if (IsDead) return;

        Stat.CurrentHp -= damage;

        if (Stat.CurrentHp <= 0f)
        {
            Stat.CurrentHp = 0f;
            IsDead = true;

            if (PhotonNetwork.IsMasterClient)
            {
                ChangeState(EEnemyState.Dead);
            }
        }
    }

    public override void OnMasterClientSwitched(Photon.Realtime.Player newMasterClient)
    {
        if (IsDead) return;

        if (PhotonNetwork.IsMasterClient)
        {
            Agent.enabled = true;
            InitializeBearStates();
        }
        else
        {
            Agent.enabled = false;
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
            stream.SendNext(Stat.CurrentHp);
            stream.SendNext((byte)CurrentStateType);
            stream.SendNext(IsDead);
        }
        else if (stream.IsReading)
        {
            _networkPosition = (Vector3)stream.ReceiveNext();
            _networkRotation = (Quaternion)stream.ReceiveNext();
            Stat.CurrentHp = (float)stream.ReceiveNext();
            CurrentStateType = (EEnemyState)(byte)stream.ReceiveNext();
            IsDead = (bool)stream.ReceiveNext();
        }
    }
}