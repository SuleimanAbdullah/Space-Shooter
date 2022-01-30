using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private float _canFire = .2f;
    private float _fireRate = 0.03f;

    private bool _isTripleShotActive;
    private bool _isSpeedBoostActive;
    private bool _isShieldActive;
    [SerializeField]
    public  int _score;

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

        if (_isTripleShotActive == true)
        {
            Instantiate(_TripleShotPrefab, transform.position, Quaternion.identity);
        }

        else
        {
            Instantiate(_laserPrefab, transform.position + new Vector3(0, 1.04f, 0), Quaternion.identity);
        }

        _audioSource.Play();
    }

    public void TakeDamage()
    {
        if (_isShieldActive == true)
        {
            _isShieldActive = false;
            _shieldVisualizer.SetActive(false);
            return;
        }
        _lives--;
        if (_lives == 2)
        {
            _rightEngine.SetActive(true);
        }
        else if (_lives==1)
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
        _shieldVisualizer.SetActive(true);
    }

    public void AddingScoreWhenKillEnemyL(int amount)
    {
         _score += amount;

        if (_uiManager !=null)
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

}
