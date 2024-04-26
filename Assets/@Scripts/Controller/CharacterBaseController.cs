using System.Collections;
using UnityEngine;

public class CharacterBaseController : BaseController
{
    protected int hp;
    public bool isDead = false;

    // Component
    public Animator Animator { get; protected set; }
    protected Rigidbody2D _rigidbody;
    protected Collider2D _collider;
    protected SpriteRenderer _sprite;

    public override bool Init()
    {
        base.Init();
        Animator = GetComponent<Animator>();
        _rigidbody = GetComponent<Rigidbody2D>();
        _collider = GetComponent<Collider2D>();
        _sprite = GetComponent<SpriteRenderer>();

        return true; 
    }

    public void Move(Transform target, float speed)
    {
        Vector3 dir = target.position - transform.position;
        Vector3 newPos = transform.position + dir.normalized * Time.deltaTime * speed;
        _rigidbody.MovePosition(newPos);
        _sprite.flipX = dir.x < 0;
    }

    public virtual void OnDemeged(int damage)
    {
        if (isDead)
            return;

        hp -= damage;
        if (hp <= 0)
        {
            isDead = true;
            hp = 0;
            OnDead();
        }
        else
            OnTakeHit();
    }

    public virtual void OnDead()
    {
    }

    public virtual void OnKnockback(Vector2 force)
    {
        StartCoroutine(COApplyKncokback(force));
    }

    private IEnumerator COApplyKncokback(Vector2 force)
    {
        _rigidbody.velocity = force;

        yield return new WaitForSeconds(0.1f);
        _rigidbody.velocity = Vector2.zero;
    }

    public virtual void OnTakeHit()
    {
        StartCoroutine(COTakeHit());
    }

    private IEnumerator COTakeHit()
    {
        _sprite.color = Color.red;
        yield return new WaitForSeconds(0.3f);
        _sprite.color = Color.white;
    }
}
