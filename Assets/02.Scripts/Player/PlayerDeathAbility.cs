using System.Collections;
using Photon.Pun;
using UnityEngine;

public class PlayerDeathAbility : PlayerAbility
{
    private const float RESPAWN_DELAY = 3f;

    [SerializeField] private float _fallThreshold = -10f;

    private CharacterController _characterController;
    private Animator _animator;
    private SpawnPositionManager _spawnPositionManager;

    private void Start()
    {
        _characterController = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();
        _spawnPositionManager = FindFirstObjectByType<SpawnPositionManager>();
    }

    private void Update()
    {
        if (!photonView.IsMine) return;
        if (_owner.IsDead) return;

        if (transform.position.y < _fallThreshold)
        {
            photonView.RPC(nameof(Die), RpcTarget.All);
        }
    }

    [PunRPC]
    public void Die()
    {
        if (_owner.IsDead) return;

        _owner.IsDead = true;
        _owner.Stat.CurrentHp = 0f;
        _characterController.enabled = false;
        _animator.SetBool("IsDead", true);

        if (photonView.IsMine)
        {
            _owner.Stat.Score /= 2;
            StartCoroutine(RespawnCoroutine());
        }
    }

    private IEnumerator RespawnCoroutine()
    {
        yield return new WaitForSeconds(RESPAWN_DELAY);

        var (position, rotation) = _spawnPositionManager.GetRandomSpawnPoint();
        photonView.RPC(nameof(Respawn), RpcTarget.All, position, rotation);
    }

    [PunRPC]
    private void Respawn(Vector3 position, Quaternion rotation)
    {
        transform.position = position;
        transform.rotation = rotation;

        _characterController.enabled = true;

        _owner.Stat.CurrentHp = _owner.Stat.MaxHp;
        _owner.Stat.Stamina.Current = _owner.Stat.Stamina.Max;
        _owner.IsDead = false;

        _animator.Rebind();
    }
}