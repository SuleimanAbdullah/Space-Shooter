using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingProjectile : MonoBehaviour
{
    private GameObject[] _enemies;
    private GameObject _closestEnemy;
    private float _closestEnemyDistance;
    [SerializeField]
    private bool _isClosestEnemyFound;
    private Rigidbody2D _rb;

    private float _speed = 5f;
    private float _rotateSpeed = 200f;

    // Start is called before the first frame update
    void Start()
    {
        _closestEnemyDistance = Mathf.Infinity;
        _closestEnemy = null;
        _isClosestEnemyFound = false;
        _rb = GetComponent<Rigidbody2D>();
        if (_rb == null)
        {
            Debug.LogError("Rigidbody is NULL:");
        }

    }

    // Update is called once per frame
    void Update()
    {
        GetClosestEnemy();
    }

     void FixedUpdate()
    {
        MoveToClosestEnemy();
    }

     void GetClosestEnemy()
    {
        _enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (var enemy in _enemies)
        {
            float currentDistance = Vector2.Distance(transform.position, enemy.transform.position);
            if (currentDistance < _closestEnemyDistance)
            {
                _closestEnemyDistance = currentDistance;
                _closestEnemy = enemy;
                _isClosestEnemyFound = true;
            }
        }
    }

     void MoveToClosestEnemy()
    {
        if (_closestEnemy != null   )
        {
            if (_isClosestEnemyFound == true)
            {
                Vector2 direction = (Vector2)_closestEnemy.transform.position - (Vector2)transform.position;
                direction.Normalize();
                float crossValue = Vector3.Cross(direction, transform.up).z;
                if (_rb != null)
                {
                    _rb.angularVelocity = crossValue * -_rotateSpeed;
                    _rb.velocity = transform.up * _speed;
                }
            }
          
        }
        else
        {
            _rb.velocity = transform.up * _speed;
            if (_rb.position.y > 15f)
            {
                Destroy(this.gameObject);
            }
            _isClosestEnemyFound = false;
        }
    }

     void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Enemy")
        {
            Destroy(gameObject);
        }
    }
}
