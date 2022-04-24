using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmartEnemyMissile : MonoBehaviour
{
    private GameObject _target;

    private Rigidbody2D _rb;

    [SerializeField]
    private float _speed = 4;
    [SerializeField]
    private float _rotatingSpeed = 200;
    [SerializeField]
    private GameObject _explosion;


    // Use this for initialization
    void Start()
    {
        _target = GameObject.FindGameObjectWithTag("Player");
        _rb = GetComponent<Rigidbody2D>();
    }


    void FixedUpdate()
    {
        CalculateTarget();
    }

    private void CalculateTarget()
    {
        if (_target != null)
        {
            Vector2 direction = (Vector2)_target.transform.position - (Vector2)transform.position;
            direction.Normalize();
            float crossValue = Vector3.Cross(direction, transform.up).z;

            if (_rb != null)
            {
                _rb.angularVelocity = _rotatingSpeed * -crossValue;

                _rb.velocity = transform.up * _speed;
            }
        }
        else
        {
            _rb.velocity = transform.up * _speed;
            if (_rb.position.y < -15f)
            {
                Destroy(this.gameObject);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Player player = GameObject.Find("Player").GetComponent<Player>();
            if (player !=null)
            {
                player.TakeDamage();
            }
            if (transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }
            Destroy(this.gameObject);
            Instantiate(_explosion, transform.position, Quaternion.identity);

        }
    }
}
