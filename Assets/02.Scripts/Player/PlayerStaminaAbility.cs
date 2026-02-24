using UnityEngine;

public class PlayerStaminaAbility : PlayerAbility
{
    private PlayerMoveAbility _moveAbility;

    private void Start()
    {
        _moveAbility = _owner.GetAbility<PlayerMoveAbility>();
        _owner.Stat.Stamina.Current = _owner.Stat.Stamina.Max;
    }

    private void Update()
    {
        if (!photonView.IsMine) return;
        if (_moveAbility.IsSprinting) return;

        _owner.Stat.Stamina.Recover(_owner.Stat.Stamina.RecoveryRate * Time.deltaTime);
    }
}