using UnityEngine;
using UnityEngine.UI;

public class PlayerHUDAbility : PlayerAbility
{
    [SerializeField] private Image _hpBar;
    [SerializeField] private Image _staminaBar;

    private void Update()
    {
        _hpBar.fillAmount = _owner.Stat.CurrentHp / _owner.Stat.MaxHp;
        _staminaBar.fillAmount = _owner.Stat.Stamina.Current / _owner.Stat.Stamina.Max;
    }
}
