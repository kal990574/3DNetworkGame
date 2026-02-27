using System;

[Serializable]
public class EnemyStat
{
    public float MaxHp = 200f;
    public float CurrentHp;
    public float Damage = 25f;
    public float MoveSpeed = 3f;
    public float ChaseSpeed = 5f;
    public float DetectionRange = 10f;
    public float AttackRange = 2.5f;
    public float AttackCooldown = 2f;
}