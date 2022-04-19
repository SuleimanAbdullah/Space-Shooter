using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmartEnemy : MonoBehaviour
{
    [SerializeField]
    private GameObject _enemyLasersPrefab;
    private float _fireRate;
    private float _canFire;

    [SerializeField]
    private GameObject _enemyMissilePrefab;
    private float _missileFireRate;
    private float _canFireMissile;

    [SerializeField]
    private AudioClip _explosionClip;

    private Animator _animator;
    private Collider2D _enemyCollider;

    private Player _player;
    public static Action onDeath;

    private bool _isShieldActive = true;

    [SerializeField]
    private GameObject _enemyShield;
    private float _moveSpeed = 3;

    private Transform _target;
    [SerializeField]
    private float _aggroRange = 5;


    private void Start()
    {
        _animator = GetComponent<Animator>();
        _enemyCollider = GetComponent<Collider2D>();
        if (_animator == null)
        {
            Debug.LogError("Animator is NULL:");
        }
        if (_enemyCollider == null)
        {
            Debug.LogError("EnemyCollider is NULL:");
        }
        _target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        //transform.Translate(new Vector3(Mathf.Cos(Time.time * 1), Mathf.Sin(Time.time * 1), 0) * 1 * Time.deltaTime);
        transform.Translate(Vector3.down * _moveSpeed * Time.deltaTime);

        if (transform.position.y < -7)
        {
            transform.position = new Vector3(UnityEngine.Random.Range(-9, 10), 8, 0);
        }
        if (Time.time > _canFire)
            SmartEnemyFireLaser();

        if (Time.time > _canFireMissile)
            SmartEnemyFireMissile();
        EnemyAggro();}


    void SmartEnemyFireLaser()
    {
        _fireRate = UnityEngine.Random.Range(4, 7);
        _canFire = Time.time + _fireRate;
        Instantiate(_enemyLasersPrefab, transform.position, Quaternion.identity);
    }

    void SmartEnemyFireMissile()
    {
        _missileFireRate = UnityEngine.Random.Range(30, 60);
        _canFireMissile = Time.time + _missileFireRate;
        Instantiate(_enemyMissilePrefab, transform.position + new Vector3(0.607f, -0.157f, 0), Quaternion.identity);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.tag == "Player")
        {
            if (_isShieldActive == true)
            {
                _isShieldActive = false;
                _enemyShield.SetActive(false);
                return;
            }
            if (onDeath != null)
            {
                onDeath();
            }
            _player = GameObject.Find("Player").GetComponent<Player>();
            if (_player != null)
            {
                _player.TakeDamage();
            }

            if (_animator != null)
            {
                _animator.SetTrigger("OnBoom");
            }
            AudioSource.PlayClipAtPoint(_explosionClip, transform.position);
            Destroy(_enemyCollider);
            Destroy(this.gameObject, 1.58f);

        }

        if (other.tag == "Laser")
        {
            if (_isShieldActive == true)
            {
                _isShieldActive = false;
                _enemyShield.SetActive(false);
                return;
            }
            if (onDeath != null)
            {
                onDeath();
            }
            if (_animator != null)
            {
                _animator.SetTrigger("OnBoom");
            }
            AudioSource.PlayClipAtPoint(_explosionClip, transform.position);
            Destroy(_enemyCollider);
            Destroy(this.gameObject, 1.56f);
        }
        if (other.tag == "Missile")
        {
            if (_isShieldActive == true)
            {
                _isShieldActive = false;
                _enemyShield.SetActive(false);
                return;
            }
            if (onDeath != null)
            {
                onDeath();
            }
            if (_animator != null)
            {
                _animator.SetTrigger("OnBoom");
            }
            AudioSource.PlayClipAtPoint(_explosionClip, transform.position);
            Destroy(_enemyCollider);
            Destroy(this.gameObject, 1.56f);
        }
    }

    void EnemyAggro()
    {
        Vector3 direction = _target.position - transform.position;
        Debug.DrawRay(transform.position, direction, Color.red);
        if (Vector2.Distance(transform.position, _target.position) <= _aggroRange)
        {
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + 90;
            Debug.Log("Angle:" + angle);
            Quaternion angleAxis = Quaternion.AngleAxis(angle, Vector3.forward);
            transform.rotation = Quaternion.Slerp(transform.rotation, angleAxis, Time.deltaTime * 40);
        }
        else
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.identity , Time.deltaTime * 80);
        }

    }
}
