using System.Collections;
using System.Runtime.CompilerServices;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : CharacterBaseController
{
    private int _attackCount = 0;

    // Data
    public AnimationData AnimationData { get; private set; }
    public PlayerData PlayerData { get; private set; }

    // Etc
    private PlayerStateMachine stateMachine;

    private EquipmentData _equippedWeapon;
    private EquipmentData _equippedArmor;

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        AnimationData = new AnimationData();
        PlayerData = new PlayerData();
        stateMachine = new PlayerStateMachine(this);

        PlayerData.Icon = Managers.ResourceManager.Load<Sprite>("Player_Icon.sprite");
        PlayerData.Name = PlayerPrefs.GetString("CurrentPlayerName"); 
        PlayerData.Hp = PlayerData.MaxHp;

        stateMachine.ChangeState(stateMachine.IdleState);

        ObjectType = Define.ObjectType.Player;
        return true;
    }

    private void FixedUpdate()
    {
        stateMachine.Update();
    }

    // Animation Event 
    public void ComboAttack(int attackCount)
    {
        if (attackCount == 3)
            Managers.SoundManager.Play(Define.SWORD3);
        else
            Managers.SoundManager.Play(Define.SWORD2);

        PlayerData.AttackRange = _attackCount < 2 ? PlayerData.AttackRange : PlayerData.LastAttackRange;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, PlayerData.AttackRange);
        if (colliders != null)
        {
            foreach (Collider2D collider in colliders)
            {
                if (collider.CompareTag("Enemy") || collider.CompareTag("DungeonObject"))
                {
                    EnemyController target = collider.GetComponent<EnemyController>();
                    if (target == null || target.isDead) return;

                    int damage = CalculateDamage();
                    bool critical = false;
                    if (IsCriticalHit())
                    {
                        critical = true;
                        damage *= (int)PlayerData.CriticalDamage;
                    }

                    target.OnDamaged(damage, critical);

                    // Knockback
                    if (attackCount == 3 && target.ObjectType != Define.ObjectType.Boss && target.ObjectType != Define.ObjectType.DungeonObject )
                    {
                        Vector2 direction = (collider.transform.position - transform.position).normalized;
                        if (!target.isDead)
                        {
                            target.OnKnockback(direction * PlayerData.KnockbackForce);
                        }
                    }
                }
            }
        }
    }

    private bool IsCriticalHit()
    {
        float randomNum = UnityEngine.Random.Range(0f, 1f);
        return randomNum <= PlayerData.CriticalChance;
    }

    private int CalculateDamage()
    {
        return UnityEngine.Random.Range(PlayerData.Damage - 5, PlayerData.Damage + 10);
    }

    public override void OnDamaged(int damage, bool critical)
    {
        if (isDead)
            return;

        PlayerData.Hp -= damage;
        if (PlayerData.Hp <= 0)
        {
            isDead = true;
            OnDead();
        }

        base.OnDamaged(damage, critical);
    }

    public override void OnDead()
    {
        stateMachine.ChangeState(stateMachine.DeadState);
        StartCoroutine(CORespawn());
    }

    private IEnumerator CORespawn()
    {
        // 부활시간
        yield return new WaitForSeconds(3.0f);

        Managers.ObjectManager.DespawnAllEnemy();

        stateMachine.ChangeState(stateMachine.IdleState);

        PlayerData.Hp = PlayerData.MaxHp;

        // 무적시간
        yield return new WaitForSeconds(1.0f);
        isDead = false;
    }

    public void Equip(EquipmentData equipmentData)
    {
        switch (equipmentData.equipmentType)
        {
            case Define.EquipmentType.Weapon:
                UnEquip(equipmentData.equipmentType);
                _equippedWeapon = equipmentData;
                _equippedWeapon.isEquipped = true;
                PlayerData.Damage += equipmentData.equippedEffect;
                break;
            case Define.EquipmentType.Armor:
                UnEquip(equipmentData.equipmentType);
                _equippedArmor = equipmentData;
                _equippedArmor.isEquipped = true;
                PlayerData.MaxHp += equipmentData.equippedEffect;
                break;

        }
    }

    public void UnEquip(Define.EquipmentType type)
    {
        switch (type)
        {
            case Define.EquipmentType.Weapon:
                if (_equippedWeapon == null) return;
                PlayerData.Damage -= _equippedWeapon.equippedEffect;
                _equippedWeapon.isEquipped = false;
                _equippedWeapon = null;

                break;
            case Define.EquipmentType.Armor:
                if (_equippedArmor == null) return;
                PlayerData.MaxHp -= _equippedArmor.equippedEffect;
                _equippedArmor.isEquipped = false;
                _equippedArmor = null;
                break;
        }
    }

    public void UpgradeEquipment(Define.EquipmentType type, int prevEffect, int newEffect)
    {
        switch(type)
        {
            case Define.EquipmentType.Weapon:
                PlayerData.Damage -= prevEffect;
                PlayerData.Damage += newEffect;
                break;
            case Define.EquipmentType.Armor:
                PlayerData.MaxHp -= prevEffect;
                PlayerData.MaxHp += newEffect;
                break; 
        }
    }

    public void OnPlayerChangedIdleStage()
    {
        stateMachine.ChangeState(stateMachine.IdleState); 
    }
}
