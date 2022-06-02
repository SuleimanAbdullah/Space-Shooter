using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeMotion : MonoBehaviour
{
    float _xInitialPosition;
    private void Start()
    {
        _xInitialPosition = transform.position.x;
    }
    void Update()
    {
        MoveLikeSnake();
    }

    void MoveLikeSnake()
    {
        Vector2 pos = transform.position;
        float sin = Mathf.Cos(pos.y);
        pos.x = _xInitialPosition+sin;
        transform.position = pos;
    }
}
