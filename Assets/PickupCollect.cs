using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupCollect : MonoBehaviour
{

    private GameObject _powerup;
    [SerializeField]
    private bool _isButtonPressed;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            _isButtonPressed = true;
            Collect();
        }
        else
        {
            _isButtonPressed = false;
        }
    }

    void Collect()
    {
        _powerup = GameObject.FindGameObjectWithTag("Powerup");
        if (_powerup != null)
        {
            if (Vector2.Distance(_powerup.transform.position, transform.position) < 4)
            {
                _powerup.transform.position = Vector2.MoveTowards(transform.position, _powerup.transform.position, 6 * Time.deltaTime);
            }
        }
    }
}
