using UnityEngine;

public enum AttackMode
{
    Sequential,
    Random
}

public class PlayerAttackAbility : MonoBehaviour
{    
    public AttackMode AttackPattern = AttackMode.Sequential;
    public int MaxAttackCount = 3;
    private Animator _animator;

    private float ATTACK_COOLTIME = 0.6f;
    private float _attackTimer = 0f;
    private int _attackIndex = 0;

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }
    
    private void Update()
    {
        _attackTimer += Time.deltaTime;

        if (Input.GetMouseButton(0) && _attackTimer >= ATTACK_COOLTIME)
        {
            _attackTimer = 0f;

            int attackNum = GetAttackNumber();
            _animator.SetTrigger($"Attack{attackNum}");
        }
    }

    private int GetAttackNumber()
    {
        switch (AttackPattern)
        {
            case AttackMode.Sequential:
                _attackIndex = (_attackIndex % MaxAttackCount) + 1;

                return _attackIndex;
            
            case AttackMode.Random:
                return Random.Range(1, MaxAttackCount + 1);
            
            default:
                return 1;
        }
    }
}
