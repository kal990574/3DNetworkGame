using UnityEngine;
using Photon.Pun;

public class PlayerWeaponHitAbility : PlayerAbility
{
    private void OnTriggerEnter(Collider other)
    {
        if (!photonView.IsMine) return;
        if (_owner.IsDead) return;

        if (other.transform == _owner.transform) return;

        var damageable = other.GetComponentInParent<IDamageable>();
        if (damageable != null)
        {
            PhotonView targetView = other.GetComponentInParent<PhotonView>();
            if (targetView == null) return;

            int actorNumber = PhotonNetwork.LocalPlayer.ActorNumber;
            targetView.RPC(nameof(damageable.TakeDamage), RpcTarget.All, _owner.Stat.Damage, actorNumber);

            _owner.GetAbility<PlayerWeaponColliderAbility>().DeActiveCollider();
        }
    }
}
