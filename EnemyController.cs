using System;
using UnityEngine;

public class EnemyController : MonoBehaviour
{

    [SerializeField] private int _score;
    [SerializeField] private float _followRange;

    [SerializeField] private int _damage;
    [SerializeField] private float _attackRange;
    [SerializeField] private float _attackCooldown;
    [SerializeField] private float _attackDisable;

    [SerializeField] private float _speed;
    [SerializeField] private float _walkCooldown;
    [SerializeField] private float _walkDuration;

    [SerializeField] private float _deathColliderTime;
    [SerializeField] private GameObject[] _destroyOnDeath;

    [SerializeField] private LayerMask _groundLayer;

    private CapsuleCollider2D _collider;
    private BoxCollider2D _deathCollider;
    private Rigidbody2D _rigidbody;
    private Animator _animator;
    private Attributes _attributes;
    private CooldownManager _cooldownManager;
    private GameManager _gameManager;

    private Vector3 _scale;

    private string _entitiyId;
    private string _walkCooldownId;
    private string _attackCooldownId;

    private float _lastWalk;
    private Direction _dir;
    private bool _isWalking;
    private bool _isDead;

    private GameObject _voidCheck;
    private GameObject _target;

    void Awake()
    {
        _collider = GetComponent<CapsuleCollider2D>();
        _deathCollider = GetComponent<BoxCollider2D>();
        _deathCollider.enabled = false;
        _rigidbody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _attributes = GetComponent<Attributes>();

        var API = GameObject.Find("API");

        _cooldownManager = API.GetComponent<CooldownManager>();
        _gameManager = API.GetComponent<GameManager>();

        _voidCheck = transform.Find("Void Check").gameObject;
        _scale = transform.localScale;
        _dir = Direction.LEFT;
        _lastWalk = Time.time;
        _isWalking = false;
        _isDead = false;
        _target = null;
        _entitiyId = Guid.NewGuid().ToString();
        _walkCooldownId = $"walk-{_entitiyId}";
        _attackCooldownId = $"attack-{_entitiyId}";

        _attributes.OnDamaged = () => _target = GameObject.Find("Player");
    }

    void Update()
    {
        _animator.SetBool("isRunning", false);
        _animator.SetBool("isDead", false);

        if (_attributes.Health == 0)
        {
            if (!_isDead)
            {
                _isDead = true;
                _gameManager.Kill();
            }

            if (_isWalking)
                _isWalking = false;

            foreach (GameObject obj in _destroyOnDeath)
                Destroy(obj);

            _animator.SetBool("isAttacking", false);
            _animator.SetBool("isDead", true);
            Invoke(nameof(DeathCollider), _deathColliderTime);
            Invoke(nameof(Destroy), 5);
            return;
        }

        if (_target != null)
        {
            var targetDir = GetTargetDirection();

            if (GetTargetDistance() <= _attackRange && _dir == targetDir)
            {
                if (_isWalking)
                    _isWalking = false;

                if (!_cooldownManager.IsInCooldown(_attackCooldownId) && !_animator.GetBool("isAttacking"))
                {
                    _animator.SetBool("isAttacking", true);
                    _cooldownManager.AddCooldown(_attackCooldownId, _attackCooldown);

                    Invoke(nameof(DamageTarget), _attackDisable);
                }
            } 
            else
            {
                if (GetTargetDistance() <= _followRange)
                {
                    _dir = targetDir;
                }
                else
                {
                    _dir = targetDir == Direction.RIGHT ? Direction.LEFT : targetDir;
                    _target = null;
                }
            }
        }

        if (!_isWalking)
        {
            if (_target != null || !_cooldownManager.IsInCooldown(_walkCooldownId))
                _isWalking = true;
        } 
        else
        {
            if (!Physics2D.OverlapCircle(_voidCheck.transform.position, 0.01f, _groundLayer))
                ChangeDirection();

            if ((Time.time - _lastWalk) > _walkDuration)
            {
                _cooldownManager.AddCooldown(_walkCooldownId, _walkCooldown);
                _isWalking = false;
                _lastWalk = Time.time;
                return;
            }

            Walk(_dir);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        var obj = collision.gameObject;

        if (obj.layer == 6)
            return;

        if (obj.CompareTag("Player"))
        {
            _target = obj;
            return;
        }

        ChangeDirection();
    }

    private void ChangeDirection()
    {
        if (_dir == Direction.RIGHT)
            _dir = Direction.LEFT;

        else if (_dir == Direction.LEFT)
            _dir = Direction.RIGHT;
    }

    private void DamageTarget()
    {
        _target.GetComponent<Attributes>().Damage(_damage);
        _animator.SetBool("isAttacking", false);
    }

    private float GetTargetDistance()
    {
        return Vector3.Distance(_target.transform.position, transform.position);
    }

    private Direction GetTargetDirection()
    {
        return transform.position.x > _target.transform.position.x ? Direction.LEFT : Direction.RIGHT;
    }

    public void Walk(Direction direction)
    {
        if (_animator.GetBool("isAttacking"))
        {
            _animator.SetBool("isAttacking", false);
            return;
        }

        _animator.SetBool("isRunning", true);

        switch (direction)
        {
            case Direction.LEFT:
                transform.localScale = _scale;
                transform.position += new Vector3(-1 * _speed * Time.deltaTime, 0f, 0f);
                break;

            case Direction.RIGHT:
                transform.localScale = new Vector3(-_scale.x, _scale.y, _scale.z);
                transform.position += new Vector3(1 * _speed * Time.deltaTime, 0f, 0f);
                break;
        }
    }

    private void DeathCollider()
    {
        _collider.enabled = false;
        _deathCollider.enabled = true;
    }

    private void Destroy()
    {
        Destroy(gameObject);
    }

}

public enum Direction
{
    LEFT,
    RIGHT
}
