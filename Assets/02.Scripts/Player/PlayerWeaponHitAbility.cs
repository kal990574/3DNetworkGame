using UnityEngine;
using Photon.Pun;

public class PlayerWeaponHitAbility : PlayerAbility
{
    private void OnTriggerEnter(Collider other)
    {
        if (!photonView.IsMine) return;
        if (_owner.IsDead) return;

        if (other.transform == _owner.transform) return;

        if (other.TryGetComponent<IDamageable>(out var damageable))
        {
            int actorNumber = PhotonNetwork.LocalPlayer.ActorNumber;
            PlayerController otherPlayer = other.GetComponent<PlayerController>();
            if (otherPlayer.IsDead) return;

            otherPlayer.PhotonView.RPC(nameof(damageable.TakeDamage), RpcTarget.All, _owner.Stat.Damage, actorNumber);
            
            _owner.GetAbility<PlayerWeaponColliderAbility>().DeActiveCollider();
        }
    }
}
