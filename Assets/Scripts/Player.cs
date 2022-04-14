using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem _particle;
    [SerializeField]
    private int _shieldStrength = 3;
    [SerializeField]
    private SpriteRenderer _shieldRenderer;

    private float _speedMultiplier = 2.0f;
    private float _canFire = .2f;
    private float _fireRate = 0.03f;
    private float _canFireMissile = -1f;
    private float _missileFireRate = 5f;

    private bool _isTripleShotActive;
    private bool _isSpeedBoostActive;
    private bool _isShieldActive;
    private bool _isMissileActive;
    [SerializeField]
    private int _score;
    [SerializeField]
    private int _engineOverSpeedCount;
    private int _totalLasers = 15;
    [SerializeField]
    private int _currentLasers;

    private CameraShaker _cameraShaker;
    private ThrusterBar _thrusterBar;
    private SpawnManager _spawnManager;
    private UIManager _uiManager;
    [SerializeField]
    private AudioClip _laserSoundClip;
    [SerializeField]
    private AudioClip _missileSoundClip;

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
    private GameObject _missilePrefab;

    [SerializeField]
    private GameObject _rightEngine;
    [SerializeField]
    private GameObject _leftEngine;
    private bool _isHighspeed;
    void Start()
    {
        transform.position = new Vector3(0, 0, 0);
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _audioSource = GetComponent<AudioSource>();
        _thrusterBar = GameObject.Find("ThrusterBar").GetComponent<ThrusterBar>();
        _cameraShaker = GameObject.Find("Camera_Handler").GetComponentInChildren<CameraShaker>();
        if (_spawnManager == null)
        {
            Debug.LogError("The SpawnManager is NULL:");
        }
        if (_uiManager == null)
        {
            Debug.LogError("The UIManager is NULL:");
        }
        if (_audioSource == null)
        {
            Debug.LogError("The Audio Source is NULL:");
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
        if (Input.GetKeyDown(KeyCode.Alpha9) && Time.time > _canFireMissile)
        {
            FireMissile();
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && _thrusterBar.IsThrusting())
        {
            SetShipSpeed(true);
            _thrusterBar.DecreasethrusterBar();
        }

       
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            if (_thrusterBar.IsEngineOverHeat() == true)
            {
               
                _moveSpeed = 5;
                _thrusterBar.IncreaseThrusterBar();
                Debug.Log("IncreseRoutine():Inside If");
            }
            else
            {
                _moveSpeed = 5;
                _thrusterBar.IncreaseThrusterBar();
                Debug.Log("IncreseRoutine():outside if");
            }
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
            _audioSource.clip = _laserSoundClip;
            _audioSource.Play();
            AmmoCount(1);
        }

    }

    private void FireMissile()
    {
        _canFireMissile = Time.time + _missileFireRate;
        if (_isMissileActive == true)
        {
            Instantiate(_missilePrefab, transform.position + new Vector3(-0.72f, -0.43f, 0), Quaternion.identity);
            _audioSource.clip = _missileSoundClip;
            _audioSource.Play();
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
            _cameraShaker.CameraShake(0.5f, 0.15f);
            _rightEngine.SetActive(true);
        }
        else if (_lives == 1)
        {
            _cameraShaker.CameraShake(0.5f, 0.15f);
            _leftEngine.SetActive(true);
        }
        _uiManager.UpdateLives(_lives);

        if (_lives < 1)
        {
            _cameraShaker.CameraShake(0.5f, 0.15f);
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
        _shieldRenderer.color = Color.red;
        yield return new WaitForSeconds(.5f);
        _shieldRenderer.color = Color.white;
    }

    private void AmmoCount(int amount)
    {
        _currentLasers -= amount;
        if (_uiManager != null)
        {
            _uiManager.UpdateAmmo(_currentLasers,_totalLasers);
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
            _uiManager.UpdateAmmo(_currentLasers,_totalLasers);
        }
        if (_currentLasers < 1)
        {
            _currentLasers = amount;
        }
    }

    public void HealthCollectible(int health)
    {
        if (_lives < 3)
        {
            _lives++;
            if (_lives == 3)
            {
                _rightEngine.SetActive(false);
            }
            else if (_lives == 2)
            {
                _leftEngine.SetActive(false);
            }
            if (_uiManager != null)
            {
                _uiManager.UpdateLives(_lives);
            }
        }
        Instantiate(_particle, transform.position, Quaternion.identity);

    }

    public void NegativePowerup()
    {
        TakeDamage();
    }

    public void ActivateMissile()
    {
        _isMissileActive = true;
        StartCoroutine(MissilePowerDownRoutine());
    }

    IEnumerator MissilePowerDownRoutine()
    {
        yield return new WaitForSeconds(9f);
        _isMissileActive = false;
    }

    public void SetShipSpeed(bool isHighSpeed)
    {
        this._isHighspeed = isHighSpeed;
        _moveSpeed = _isHighspeed ? _moveSpeed * 2 : _moveSpeed =5;
        if (_isHighspeed==false &&_thrusterBar.IsEngineOverHeat()==true)
        {
            _engineOverSpeedCount++;
            if (_engineOverSpeedCount==3)
            {
                _thrusterBar.ActivateThrusterCoolDown();
                _engineOverSpeedCount = 0;
            }
        }
    }
}
