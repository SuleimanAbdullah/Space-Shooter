using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileIcon : MonoBehaviour
{
    private SpriteRenderer _missileInsideBubble;

    private void Start()
    {
        _missileInsideBubble = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            _missileInsideBubble.enabled = false;
        }
    }
}
