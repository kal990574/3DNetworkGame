using UnityEngine;
using UnityEngine.UI;

public class EnemyHPBar : MonoBehaviour
{
    [SerializeField] private EnemyController _controller;
    [SerializeField] private Image _hpBar;
    [SerializeField] private Canvas _canvas;

    private Transform _cameraTransform;

    private void Start()
    {
        _cameraTransform = Camera.main.transform;
    }

    private void Update()
    {
        _hpBar.fillAmount = _controller.Stat.CurrentHp / _controller.Stat.MaxHp;

        if (_controller.IsDead)
        {
            _canvas.enabled = false;
            return;
        }

        _canvas.transform.forward = _cameraTransform.forward;
    }
}