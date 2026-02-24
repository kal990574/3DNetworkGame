using System;

[Serializable]
public class Stamina
{
    public float Max = 100f;
    public float Current;
    public float DrainRate = 20f;
    public float RecoveryRate = 15f;
    public float JumpCost = 10f;
    public float AttackCost = 15f;

    public bool TryConsume(float amount)
    {
        if (Current < amount) return false;
        Current -= amount;
        return true;
    }

    public void Drain(float amount)
    {
        Current = Math.Max(Current - amount, 0f);
    }

    public void Recover(float amount)
    {
        Current = Math.Min(Current + amount, Max);
    }
}