using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float _moveSpeed = 5f;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(0, 0, 0);
    }

    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        transform.Translate(Vector3.right * horizontalInput * _moveSpeed * Time.deltaTime);
        transform.Translate(Vector3.up * verticalInput * _moveSpeed * Time.deltaTime);

        //if player position on the y is > than 0 reset y pos = 0
        //else if position on the y is < than -3.9f y pos = -3.9f


        if(transform.position.y >=0)
        {
            transform.position = new Vector3(transform.position.x, 0, 0);
        }
        else if(transform.position.y <= -3.9f)
        {
            transform.position = new Vector3(transform.position.x,-3.9f, 0);
        }

        // if player on the x > 11
        // x pos = -11
        // else if player < -11
        // x pos = 11
        if(transform.position.x > 11)
        {
            transform.position = new Vector3(-11, transform.position.y, 0);
        }

        else if (transform.position.x < -11)
        {
            transform.position = new Vector3(11, transform.position.y, 0);
        }
        

    }





}
