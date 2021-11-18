using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private float _speed = 4;
    void Update()
    {
        //move down at 4 meters per speed 
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        //if buttom of screen 
        //respawn at top with new random x position

      if(transform.position.y < -7)
        {
            transform.position = new Vector3(Random.Range(-9, 9), 8, 0);
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            Debug.Log("am Hit By " + other.transform.name);
        }
    }



}
