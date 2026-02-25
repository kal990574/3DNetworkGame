using Photon.Pun;
using UnityEngine;

public class PlayerSpawnManager : MonoBehaviour
{
    [SerializeField] private PhotonRoomManager _roomManager;
    [SerializeField] private SpawnPositionManager _spawnPositionManager;

    private void OnEnable()
    {
        _roomManager.OnRoomJoined += SpawnLocalPlayer;
    }

    private void OnDisable()
    {
        _roomManager.OnRoomJoined -= SpawnLocalPlayer;
    }

    private void SpawnLocalPlayer()
    {
        var (position, rotation) = _spawnPositionManager.GetRandomSpawnPoint();
        PhotonNetwork.Instantiate("Player", position, rotation);
    }
}