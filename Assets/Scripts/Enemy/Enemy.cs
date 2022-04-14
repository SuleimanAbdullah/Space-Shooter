using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3;
    private Player _player;

    [SerializeField]
    private AudioClip _explosionClip;

    private Animator _animator;
    private Collider2D _enemyCollider;

    [SerializeField]
    private GameObject _enemyLaserPrefab;

    private float _fireRate;
    private float _canFire;

    [SerializeField]
    private float _amplitude = .3f;

    public static Action onDeath; 

    private float _frequency = 2f;
    private void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        _animator = GetComponent<Animator>();

        _enemyCollider = GetComponent<Collider2D>();
    }

    void Update()
    {
        // float x =Mathf.Cos(Time.time * _frequency);
        //float y = transform.position.y;
        //float z = transform.position.z;
        //transform.position = new Vector3(x, y, z);
        //comenting this and add mathf.cos inside translator to fix enemy spawn while moving on top of others
        transform.Translate(new Vector3(Mathf.Cos(Time.time * _frequency) * _amplitude, -1,0) * _speed * Time.deltaTime);

        if (transform.position.y < -7)
        {
            transform.position = new Vector3(UnityEngine.Random.Range(-9, 10), 8, 0);
        }

        if (Time.time > _canFire)
        {
            EnemyFire();
        }


    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            onDeath();
            if (_player != null)
            {
                _player.TakeDamage();
            }

            if (_animator != null)
            {
                _animator.SetTrigger("OnDestroyed");
            }
            AudioSource.PlayClipAtPoint(_explosionClip, transform.position);
            Destroy(_enemyCollider);
            Destroy(this.gameObject, 2.4f);

        }
        if (other.tag == "Laser")
        {
            onDeath();
            Destroy(other.gameObject);
            _player.AddingScoreWhenKillEnemyL(10);
            if (_animator != null)
            {
                _animator.SetTrigger("OnDestroyed");
            }
            AudioSource.PlayClipAtPoint(_explosionClip, transform.position);
            Destroy(_enemyCollider);
            Destroy(this.gameObject, 2.4f);
        }
        if (other.tag == "Missile")
        {
            onDeath();
            _player.AddingScoreWhenKillEnemyL(10);
            if (_animator != null)
            {
                _animator.SetTrigger("OnDestroyed");
            }
            AudioSource.PlayClipAtPoint(_explosionClip, transform.position);
            Destroy(_enemyCollider);
            Destroy(this.gameObject, 2.4f);
        }
    }

    private void EnemyFire()
    {
        _fireRate = UnityEngine.Random.Range(4, 7);
        _canFire = Time.time + _fireRate;
        GameObject enemyLaser = Instantiate(_enemyLaserPrefab, transform.position, Quaternion.identity);
        Laser[] lasers = enemyLaser.GetComponentsInChildren<Laser>();
        lasers[0].ActivateEnemyLaser();
        lasers[1].ActivateEnemyLaser();
    }

}
