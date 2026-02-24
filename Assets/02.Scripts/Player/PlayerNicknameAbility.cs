using Photon.Pun;
using TMPro;
using UnityEngine;

public class PlayerNicknameAbility : PlayerAbility
{
    [SerializeField] private TextMeshProUGUI _nicknameTextUI;

    private void Start()
    {
        _nicknameTextUI.text = photonView.Owner.NickName;
        if (photonView.IsMine)
        {
            _nicknameTextUI.color = new Color32(100, 255, 255, 255);
        }
        else
        {
            _nicknameTextUI.color = new Color32(255, 255, 255, 255);
        }
    }
}
