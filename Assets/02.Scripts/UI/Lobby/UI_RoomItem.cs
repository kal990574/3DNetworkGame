using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_RoomItem : MonoBehaviour
{
    [SerializeField] private TMP_Text _roomNameText;
    [SerializeField] private TMP_Text _masterNameText;
    [SerializeField] private TMP_Text _playerCountText;

    private string _roomName;
    private Button _button;

    public event Action<string> OnClicked;

    private void Awake()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(() => OnClicked?.Invoke(_roomName));
    }

    public void Initialize(string roomName, string masterName, int currentPlayers, int maxPlayers)
    {
        _roomName = roomName;
        _roomNameText.text = roomName;
        _masterNameText.text = masterName;
        _playerCountText.text = $"{currentPlayers}/{maxPlayers}";
    }
}