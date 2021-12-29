﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3f;
    private Player _player;

    [SerializeField]
    private int _powerupID;

    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
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
            if(_player != null)
            {
                if (_powerupID == 0)
                {
                    _player.ActivateTripleShot();
                }
                else if (_powerupID == 1)
                {
                    _player.ActivateSpeedBoost();
                }
                
            }
            Destroy(this.gameObject);
        }
    }

}
