using System.Collections;
using Photon.Pun;
using UnityEngine;

public class EnemySpawnManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private Transform[] _spawnPoints;
    [SerializeField] private int _maxBearCount = 3;
    [SerializeField] private float _respawnDelay = 10f;

    private const string BEAR_PREFAB_NAME = "Bear";

    private void Start()
    {
        PhotonRoomManager.Instance.OnRoomJoined += HandleRoomJoined;
    }

    private void OnDestroy()
    {
        PhotonRoomManager.Instance.OnRoomJoined -= HandleRoomJoined;
    }

    public override void OnMasterClientSwitched(Photon.Realtime.Player newMasterClient)
    {
    }

    private void HandleRoomJoined()
    {
        if (!PhotonNetwork.IsMasterClient) return;

        SpawnBears();
    }

    private void SpawnBears()
    {
        int count = Mathf.Min(_maxBearCount, _spawnPoints.Length);

        for (int i = 0; i < count; i++)
        {
            Vector3 position = _spawnPoints[i].position;
            Quaternion rotation = _spawnPoints[i].rotation;
            PhotonNetwork.InstantiateRoomObject(BEAR_PREFAB_NAME, position, rotation);
        }
    }

    public void RequestRespawn(Vector3 deathPosition)
    {
        if (!PhotonNetwork.IsMasterClient) return;

        StartCoroutine(RespawnAfterDelay());
    }

    private IEnumerator RespawnAfterDelay()
    {
        yield return new WaitForSeconds(_respawnDelay);

        if (!PhotonNetwork.IsMasterClient) yield break;

        int index = Random.Range(0, _spawnPoints.Length);
        Vector3 position = _spawnPoints[index].position;
        Quaternion rotation = _spawnPoints[index].rotation;
        PhotonNetwork.InstantiateRoomObject(BEAR_PREFAB_NAME, position, rotation);
    }
}