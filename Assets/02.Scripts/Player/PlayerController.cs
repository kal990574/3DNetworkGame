using System;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class PlayerController : MonoBehaviour, IPunObservable, IDamageable
{
    public PlayerStat Stat;

    public PhotonView PhotonView;

    public bool IsDead { get; set; }

    private void Awake()
    {
        PhotonView = GetComponent<PhotonView>();
    }

    [PunRPC]
    public void TakeDamage(float damage, int attackerActorNumber)
    {
        if (IsDead) return;

        Stat.CurrentHp -= damage;

        if (Stat.CurrentHp <= 0f)
        {
            Stat.CurrentHp = 0f;
            GetAbility<PlayerDeathAbility>().Die();
            PhotonRoomManager.Instance.OnPlayerDeath(attackerActorNumber, PhotonView.Owner.ActorNumber);
        }
    }
    
    private void Start()
    {
        if (!PhotonView.IsMine) return;

        var copyPosition = FindFirstObjectByType<CopyPosition>();
        if (copyPosition != null)
            copyPosition.SetTarget(transform);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(Stat.CurrentHp);
            stream.SendNext(Stat.Stamina.Current);
            stream.SendNext(IsDead);
        }
        else if(stream.IsReading)
        {
            Stat.CurrentHp = (float)stream.ReceiveNext();
            Stat.Stamina.Current = (float)stream.ReceiveNext();
            IsDead = (bool)stream.ReceiveNext();
        }
    }

    private Dictionary<Type, PlayerAbility> _abilitiesCache = new();
    
    public T GetAbility<T>() where T : PlayerAbility
    {
        var type = typeof(T);

        if (_abilitiesCache.TryGetValue(type, out PlayerAbility ability))
        {
            return ability as T;
        }

        ability = GetComponent<T>();

        if (ability != null)
        {
            _abilitiesCache[ability.GetType()] = ability;

            return ability as T;
        }
        
        throw new Exception($"어빌리티 {type.Name}을 {gameObject.name}에서 찾을 수 없습니다.");
    }
}