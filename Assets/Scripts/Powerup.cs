using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3f;
    private Player _player;

    [SerializeField]
    private int _powerupID;

    private SpriteRenderer _powerupsprite;
    
    private AudioSource _audioSource;

    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        _audioSource = GetComponent<AudioSource>();
        _powerupsprite = GetComponent<SpriteRenderer>();
        if (_player == null)
        {
            Debug.LogError("Player is NULL:");
        }
    }

    void Update()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);
        if (transform.position.y < -5)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            if (_player != null)
            {
                switch (_powerupID)

                {
                    case 0:
                        _player.ActivateTripleShot();
                        break;
                    case 1:
                        _player.ActivateSpeedBoost();
                        break;
                    case 2:
                        _player.ActivateShield();
                        break;
                    case 3:
                        _player.AmmoCollectible(15);
                        break;
                    case 4:
                        _player.ActivateMissile();
                        break;
                    case 5:
                        _player.NegativePowerup();
                        break;
                }
            }
            _powerupsprite.enabled = false;
            _audioSource.Play();

            Destroy(this.gameObject, 3f);
        }
    }

}
