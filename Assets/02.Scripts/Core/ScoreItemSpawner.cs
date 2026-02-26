using System.Collections;
using Photon.Pun;
using UnityEngine;

public class ScoreItemSpawner : MonoBehaviourPunCallbacks
{
    [Header("스폰 영역")]
    [SerializeField] private Vector2 _spawnAreaMin = new Vector2(-20f, -20f);
    [SerializeField] private Vector2 _spawnAreaMax = new Vector2(40f, 33f);
    [SerializeField] private float _spawnHeight = 30f;

    [Header("스폰 간격 (초)")]
    [SerializeField] private float _minInterval = 3f;
    [SerializeField] private float _maxInterval = 8f;

    private Coroutine _spawnCoroutine;

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
        StopSpawn();

        if (PhotonNetwork.IsMasterClient)
        {
            StartSpawn();
        }
    }

    private void HandleRoomJoined()
    {
        if (!PhotonNetwork.IsMasterClient) return;

        StartSpawn();
    }

    private void StartSpawn()
    {
        _spawnCoroutine = StartCoroutine(SpawnRoutine());
    }

    private void StopSpawn()
    {
        if (_spawnCoroutine != null)
        {
            StopCoroutine(_spawnCoroutine);
            _spawnCoroutine = null;
        }
    }

    private IEnumerator SpawnRoutine()
    {
        while (true)
        {
            float interval = Random.Range(_minInterval, _maxInterval);
            yield return new WaitForSeconds(interval);

            SpawnScoreItem();
        }
    }

    private void SpawnScoreItem()
    {
        float x = Random.Range(_spawnAreaMin.x, _spawnAreaMax.x);
        float z = Random.Range(_spawnAreaMin.y, _spawnAreaMax.y);
        Vector3 spawnPosition = new Vector3(x, _spawnHeight, z);

        PhotonNetwork.InstantiateRoomObject("ScoreItem", spawnPosition, Quaternion.identity);
    }
}