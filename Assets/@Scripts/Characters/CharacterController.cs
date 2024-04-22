using System.Collections;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    protected int hp; 

    // Component
    public Animator Animator { get; protected set; }
    protected Rigidbody2D _rigidbody;
    protected Collider2D _collider;
    protected SpriteRenderer _sprite;

    protected void Init()
    {
        Animator = GetComponent<Animator>();
        _rigidbody = GetComponent<Rigidbody2D>();
        _collider = GetComponent<Collider2D>();
        _sprite = GetComponent<SpriteRenderer>();
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
        hp -= damage;
        if (hp <= 0)
            OnDead();
    }

    public virtual void OnDead()
    {
        gameObject.SetActive(false);
    }

    public virtual void OnKnockback(Vector2 force)
    {
        StartCoroutine(ApplyKncokback(force));
    }
    
    private IEnumerator ApplyKncokback(Vector2 force)
    {
        _rigidbody.velocity = force;

        yield return new WaitForSeconds(0.1f);
        _rigidbody.velocity = Vector2.zero; 
    }
}
