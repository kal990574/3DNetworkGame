using UnityEngine;
using Photon.Pun;

public class PlayerWeaponHitAbility : PlayerAbility
{
    private void OnTriggerEnter(Collider other)
    {
        if (!photonView.IsMine) return;
        
        if (other.transform == _owner.transform) return;
        
        if (other.TryGetComponent<IDamageable>(out var damageable))
        {
            PlayerController otherPlayer = other.GetComponent<PlayerController>();
            otherPlayer.PhotonView.RPC(nameof(damageable.TakeDamage), RpcTarget.All, _owner.Stat.Damage);
            
            _owner.GetAbility<PlayerWeaponColliderAbility>().DeActiveCollider();
        }
    }
}
