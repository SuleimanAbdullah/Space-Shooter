using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Boss : MonoBehaviour
{
    private CameraShaker _cameraShaker;
    [SerializeField]
    private float _speed = 2;
    private Gun[] _guns;

    private float _firRate = .5f;
    private float _canFire = 0;

    [SerializeField]
    private int _amplititude = 1;
    [SerializeField]
    private float _frequence = 1;

    [SerializeField]
    private GameObject _waveLasersPrefab;

    public int _maxHealth = 20;

    public int _currentHealth;
    [SerializeField] GameObject _explosionPrefab;

    private BossHealthBar _bossHealthBar;

    void Start()
    {
        _currentHealth = _maxHealth;
        _guns = transform.GetComponentsInChildren<Gun>();
        _cameraShaker = GameObject.Find("Camera_Handler").GetComponentInChildren<CameraShaker>();
        _bossHealthBar = GameObject.Find("Boss_Canvas").GetComponent<BossHealthBar>();
        if (_bossHealthBar == null)
        {
            Debug.Log("BossHealthBar is NULL:");
        }
        if (_cameraShaker == null)
        {
            Debug.LogError("CamerShaker is NULL:");
        }
    }

    void Update()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);
        if (transform.position.y <= 3.7f)
        {
            float x = (Mathf.Cos(Time.time) * _amplititude);
            float y = 3.7f;
            float z = transform.position.z;
            transform.position = new Vector3(x, y, z);
        }

        if (Time.time > _canFire && transform.position.y <= 3.7f)
        {
            ActivateBossFireBall();
            _bossHealthBar.ActivateBossHealthObject();
            if (_currentHealth <= 10)
            {
                ShootWavelasers();
            }
        }
    }

    private void ActivateBossFireBall()
    {
        _canFire = Time.time + _firRate;
        foreach (var gun in _guns)
        {
            if (gun != null)
            {
                gun.Shoot();
            }
        }
    }

    private void Damage(int amount)
    {
        _currentHealth -= amount;
        _bossHealthBar.DecreaseBossHealth(amount);
        if (_currentHealth < 1)
        {
            _currentHealth = 0;
            Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
            _cameraShaker.CameraShake(0.5f, 0.15f);

            Destroy(this.gameObject, 2f);
        }
    }

    private void ShootWavelasers()
    {
        Instantiate(_waveLasersPrefab, transform.position, Quaternion.identity);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Laser")
        {
            Damage(1);
            _cameraShaker.CameraShake(0.5f, 0.15f);
        }
        if (other.tag == "Missile")
        {
            Damage(2);
            _cameraShaker.CameraShake(0.6f, 0.16f);
        }
    }
}
