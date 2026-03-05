using Photon.Pun;
using UnityEngine;

public class PlayerSpawnManager : MonoBehaviour
{
    [SerializeField] private PhotonRoomManager _roomManager;
    [SerializeField] private SpawnPositionManager _spawnPositionManager;

    private void OnEnable()
    {
        if (_roomManager != null)
            _roomManager.OnRoomJoined += SpawnLocalPlayer;
    }

    private void OnDisable()
    {
        if (_roomManager != null)
            _roomManager.OnRoomJoined -= SpawnLocalPlayer;
    }

    private void SpawnLocalPlayer()
    {
        var (position, rotation) = _spawnPositionManager.GetRandomSpawnPoint();
        PhotonNetwork.Instantiate("Player", position, rotation);
    }
}