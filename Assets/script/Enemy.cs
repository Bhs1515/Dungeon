using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float speed;

    [SerializeField]
    private SpriteRenderer _spriteRenderer;

    [SerializeField]
    private int hp = 10;

    [SerializeField]
    private Animator _animator;

    private float range = 3f;
    private float wallRange = 1f;

    private Vector2 direction;
    private Vector2 wallDirection;

    private Rigidbody2D rb;

    private LayerMask detectionLayerMask;


    private float _timer;
    
    private int rand;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        detectionLayerMask = LayerMask.GetMask("Detection");
    }

    // Update is called once per frame
    void Update()
    {
        if (hp <= 0)
        {
            _timer += Time.deltaTime;

            if (_timer > 2)
            {
                _timer = 0;
                rand = Random.Range(0, 3) - 1;
            }
            Move();
            Detect();
        }

        else 
        { 
        
        }
    }

    private void Move()
    {
        if(rand != 0)
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
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, range, detectionLayerMask);
        RaycastHit2D wall = Physics2D.Raycast(transform.position, wallDirection, wallRange, detectionLayerMask);
        Debug.DrawRay(transform.position, wallDirection * wallRange, Color.blue);
        Debug.DrawRay(transform.position, direction * range, Color.red);

        if (hit.collider != null && wall.collider != null)
        {
            if(hit.collider.CompareTag("Player"))
            {

            }
            if (!wall.collider.CompareTag("Floor"))
            {
                rand *= -1;
            }
        }
    }

    public void TakeDamage(int damage)
    {
        hp -= damage;
    }
}
