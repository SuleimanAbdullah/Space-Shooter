using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{

    private void Update()
    {
        Die();
    }
    private void Die()
    {
        Destroy(this.gameObject, 7f);
    }

}
