using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    private float _speed = 8f;

    // Update is called once per frame
    void Update()
    {
        MoveUp();

    }

    private void MoveUp()
    {
        transform.Translate(Vector3.up * _speed * Time.deltaTime);
        // if laser position is >= 8 on the y
        // destroy the object

        if (transform.position.y >= 8)
        {
            Destroy(this.gameObject);
        }
    }
}
