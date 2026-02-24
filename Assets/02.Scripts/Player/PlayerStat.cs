using System;

[Serializable]
public class PlayerStat
{
    public float MoveSpeed;
    public float SprintSpeedMultiplier = 1.8f;
    public float JumpPower;
    public float RotationSpeed;
    public float AttackSpeed;

    public float MaxStamina = 100f;
    public float CurrentStamina;
    public float StaminaDrainRate = 20f;
    public float StaminaRecoveryRate = 15f;

    public float MaxHp = 100f;
    public float CurrentHp;
}