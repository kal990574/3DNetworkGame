using Photon.Pun;
using UnityEngine;

public class ScoreItem : MonoBehaviourPun
{
    private bool _isPickedUp;
    private Collider _collider;

    private void Awake()
    {
        _collider = GetComponent<Collider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_isPickedUp) return;

        var player = other.GetComponentInParent<PlayerController>();
        if (player == null) return;
        if (player.IsDead) return;
        if (!player.PhotonView.IsMine) return;

        _isPickedUp = true;
        player.AddScore(1);

        photonView.RPC(nameof(RPC_Pickup), RpcTarget.All);
    }

    [PunRPC]
    private void RPC_Pickup()
    {
        _isPickedUp = true;
        _collider.enabled = false;

        if (photonView.IsMine)
        {
            PhotonNetwork.Destroy(gameObject);
        }
    }
}