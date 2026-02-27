using UnityEngine;
using UnityEngine.UI;

public class PlayerHUDAbility : PlayerAbility
{
    [SerializeField] private Image _hpBar;
    [SerializeField] private Image _staminaBar;
    [SerializeField] private Canvas _canvas;

    private Transform _cameraTransform;

    private void Start()
    {
        _cameraTransform = Camera.main.transform;
    }

    private void Update()
    {
        _hpBar.fillAmount = _owner.Stat.CurrentHp / _owner.Stat.MaxHp;
        _staminaBar.fillAmount = _owner.Stat.Stamina.Current / _owner.Stat.Stamina.Max;

        _canvas.transform.forward = _cameraTransform.forward;
    }
}
