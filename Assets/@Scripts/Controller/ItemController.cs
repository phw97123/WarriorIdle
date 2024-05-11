using System.Collections;
using UnityEngine;
using UnityEngine.U2D;
using static Define;

public class ItemController : BaseController
{
    private SpriteRenderer spriteRenderer;
    public CurrencyType CurrencyType { get; private set; }

    private float moveSpeed = 10.0f;

    public override bool Init()
    {
        base.Init();

        CharacterType = ObjectType.Item; 

        spriteRenderer = GetComponent<SpriteRenderer>();
        CurrencyType = SelectType();
        StartCoroutine(CORemove());
        return true;
    }

    private CurrencyType SelectType()
    {
        CurrencyType type = UnityEngine.Random.Range(0, 2) == 0 ? CurrencyType.Gold : CurrencyType.UpgradeStone;

        switch (type)
        {
            case CurrencyType.Gold:
                spriteRenderer.sprite = Managers.ResourceManager.Load<SpriteAtlas>("ItemAtlas.spriteatlas").GetSprite(GOLD_SPRITE);
                break;
            case CurrencyType.UpgradeStone:
                spriteRenderer.sprite = Managers.ResourceManager.Load<SpriteAtlas>("ItemAtlas.spriteatlas").GetSprite(ENHANCESTONE_SPRTE);
                break;
        }

        return type;
    }

    private IEnumerator CORemove()
    {
        yield return new WaitForSeconds(1.5f);

        Transform player = Managers.ObjectManager.Player.transform;

        while (true)
        {
            float distance = Vector3.Distance(transform.position, player.position);
            transform.position = Vector3.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);

            if (distance <= 0.1f)
            {
                Managers.ResourceManager.Destroy(gameObject);
                break; 
            }
            yield return null;
        }
    }
}
