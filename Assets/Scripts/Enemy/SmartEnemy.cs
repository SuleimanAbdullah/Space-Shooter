using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmartEnemy : MonoBehaviour
{
    [SerializeField]
    private GameObject _enemyLasersPrefab;
    [SerializeField]
    private float _fireRate;
    private float _canFire;
    [SerializeField]
    private GameObject _explotion;

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
    [SerializeField]
    private float _moveSpeed = 3;

    private Transform _target;
    [SerializeField]
    private float _aggroRange = 5;
    [SerializeField]
    private GameObject _smartEnemyLaserPrefab;
    [SerializeField]
    private bool _isTargetBehind;

    private float _dot;

    [SerializeField]
    private GameObject _laserStartPoint;
    [SerializeField]
    private LayerMask _layerMask;
    [SerializeField]
    private LineRenderer _lineRenderer;


    Vector2 _lookDirection;
    Vector2 _originPos;
    Vector2 _targetPoint;


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
        //transform.rotation = Quaternion.Euler(0, 0, 180);
    }

    void Update()
    {
        //transform.Translate(new Vector3(Mathf.Cos(Time.time * 1), Mathf.Sin(Time.time * 1), 0) * 1 * Time.deltaTime);
        transform.Translate(Vector3.up * _moveSpeed * Time.deltaTime);

        if (transform.position.y < -7)
        {
            transform.position = new Vector3(UnityEngine.Random.Range(-9, 10), 8, 0);
        }

        if (_isTargetBehind == false)
        {
            if (Time.time > _canFire)
            {
                SmartEnemyFireLaser();
            }
        }

        if (_dot < -0.986f)
        {
            _isTargetBehind = true;

            if (Time.time > _canFire)
            {
                FireBackLaser();
            }
        }
        else
        {
            _isTargetBehind = false;
        }

        //if (Time.time > _canFireMissile)
        //  SmartEnemyFireMissile();
        EnemyAggro();
        SmartEnemyDetectPlayerFromBack();
        StartCoroutine(ShootPowerup());
    }


    void SmartEnemyFireLaser()
    {
        _fireRate = UnityEngine.Random.Range(2f, 2);
        _canFire = Time.time + _fireRate;
        Instantiate(_enemyLasersPrefab, transform.position, Quaternion.identity);
    }

    private void FireBackLaser()
    {
        _fireRate = 1.5f;
        _canFire = Time.time + _fireRate;
        GameObject SmartEnemyLaser = Instantiate(_smartEnemyLaserPrefab, transform.position + new Vector3(-0.017f, 2.19f, 0), Quaternion.identity);

        SmartEnemyLaser laser = SmartEnemyLaser.GetComponent<SmartEnemyLaser>();
        laser.ActivatBackGun();
    }



    void SmartEnemyFireMissile()
    {
        _missileFireRate = UnityEngine.Random.Range(30, 60);
        _canFireMissile = Time.time + _missileFireRate;
        Instantiate(_enemyMissilePrefab, transform.position + new Vector3(0.607f, -0.157f, 0), Quaternion.identity);
    }

    private IEnumerator ShootPowerup()
    {
        _originPos = _laserStartPoint.transform.position;
        _lookDirection = _laserStartPoint.transform.up;

        RaycastHit2D hitInfo = Physics2D.Raycast(_originPos, _lookDirection, Mathf.Infinity, _layerMask);
        Debug.DrawRay(_originPos, _lookDirection, Color.red);
        _targetPoint = hitInfo.point;

        if (hitInfo.collider != null)
        {
            _lineRenderer.SetPosition(0, _originPos);
            _lineRenderer.SetPosition(1, hitInfo.point);
            _lineRenderer.enabled = true;

            yield return new WaitForSeconds(.089f);

            _lineRenderer.enabled = false;
            if (hitInfo.transform !=null)
            {
                Destroy(hitInfo.transform.gameObject);
            }
            
        }

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
        if (_target != null)
        {
            Vector3 direction = _target.position - transform.position;
            Debug.DrawRay(transform.position, direction, Color.red);
            if (Vector2.Distance(transform.position, _target.position) <= _aggroRange)
            {
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + 90;
                Quaternion angleAxis = Quaternion.AngleAxis(angle, Vector3.forward);
                transform.rotation = Quaternion.Slerp(transform.rotation, angleAxis, Time.deltaTime * 40);
            }
            else
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 0, 180), Time.deltaTime * 80);
            }
        }

    }

    private void SmartEnemyDetectPlayerFromBack()
    {
        if (_target != null)
        {
            Vector3 directioToTarget = _target.position - transform.position;
            directioToTarget.Normalize();
            _dot = Vector3.Dot(transform.up, directioToTarget);
            //Debug.Log("DotProduct: " + _dot);
        }
    }
}
