using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class Player : MonoBehaviour
{
    [SerializeField]
    public int _MaxHealthPoint = 100;

    [SerializeField]
    public int _healthPoint = 100;

    [SerializeField]
    private int _attackPower = 10;

    [SerializeField]
    private int speed = 10;

    [SerializeField]
    private float _jump = 10;

    [SerializeField]
    private bool _isJump = false;

    [SerializeField]
    private bool _isAttack = false;

    [SerializeField]
    private SpriteRenderer _spriteRenderer;

    [SerializeField]
    private Animator _animator;

    [SerializeField]
    private float _attackTimerMax;

    [SerializeField]
    private float _attackTimer = 0;

    [SerializeField]
    private float _attackRange = 3;

    private LayerMask _targetLayer;

    private Vector3 direction;

    private int _attackStack = 0;

    private Vector3 _attackPos;

    private float _moveX;
    private Rigidbody2D rb;
    

    public enum PlayerState
    {
        IDLE,
        RUN,
        JUMP,
        ATTACK
    }

    private PlayerState _state;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _state = PlayerState.IDLE;
        rb = GetComponent<Rigidbody2D>();
        _targetLayer = LayerMask.GetMask("Enemy");
        direction = Vector2.right;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Jump();
        Attack();

        if (Input.GetKeyDown(KeyCode.F))
        {
            Debug.Log("���� Attack �׽�Ʈ");
            Attack();
        }
    }

    private void Move()
    {
        _moveX = Input.GetAxisRaw("Horizontal");

        if (_isAttack && !_isJump) _moveX = 0;

            rb.linearVelocity = new Vector2(_moveX * speed, rb.linearVelocity.y);

        if (_moveX == 1) 
        {
            _spriteRenderer.flipX = false;
        }
        
        else if (_moveX == -1) 
        {
            _spriteRenderer.flipX = true;
        }

        if (_moveX != 0) 
        {
            _state = PlayerState.RUN;
            _animator.SetBool("run", true);
        }
        else
            _animator.SetBool("run", false);

        direction = _spriteRenderer.flipX == false ? Vector2.right : Vector2.left;
    }

    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && _isJump == false)
        {
            rb.AddForceY(_jump);
            _isJump = true;
            _animator.SetBool("isJump", true);
            _animator.SetTrigger("jump");
        }
    }

    private void Attack()
    {

        if (_attackStack >= 4)
            _attackStack = 0;

        if (Input.GetMouseButtonDown(0) && _isAttack == false)
        {
            _state = PlayerState.ATTACK;

            _attackStack++;
            _isAttack = true;           
            

            switch (_attackStack)
            {
                case 1:                    
                    _animator.Play("attack");
                    _attackTimer = 0;
                    break;

                case 2:
                    _animator.Play("attack2");
                    _attackTimer = 0;
                    break;

                case 3:
                    _animator.Play("attack3");
                    break;
            }
            
        }

        _attackTimer += Time.deltaTime;

        if (_attackTimer >= _attackTimerMax)
        {
            _attackStack = 0;
            _attackTimer = 0;
            _isAttack = false;

            if(_attackStack == 3)
            {
                Debug.Log("111111");
            }
        }
        
    }

    private void Shield()
    {

    }

    public void AttackEnd()
    {
        _isAttack = false;

        _attackPos = transform.position + direction * 1;

        Collider2D[] hit = Physics2D.OverlapCircleAll(_attackPos, _attackRange, _targetLayer);

        Debug.DrawRay(transform.position, direction * _attackRange, Color.red, 10f);

        foreach(Collider2D hitEnemy in hit)
        if (hitEnemy != null && hitEnemy.CompareTag("Enemy"))
        {

            Debug.Log("afdsasdf");

            Enemy enemy = hitEnemy.GetComponent<Enemy>();

            enemy.Hit(_attackPower);
        }

        
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Floor")){
            _isJump = false;
            _state = PlayerState.IDLE;
            _animator.SetBool("isJump", false);
        }
    }

    private void OnDrawGizmos()
    {
        Vector3 attackPos = transform.position + (_spriteRenderer.flipX ? Vector3.left : Vector3.right) * 1;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPos, _attackRange);
    }

    public void Hit(int damage)
    {
        _healthPoint -= damage;
    }

}
