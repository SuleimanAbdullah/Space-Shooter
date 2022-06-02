using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    [SerializeField]
    private int _health =20;
    [SerializeField]
    private int _currentHealth;
    [SerializeField] GameObject[] _explosionPrefabs;
    void Start()
    {
        _currentHealth = _health;
        _guns = transform.GetComponentsInChildren<Gun>();
        _cameraShaker = GameObject.Find("Camera_Handler").GetComponentInChildren<CameraShaker>();
    }

    void Update()
    {
         transform.Translate(Vector3.down * _speed * Time.deltaTime);
        if (transform.position.y <= 4.27f)
        {
            float x = (Mathf.Cos(Time.time) * _amplititude);
            float y = 4.27f;
            float z = transform.position.z;
            transform.position = new Vector3(x, y, z);
        }

        if (Time.time > _canFire && transform.position.y <= 4.27f)
        {
            ActivateBossFireBall();
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
        if (_currentHealth < 1)
        {
            _currentHealth = 0;
            foreach (var explosion in _explosionPrefabs)
            {
                Instantiate(explosion, transform.position, Quaternion.identity);
                _cameraShaker.CameraShake(0.5f, 0.15f);
            }
           
            Destroy(this.gameObject,2f);
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
        }
        if (other.tag == "Missile")
        {
            Damage(2);
        }
    }
}
