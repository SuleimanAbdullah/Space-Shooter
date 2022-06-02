using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShaker : MonoBehaviour
{
    private float _shakeTime;

    private float _shakePower;
    private float _shakeFadeTime;

    private float _shakeRotation;
    [SerializeField]
    private float _rotationMultiplier = 2f;

    private void LateUpdate()
    {
        if (_shakeTime > 0)
        {
            _shakeTime -= Time.deltaTime;

            float x = Random.Range(-1, 1) * _shakePower;
            float y = Random.Range(-1, 1) * _shakePower;

            transform.position = new Vector3(x, y, transform.position.z);
            transform.rotation = Quaternion.Euler(0, 0, Random.Range(-1, 1) * _shakeRotation);

            _shakePower = Mathf.MoveTowards(_shakePower, 0, _shakeFadeTime * Time.deltaTime);
            _shakeRotation = Mathf.MoveTowards(_shakeRotation, 0, (_shakeFadeTime * _rotationMultiplier) * Time.deltaTime);
        }
    }

    public void CameraShake(float duration, float magnitude)
    {
        _shakeTime = duration;
        _shakePower = magnitude;

        _shakeFadeTime = magnitude / duration;
        _shakeRotation = _shakePower * _rotationMultiplier;
    }
}
