using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : MonoBehaviour
{
    [SerializeField]
    private float _moveSpeed = 7f;

    public Vector3 _fireBallDirection = new Vector3(1,0,0);
    private void Start()
    {
        transform.rotation = Quaternion.Euler(0, 0, -90);
    }

    void Update()
    {
        transform.Translate(_fireBallDirection * _moveSpeed * Time.deltaTime);
        if (transform.position.y < -7f)
        {
            Destroy(this.gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            //Player player = GameObject.Find("Player").GetComponent<Player>();
           // player.TakeDamage();
        }
    }
}
