using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField]
    private GameObject _fireBall;

    private Vector3 _direction;

    // Update is called once per frame
    void Update()
    {
        _direction = (transform.localRotation * transform.right).normalized;
    }

    public void Shoot()
    {
        GameObject newFireBallObj = Instantiate(_fireBall, transform.position, Quaternion.identity);
        FireBall fireBall = newFireBallObj.GetComponent<FireBall>();
        fireBall._fireBallDirection = _direction;
    }

}
