using System;

[Serializable]
public class PlayerStat
{
    public float MoveSpeed;
    public float SprintSpeedMultiplier = 1.8f;
    public float JumpPower;
    public float RotationSpeed;
    public float AttackSpeed;

    public float Damage;

    public Stamina Stamina = new();

    public float MaxHp = 100f;
    public float CurrentHp;
}