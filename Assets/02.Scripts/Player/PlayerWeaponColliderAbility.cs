using System;
using UnityEngine;

public class PlayerWeaponColliderAbility : PlayerAbility
{
    [SerializeField] private Collider _collider;

    private void Start()
    {
        DeActiveCollider();
    }

    public void ActiveCollider()
    {
        _collider.enabled = true;
    }
    
    public void DeActiveCollider()
    {
        _collider.enabled = false;
    }
}
