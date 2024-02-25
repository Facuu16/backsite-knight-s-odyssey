using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [SerializeField] private float _jumpForce;
    [SerializeField] private float _speed;
    [SerializeField] private int _damage;

    [SerializeField] private int _staminaPerAttack;

    [SerializeField] private GameObject _groundCheck;
    [SerializeField] private LayerMask _groundLayer;

    private CapsuleCollider2D _collider;
    private Rigidbody2D _rigidbody;
    private Animator _animator;
    private Attributes _attributes;
    private HitCheck _hitCheck;

    private float _horizontal;
    private bool _isGrounded;

    private Vector2 _colliderOffset, _colliderSize;

    void Awake()
    {   
        _collider = GetComponent<CapsuleCollider2D>();
        _rigidbody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _attributes = GetComponent<Attributes>();
        _hitCheck = transform.Find("Hit Check")
            .gameObject
            .GetComponent<HitCheck>();

        _colliderOffset = _collider.offset;
        _colliderSize = _collider.size;
    }

    void Update()
    {      
        _horizontal = Input.GetAxisRaw("Horizontal");
        _isGrounded = Physics2D.OverlapCircle(_groundCheck.transform.position, 0.01f, _groundLayer);

        _animator.SetBool("isRunning", _horizontal != 0);

        if (_isGrounded)
        {
            _animator.SetBool("isJumping", false);

            if (Input.GetKeyDown(KeyCode.LeftControl) && !_animator.GetBool("isRunning"))
            {
                var isCrouching = _animator.GetBool("isCrouching");

                _animator.SetBool("isCrouching", !isCrouching);

                if (isCrouching)
                {
                    _collider.offset = _colliderOffset;
                    _collider.size = _colliderSize;
                } else
                {
                    _collider.offset = new Vector2(-0.0002023735f, -6.137149e-05f);
                    _collider.size = new Vector2(0.210228f, 0.3110157f);
                }
            }

            if (Input.GetKeyDown(KeyCode.W) && !_animator.GetBool("isCrouching"))
            {
                _rigidbody.AddForce(Vector2.up * _jumpForce);
                _animator.SetBool("isJumping", true);
            }

            if (!_animator.GetBool("isAttacking") && !_animator.GetBool("isRunning") && !_animator.GetBool("isCrouching"))
            {
                if (Input.GetMouseButtonDown(0) && _attributes.Stamina >= 5)
                {
                    _animator.SetBool("isAttacking", true);
                    _attributes.Stamina -= _staminaPerAttack;

                    foreach (var collision in _hitCheck.Collisions)
                    {
                        collision.GetComponent<Attributes>().Damage(_damage);
                    }

                    Invoke(nameof(DisableAttacking), 0.25f);
                }
            } 
        }
        
        if (_horizontal < 0.0f)
        {
            transform.localScale = new Vector3(-5.7f, 5.6f, 1.0f);
        } else if(_horizontal > 0.0f)
        {
            transform.localScale = new Vector3(5.7f, 5.6f, 1.0f);
        }

    }

    private void DisableAttacking()
    {
        _animator.SetBool("isAttacking", false);
    }

    private void FixedUpdate()
    {
        if (_animator.GetBool("isRunning"))
        {
            transform.position += new Vector3(_horizontal * _speed * Time.fixedDeltaTime, 0f, 0f);
        }
    }

}
