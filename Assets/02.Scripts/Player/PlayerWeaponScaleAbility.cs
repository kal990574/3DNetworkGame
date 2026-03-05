using UnityEngine;

public class PlayerWeaponScaleAbility : PlayerAbility
{
    [SerializeField] private Transform _weaponTransform;

    private const float SCALE_PER_LEVEL = 0.1f;
    private const int SCORE_PER_LEVEL = 1000;

    private Vector3 _baseScale;
    private int _prevLevel;

    private void Start()
    {
        _baseScale = _weaponTransform.localScale;
    }

    private void Update()
    {
        int currentLevel = _owner.Stat.Score / SCORE_PER_LEVEL;
        if (currentLevel == _prevLevel) return;

        _prevLevel = currentLevel;
        _weaponTransform.localScale = _baseScale + Vector3.one * (currentLevel * SCALE_PER_LEVEL);
    }
}