using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class EnemyAvoidShot : MonoBehaviour
{
    public static Action OnDeath;
    [SerializeField]
    private bool _isShotDetected;
    [SerializeField]
    private int _speed = 2;
    [SerializeField]
    private GameObject _explosionPrefab;
    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);
        if (transform.position.y < -7)
        {
            transform.position = new Vector3(UnityEngine.Random.Range(-9, 10), 8, 0);
        }

        if (_isShotDetected == true)
        {
            StartCoroutine(AvoidShot());
        }
    }
    private IEnumerator AvoidShot()
    {
        transform.Translate(Vector3.right * 3 * Time.deltaTime);
        yield return new WaitForSeconds(2f);
        _isShotDetected = false;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Laser")
        {
            _isShotDetected = true;
        }
        if (other.tag == "Missile")
        {
            if (OnDeath !=null)
            {
                OnDeath();
            }
            Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
            Destroy(this.gameObject);
        }
       
    }
}
