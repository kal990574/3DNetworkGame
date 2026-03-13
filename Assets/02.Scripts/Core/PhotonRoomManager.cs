using System;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class PhotonRoomManager : MonoBehaviourPunCallbacks
{
    public static PhotonRoomManager Instance { get; private set; }

    private Room _room;
    public Room Room => _room;

    public event Action OnDataChanged; 
    public event Action OnRoomJoined;
    public event Action<Player> OnPlayerEnter;
    public event Action<Player> OnPlayerLeft;
    public event Action<string, string> OnPlayerDeathed;
    
    private void Awake()
    {
        Instance = this;
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }

    private void Start()
    {
        if (PhotonNetwork.InRoom)
        {
            _room = PhotonNetwork.CurrentRoom;
            OnDataChanged?.Invoke();
            OnRoomJoined?.Invoke();
        }
    }

    public override void OnJoinedRoom()
    {
        _room = PhotonNetwork.CurrentRoom;
        
        OnDataChanged?.Invoke();
        OnRoomJoined?.Invoke();
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        OnDataChanged?.Invoke();
        OnPlayerEnter?.Invoke(newPlayer);
    }

    public override void OnPlayerLeftRoom(Player player)
    {
        OnDataChanged?.Invoke();
        OnPlayerLeft?.Invoke(player);
    }

    public void OnPlayerDeath(int attackerActorNumber, int victimActorNumber)
    {
        string attackerNickname = _room.Players[attackerActorNumber].NickName;
        string victimNickname = _room.Players[victimActorNumber].NickName;

        OnPlayerDeathed?.Invoke(attackerNickname, victimNickname);
    }
}
