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

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _enemyCollider = GetComponent<Collider2D>();
        if (_animator == null)
        {
            Debug.LogError("Animator is NULL:");
        }
        if (_enemyCollider ==null)
        {
            Debug.LogError("EnemyCollider is NULL:");
        }
    }

    void Update()
    {
        transform.Translate(new Vector3(Mathf.Cos(Time.time * 2), Mathf.Sin(Time.time * 2), 0) * 3 * Time.deltaTime);
        transform.Translate(Vector3.down * Time.deltaTime);

        if (transform.position.y < -7)
        {
            transform.position = new Vector3(UnityEngine.Random.Range(-9, 10), 8, 0);
        }
        if (Time.time > _canFire)
            SmartEnemyFireLaser();

        if (Time.time > _canFireMissile)
            SmartEnemyFireMissile();
    }


    void SmartEnemyFireLaser()
    {
        _fireRate = UnityEngine.Random.Range(4, 7);
        _canFire = Time.time + _fireRate;
        Instantiate(_enemyLasersPrefab, transform.position, Quaternion.identity);
    }

    void SmartEnemyFireMissile()
    {
        _missileFireRate = UnityEngine.Random.Range(5, 8);
        _canFireMissile = Time.time + _missileFireRate;
        Instantiate(_enemyMissilePrefab, transform.position + new Vector3(0.607f, -0.157f, 0),Quaternion.identity);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        
        if (other.tag == "Player")
        {
            if (onDeath !=null)
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
    }
}
