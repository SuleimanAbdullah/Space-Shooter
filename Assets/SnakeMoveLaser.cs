using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeMoveLaser : MonoBehaviour
{

    private float _speed = 1.5f;

    void Update()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);
        if (transform.position.y < -7)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
           
            Player player = transform.GetComponent<Player>();
            if (player !=null)
            {
                player.TakeDamage();
            }
            Destroy(this.gameObject);
        }
    }
}
