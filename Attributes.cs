using System;
using UnityEngine;

public class Attributes : MonoBehaviour
{

    [SerializeField] private int _maxHealth;
    [SerializeField] private int _maxStamina;

    [SerializeField] private int _staminaRegenRate;
    [SerializeField] private int _staminaRegenTime;

    [SerializeField] private bool _hasDamagedAnimation;

    private Animator _animator;
    private SpriteRenderer _spriteRenderer;

    private Color _defaultColor;

    private int _health;
    private int _stamina;

    private Action _onDamaged;
    private bool _isDamaged;

    void Awake()
    {
        _animator = GetComponent<Animator>();   
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _defaultColor = _spriteRenderer.color;

        _health = _maxHealth;
        _stamina = _maxStamina;

        if (gameObject.CompareTag("Player"))
            InvokeRepeating(nameof(RegenerateStamina), 0, _staminaRegenTime);
    }

    private void Update()
    {
        if (_hasDamagedAnimation)
            _animator.SetBool("isTakingHit", false);
    }

    private void RegenerateStamina()
    {
        if (_stamina == _maxStamina)
            return;

        if (_stamina > _maxStamina)
            _stamina = _maxStamina;

        Stamina = _stamina + _staminaRegenRate;
    }

    private void PostDamage()
    {
        _spriteRenderer.color = _defaultColor;
        _isDamaged = false;
    }

    public void Damage(int damage)
    {
        if (_hasDamagedAnimation)
            _animator.SetBool("isTakingHit", true);

        _isDamaged = true;
        _onDamaged?.Invoke();
        Health -= damage;
        _spriteRenderer.color = Color.red;
        Invoke("PostDamage", 0.5f);
    }

    public Action OnDamaged
    {
        set { _onDamaged = value; }

        get { return _onDamaged; }
    }

    public bool IsDamaged
    {
        get { return _isDamaged; }
    }

    public int MaxHealth
    {
        get { return _maxHealth; }
    }

    public int MaxStamina
    {
        get { return _maxStamina; }
    }

    public int Health
    {
        set
        {
            if (value > _maxHealth)
            {
                value = _maxHealth;
            }

            if (value < 0)
            {
                value = 0;
            }

            _health = value;
        }

        get { return _health; }
    }

    public int Stamina
    {
        set {
            if (value > _maxStamina)
            {
                value = _maxStamina;
            }

            if (value < 0)
            {
                value = 0;
            }

            _stamina = value; 
        }

        get { return _stamina; }
    }

}
