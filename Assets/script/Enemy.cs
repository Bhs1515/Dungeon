using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float speed;

    [SerializeField]
    private SpriteRenderer _spriteRenderer;

    [SerializeField]
    private int hp = 10;

    [SerializeField]
    private int _attackPower = 10;

    [SerializeField]
    private Animator _animator;

    [SerializeField]
    private bool _isAttack = false;

    [SerializeField]
    public Player player;

    [SerializeField]
    private float _knockbackForce;

    private float range = 3f;
    private float wallRange = 1f;
    private float _attackRange = 1f;

    private Vector2 direction;
    private Vector2 wallDirection;
    private Vector2 forceDir;
    private Rigidbody2D rb;
    

    private LayerMask detectionLayerMask;
    private LayerMask playerLayerMask;

    bool _isHit = false;
    bool _isBattle = false;

    private float _timer;
    
    private int rand;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        detectionLayerMask = LayerMask.GetMask("Detection");
        playerLayerMask = LayerMask.GetMask("Player");
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        if (hp > 0)
        {
            if (!_isHit && !_isAttack)
            {
                if (!_isBattle) 
                    Move();

                Detect();
            }
        }

        else
        {
            Die();
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            Attack();
        }
    }

    private void Move()
    {
        _timer += Time.deltaTime;

        if (_timer > 2 && !_isBattle)
        {
            _timer = 0;
                rand = Random.Range(0, 3) - 1;
        }

        if (rand != 0)
        {
            _animator.SetBool("skeletonWalk", true);
            rb.linearVelocity = new Vector2(rand * speed, rb.linearVelocity.y);

            _spriteRenderer.flipX = rand == -1;

            wallDirection = _spriteRenderer.flipX == false ? new Vector2(1, -1) : new Vector2(-1, -1);
            direction = _spriteRenderer.flipX == false ? Vector2.right : Vector2.left;
        }
        else
        {
            _animator.SetBool("skeletonWalk", false);
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
        }
    }

    private void Detect()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, range, playerLayerMask);
        RaycastHit2D attack = Physics2D.Raycast(transform.position, direction, _attackRange, playerLayerMask);
        RaycastHit2D wall = Physics2D.Raycast(transform.position, wallDirection, wallRange, detectionLayerMask);
        Debug.DrawRay(transform.position, wallDirection * wallRange, Color.blue);
        Debug.DrawRay(transform.position, direction * _attackRange, Color.red);

        if (hit.collider != null)
        {
            rand = _spriteRenderer.flipX ? -1 : 1;
            if (attack.collider != null && attack.collider.CompareTag("Player"))
            {
                _isBattle = true;
                
                if (!_isAttack)
                {
                    Attack();
                }
                else
                {
                    Debug.Log(23232323);
                }
            }
            else
            {
                _isBattle = false;
            }
            
        }
            
        if (wall.collider != null && !wall.collider.CompareTag("Floor"))
        {
                rand *= -1;
        }
        
    }

    public void Attack()
    {
        _animator.Play("skeletonAttack1");
        _isAttack = true;
        
    }

    public void AttackEnd()
    {
        player.Hit(_attackPower);
        _isAttack = false;
    }

    public void Hit(int damage)
    {
        _isAttack = false;
        rb.linearVelocity = Vector2.zero;
        _isHit = true;
        rb.linearDamping = 10;
        Vector2 knockbackDir = (transform.position - player.transform.position).normalized;
        rb.linearVelocity = knockbackDir * _knockbackForce;

        hp -= damage;
        _animator.Play("skeletonHit");
    }

    public void HitEnd()
    {
        _isHit = false;
        rb.linearDamping = 0;
    }

    public void Die()
    {
        _animator.Play("skeletonDie");
        Destroy(GetComponent<Collider2D>());
        rb.bodyType = RigidbodyType2D.Static;

        _spriteRenderer.sortingOrder = 10;
        gameObject.layer = LayerMask.NameToLayer("Dead");
    }
}
