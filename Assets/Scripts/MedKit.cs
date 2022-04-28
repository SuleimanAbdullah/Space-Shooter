using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MedKit : MonoBehaviour
{
    private Player _player;


    [SerializeField]
    private float _moveSpeed = 3f;

    private void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
    }

    private void Update()
    {
        transform.Translate(Vector3.down * _moveSpeed * Time.deltaTime);
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
                _player.HealthCollectible(1);
            }
            Destroy(this.gameObject);
        }
    }
}
