using System;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class UI_RoomItemList : MonoBehaviourPunCallbacks
{
    [SerializeField] private Transform _content;
    [SerializeField] private UI_RoomItem _roomItemPrefab;

    private readonly List<UI_RoomItem> _roomItems = new();

    public event Action<string> OnRoomSelected;

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        ClearRoomList();

        foreach (RoomInfo info in roomList)
        {
            if (!info.IsOpen || !info.IsVisible || info.RemovedFromList)
                continue;

            string masterName = "";
            if (info.CustomProperties.TryGetValue(UI_Lobby.ROOM_MASTER_NAME_KEY, out object nameObj))
                masterName = nameObj.ToString();

            UI_RoomItem item = Instantiate(_roomItemPrefab, _content);
            item.Initialize(info.Name, masterName, info.PlayerCount, info.MaxPlayers);
            item.OnClicked += HandleRoomClicked;
            _roomItems.Add(item);
        }
    }

    private void HandleRoomClicked(string roomName)
    {
        OnRoomSelected?.Invoke(roomName);
    }

    private void ClearRoomList()
    {
        foreach (UI_RoomItem item in _roomItems)
        {
            item.OnClicked -= HandleRoomClicked;
            Destroy(item.gameObject);
        }

        _roomItems.Clear();
    }
}