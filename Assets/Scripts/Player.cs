using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private int _shieldStrength = 3;
    [SerializeField]
    private SpriteRenderer _shieldRenderer;

    private float _speedMultiplier = 2.0f;
    private float _canFire = .2f;
    private float _fireRate = 0.03f;

    private bool _isTripleShotActive;
    private bool _isSpeedBoostActive;
    private bool _isShieldActive;
    [SerializeField]
    private int _score;

    private int _totalLasers = 15;
    [SerializeField]
    private int _currentLasers;



    private SpawnManager _spawnManager;
    private UIManager _uiManager;
    private AudioSource _audioSource;

    [SerializeField]
    private WaitForSeconds _timeBeforeCoolDown = new WaitForSeconds(6f);

    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private GameObject _shieldVisualizer;

    [SerializeField]
    private float _moveSpeed = 5f;

    [SerializeField]
    private int _lives = 3;

    [SerializeField]
    private GameObject _TripleShotPrefab;

    [SerializeField]
    private GameObject _rightEngine;
    [SerializeField]
    private GameObject _leftEngine;

    void Start()
    {
        transform.position = new Vector3(0, 0, 0);
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _audioSource = GetComponent<AudioSource>();
        if (_audioSource == null)
        {
            Debug.Log("The Audio Source is NULL:");
        }

        _shieldVisualizer.SetActive(false);
        _currentLasers = _totalLasers;
    }

    void Update()
    {
        CalculateMovement();

        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire)
        {
            FireLaser();
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            _moveSpeed *= _speedMultiplier;
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            _moveSpeed /= _speedMultiplier;
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

        //check if current laser is greater than zero 
        //fireLaser then call
        //ammo count
        if (_currentLasers > 0)
        {
            if (_isTripleShotActive == true)
            {
                Instantiate(_TripleShotPrefab, transform.position, Quaternion.identity);
            }

            else
            {
                Instantiate(_laserPrefab, transform.position + new Vector3(0, 1.04f, 0), Quaternion.identity);
            }

            _audioSource.Play();
            AmmoCount(1);
        }

       

    }

    public void TakeDamage()
    {

        if (_isShieldActive == true)
        {
            _shieldStrength--;
            if (_shieldStrength == 2)
            {
                StartCoroutine(ShieldHitVisual());
                return;
            }
            else if (_shieldStrength == 1)
            {
                StartCoroutine(ShieldHitVisual());
                return;
            }
            if (_shieldStrength < 1)
            {
                _shieldStrength = 0;
                _isShieldActive = false;
                _shieldVisualizer.SetActive(false);
                return;
            }

        }
        _lives--;
        if (_lives == 2)
        {
            _rightEngine.SetActive(true);
        }
        else if (_lives == 1)
        {
            _leftEngine.SetActive(true);
        }
        _uiManager.UpdateLives(_lives);

        if (_lives < 1)
        {
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

    public void ActivateSpeedBoost()
    {
        _moveSpeed = _moveSpeed + 3f;
        _isSpeedBoostActive = true;
        StartCoroutine(SpeedbBoostPowerDownRoutine());
    }

    public void ActivateShield()
    {
        _isShieldActive = true;
        _shieldStrength = 3;
        _shieldVisualizer.SetActive(true);
    }

    public void AddingScoreWhenKillEnemyL(int amount)
    {
        _score += amount;

        if (_uiManager != null)
        {
            _uiManager.UpdateScore(_score);
        }
    }

    public IEnumerator TripleShotPowerDownRoutine()
    {
        yield return _timeBeforeCoolDown;
        _isTripleShotActive = false;
    }

    IEnumerator SpeedbBoostPowerDownRoutine()
    {
        yield return _timeBeforeCoolDown;
        _moveSpeed = 5f;
        _isSpeedBoostActive = false;
    }

    IEnumerator ShieldHitVisual()
    {
        //change color first then wait
        _shieldRenderer.color = Color.red;
        yield return new WaitForSeconds(.5f);
        _shieldRenderer.color = Color.white;
    }

    private void AmmoCount(int amount)
    {
        _currentLasers -= amount;
        if (_uiManager != null)
        {
            _uiManager.UpdateAmmo(_currentLasers);
        }
        
        if (_currentLasers < 1)
        {
            _currentLasers = 0;
        }
    }

    public void AmmoCollectible(int amount)
    {
        _currentLasers = amount;
        if (_uiManager != null)
        {
            _uiManager.UpdateAmmo(_currentLasers);
        }
        if (_currentLasers < 1)
        {
            _currentLasers = amount;
        }
    }
}
