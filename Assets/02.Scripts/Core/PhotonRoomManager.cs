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
    
    private void Awake()
    {
        Instance = this;
    }
    
    public override void OnJoinedRoom()
    {
        _room = PhotonNetwork.CurrentRoom;
        
        OnDataChanged?.Invoke();
        OnRoomJoined?.Invoke();
    }
}
