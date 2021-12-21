using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private float _canFire = .2f;
    private float _fireRate = 0.03f;
    private SpawnManager _spawnManager;

    private WaitForSeconds _timeBeforeCoolDown = new WaitForSeconds(5f);

    [SerializeField]
    private GameObject _laserPrefab;

    [SerializeField]
    private float _moveSpeed = 5f;

    [SerializeField]
    private int _lives = 3;
    
    [SerializeField]
    private bool _isTripleShotActive;

    [SerializeField]
    private GameObject _TripleShotPrefab;

    void Start()
    {
        transform.position = new Vector3(0, 0, 0);
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
    }

    void Update()
    {
        CalculateMovement();

        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire)
        {
            FireLaser();
        }
    }

    void CalculateMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        transform.Translate(Vector3.right * horizontalInput * _moveSpeed * Time.deltaTime);
        transform.Translate(Vector3.up * verticalInput * _moveSpeed * Time.deltaTime);

        if (transform.position.y >= 0)
        {
            transform.position = new Vector3(transform.position.x, 0, 0);
        }
        else if (transform.position.y <= -3.9f)
        {
            transform.position = new Vector3(transform.position.x, -3.9f, 0);
        }

        if (transform.position.x > 11.5f)
        {
            transform.position = new Vector3(-11.5f, transform.position.y, 0);
        }

        else if (transform.position.x < -11.5f)
        {
            transform.position = new Vector3(11.5f, transform.position.y, 0);
        }
    }

    private void FireLaser()
    {
        _canFire = Time.time + _fireRate;

        if(_isTripleShotActive== true)
        {
            //fire Triple shot
            Instantiate(_TripleShotPrefab, transform.position, Quaternion.identity);
            
        }

        else
        {
            Instantiate(_laserPrefab, transform.position + new Vector3(0, 0.8f, 0), Quaternion.identity);
        }
        
    }

    public void TakeDamage()
    {
        _lives--;
       
        if (_lives < 1)
        {
            //communicate 
            //with SpawnManager
            if (_spawnManager != null)
            {
                _spawnManager.OnPlayerDeath();
            }
            Destroy(this.gameObject);
        }
    }

    public void ActivateTripleShot()
    {
        _isTripleShotActive = true;
        StartCoroutine(TripleShotPowerDownRoutine());
        
    }

    public IEnumerator TripleShotPowerDownRoutine()
    {
        yield return _timeBeforeCoolDown;
        _isTripleShotActive = false;
    }
}
