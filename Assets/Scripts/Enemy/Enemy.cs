using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private float _speed = 4;
    private Player _player;

    [SerializeField]
    private AudioClip _explosionClip;

    private Animator _animator;
    private Collider2D _enemyCollider;
    [SerializeField]
    private GameObject _enemyLaserPrefab;

    private float _fireRate;
    private float _canFire;
    private void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        _animator = GetComponent<Animator>();

        _enemyCollider = GetComponent<Collider2D>();
    }

    void Update()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y < -7)
        {
            transform.position = new Vector3(Random.Range(-9, 9), 8, 0);
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
    }

    private void EnemyFire()
    {
        _fireRate = Random.Range(3, 6);
        _canFire = Time.time + _fireRate;
        GameObject enemyLaser = Instantiate(_enemyLaserPrefab, transform.position, Quaternion.identity);
        Laser[] lasers = enemyLaser.GetComponentsInChildren<Laser>();
        lasers[0].ActivateEnemyLaser();
        lasers[1].ActivateEnemyLaser();
    }


}
