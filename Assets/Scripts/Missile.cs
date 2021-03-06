using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Missile : MonoBehaviour
{
    [SerializeField]
    private GameObject[] _targets;

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
        _targets = GameObject.FindGameObjectsWithTag("Enemy");

        _rb = GetComponent<Rigidbody2D>();
    }


    void FixedUpdate()
    {
        CalculateTarget();
    }


    private void CalculateTarget()
    {
        foreach (GameObject target in _targets)
        {
            if (target != null)
            {
                Vector2 direction = (Vector2)target.transform.position - (Vector2)transform.position;
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
                if (_rb.position.y > 15f)
                {
                    Destroy(this.gameObject);
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Enemy")
        {
            Destroy(this.gameObject);
            Instantiate(_explosion, transform.position, Quaternion.identity);
        }
    }
}