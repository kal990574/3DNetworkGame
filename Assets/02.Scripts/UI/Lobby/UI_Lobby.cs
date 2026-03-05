using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Lobby : MonoBehaviourPunCallbacks
{
    public const string PLAYER_GENDER_KEY = "Gender";
    public const string ROOM_MASTER_NAME_KEY = "MasterName";

    public GameObject MaleCharacter;
    public GameObject FemaleCharacter;

    public TMP_InputField NicknameInputField;
    public TMP_InputField RoomNameInputField;
    public Button CreateRoomButton;

    [Header("룸 목록")]
    [SerializeField] private UI_RoomItemList _roomItemList;

    private ECharacterType _characterType;

    private void Start()
    {
        CreateRoomButton.onClick.AddListener(MakeRoom);
        _roomItemList.OnRoomSelected += JoinRoom;
    }

    private void OnDestroy()
    {
        _roomItemList.OnRoomSelected -= JoinRoom;
    }

    private void MakeRoom()
    {
        string nickname = NicknameInputField.text;
        string roomName = RoomNameInputField.text;

        if (string.IsNullOrEmpty(nickname) || string.IsNullOrEmpty(roomName))
        {
            return;
        }

        ApplyPlayerSettings();

        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 20;
        roomOptions.IsVisible = true;
        roomOptions.IsOpen = true;
        roomOptions.CustomRoomProperties = new Hashtable { { ROOM_MASTER_NAME_KEY, nickname } };
        roomOptions.CustomRoomPropertiesForLobby = new string[] { ROOM_MASTER_NAME_KEY };

        PhotonNetwork.CreateRoom(roomName, roomOptions);
    }

    private void JoinRoom(string roomName)
    {
        ApplyPlayerSettings();
        PhotonNetwork.JoinRoom(roomName);
    }

    private void ApplyPlayerSettings()
    {
        PhotonNetwork.NickName = NicknameInputField.text;

        Hashtable props = new Hashtable { { PLAYER_GENDER_KEY, _characterType.ToString() } };
        PhotonNetwork.LocalPlayer.SetCustomProperties(props);
    }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel("GameScene");
    }

    public void OnClickMale() => OnClickCharacterButton(ECharacterType.Male);
    public void OnClickFemale() => OnClickCharacterButton(ECharacterType.Female);

    private void OnClickCharacterButton(ECharacterType characterType)
    {
        _characterType = characterType;

        MaleCharacter.SetActive(_characterType == ECharacterType.Male);
        FemaleCharacter.SetActive(_characterType == ECharacterType.Female);
    }
}