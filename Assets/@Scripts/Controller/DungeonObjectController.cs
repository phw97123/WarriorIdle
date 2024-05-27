using System;
using System.Collections;
using TMPro;
using UnityEngine;
using static Define;
using UnityEngine.U2D;

public class DungeonObjectController : EnemyController
{
    private SpriteRenderer _spriteRenderer;
    private bool _isShaking = false;

    public event Action<int> OnTakeDamage;
    private CurrencyType _rewardType;

    private int _takeDamage; 

    public override bool Init()
    {
        ObjectType = Define.ObjectType.DungeonObject;
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        OnDeath -= Managers.GameManager.Rewards; 
        OnDeath += Managers.GameManager.Rewards; 
        return true;
    }

    public void SetData(CurrencyType type)
    {
        _takeDamage = 0;
        _rewardType = type;
        isDead = false;
        switch (_rewardType)
        {
            case Define.CurrencyType.GoldKey:
                _spriteRenderer.sprite = Managers.ResourceManager.Load<Sprite>(Define.GOLDS_SPRITE);
                break;

            case Define.CurrencyType.UpgradeStoneKey:
                _spriteRenderer.sprite = Managers.ResourceManager.Load<Sprite>(Define.UPGRADESTONES_SPRITE);
                break;
        }
    }

    public override void OnDamaged(int damage, bool critical)
    {
        _takeDamage += damage; 
        OnTakeHit();
        var dtc = Managers.ObjectManager.Spawn<DamageTextController>(_damageTextPos.position);

        dtc.Damage = damage;
        if (critical)
            dtc.SetColor();
        OnTakeDamage?.Invoke(damage);
    }

    public override void OnDead()
    {
        var player = Managers.GameManager.Player; 
        switch (_rewardType)
        {
            case Define.CurrencyType.GoldKey:
                {
                    Sprite sprite = Managers.ResourceManager.Load<SpriteAtlas>(ITEM_SPRITEATLAS).GetSprite(GOLD_SPRITE);
                    RewardData[] rewards = new RewardData[]
                    {
           new RewardData(sprite, _takeDamage / player.PlayerData.Level ,RewardType.Gold),
                    };
                    OnDeath?.Invoke(rewards);
                }
                    break;
            case Define.CurrencyType.UpgradeStoneKey:
                {
                    Sprite sprite = Managers.ResourceManager.Load<SpriteAtlas>(ITEM_SPRITEATLAS).GetSprite(UPGRADESTONE_SPRITE);
                    RewardData[] rewards = new RewardData[]
                    {
           new RewardData(sprite, _takeDamage / player.PlayerData.Level,RewardType.UpgradeStone),
                    };
                    OnDeath?.Invoke(rewards);
                }
                break;
        }
        isDead = true;
        Managers.ObjectManager.Despawn(this);
    }

    public override void OnTakeHit()
    {
        if (!_isShaking)
            StartCoroutine(Shake());
    }

    private IEnumerator Shake()
    {
        _isShaking = true;
        float duration = .8f;
        float shakePower = 0.2f;
        float elapsed = 0f;
        Vector2 origin = transform.position;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float remainingTime = duration - elapsed;
            float currentShakePower = shakePower * (remainingTime / duration);

            float offsetX = Mathf.PerlinNoise(Time.time * 10f, 0f) * 2f - 1f;
            float offsetY = Mathf.PerlinNoise(0f, Time.time * 10f) * 2f - 1f;

            transform.position = origin + new Vector2(offsetX, offsetY) * currentShakePower;
            yield return null;
        }

        transform.position = origin;
        _isShaking = false;
    }
}
