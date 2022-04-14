﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmartEnemyLaser : MonoBehaviour
{
    [SerializeField]
    private float _speed;
    private Player _player;

    // Update is called once per frame
    void Update()
    {
        MoveDown();
    }

    private void MoveDown()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);
        if (transform.position.y < -7)
        {
            if (transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            _player = GameObject.Find("Player").GetComponent<Player>();
            if (_player != null)
            {
                _player.TakeDamage();
            }
        }
    }
}
