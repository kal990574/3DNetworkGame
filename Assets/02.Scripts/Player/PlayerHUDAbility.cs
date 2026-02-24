using UnityEngine;
using UnityEngine.UI;

public class PlayerHUDAbility : PlayerAbility
{
    [SerializeField] private Image _staminaBar;

    private PlayerMoveAbility _moveAbility;

    private void Start()
    {
        _moveAbility = _owner.GetAbility<PlayerMoveAbility>();
    }

    private void Update()
    {
        if (!photonView.IsMine) return;
        
        _staminaBar.fillAmount = _moveAbility.CurrentStamina / _owner.Stat.MaxStamina;
    }
}
