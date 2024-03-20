using UnityEngine;

public class CharacterController : MonoBehaviour
{
    // TODO : 데이터 만들기
    protected float speed = 15.0f;
    protected int hp = 100; 

    // Component
    public Animator Animator { get; protected set; }
    protected Rigidbody2D _rigidbody;
    protected Collider2D _collider;
    protected SpriteRenderer _sprite;

    public void Move(Transform target)
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
        _rigidbody.AddForce(force, ForceMode2D.Impulse); 
    }
}
