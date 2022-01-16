using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private float _speed = 4;
    private Player _player;

    private Animator _animator;

    private void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        _animator = GetComponent<Animator>();
    }

    void Update()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y < -7)
        {
            transform.position = new Vector3(Random.Range(-9, 9), 8, 0);
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
            
            if (_animator !=null)
            {
                _animator.SetTrigger("OnDestroyed");
            }
            Destroy(this.gameObject,2.4f);
        }
        if (other.tag == "Laser")
        {
            Destroy(other.gameObject);
            _player.AddingScoreWhenKillEnemyL(10);
            if (_animator !=null)
            {
                _animator.SetTrigger("OnDestroyed");
            }
           
            Destroy(this.gameObject,2.4f);
        }
    }
}
