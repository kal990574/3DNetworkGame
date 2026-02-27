using System.Collections;
using Photon.Pun;
using UnityEngine;

public class BearDeadState : IState
{
    private readonly EnemyController _controller;
    private const float DESTROY_DELAY = 3f;
    private const float DROP_RADIUS = 2f;

    public BearDeadState(EnemyController controller)
    {
        _controller = controller;
    }

    public void Enter()
    {
        _controller.Agent.isStopped = true;
        _controller.Agent.enabled = false;
        _controller.DeActiveAttackCollider();
        _controller.photonView.RPC(nameof(EnemyController.PlayDeathAnimation), RpcTarget.All);

        if (PhotonNetwork.IsMasterClient)
        {
            DropItems();
            _controller.StartCoroutine(DestroyAfterDelay());
        }
    }

    public void Execute() { }

    public void Exit() { }

    private void DropItems()
    {
        int itemCount = Random.Range(3, 8);
        Vector3 deathPosition = _controller.transform.position;

        for (int i = 0; i < itemCount; i++)
        {
            Vector2 randomOffset = Random.insideUnitCircle * DROP_RADIUS;
            Vector3 spawnPosition = deathPosition + new Vector3(randomOffset.x, 0.5f, randomOffset.y);
            PhotonNetwork.InstantiateRoomObject("ScoreItem", spawnPosition, Quaternion.identity);
        }
    }

    private IEnumerator DestroyAfterDelay()
    {
        yield return new WaitForSeconds(DESTROY_DELAY);

        EnemySpawnManager spawner = Object.FindFirstObjectByType<EnemySpawnManager>();
        if (spawner != null)
        {
            spawner.RequestRespawn(_controller.transform.position);
        }

        PhotonNetwork.Destroy(_controller.gameObject);
    }
}