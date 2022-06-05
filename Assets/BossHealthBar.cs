using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHealthBar : MonoBehaviour
{ 
    [SerializeField]
    private Image _image;
    [SerializeField]
    private Slider _slider;
    private int _maxHealth = 20;
    private int _currentHealth;
    [SerializeField]
    private GameObject _healthBarObj;

    // Start is called before the first frame update
    void Start()
    {
        _currentHealth = _maxHealth;
        _slider.maxValue = _maxHealth;
        _slider.value = _currentHealth;
        _healthBarObj.SetActive(false);

    }

    public void DecreaseBossHealth(int amount)
    {
        if (_currentHealth - amount >=0)
        {
            _currentHealth -= amount;
            _slider.value = _currentHealth;
        }
        else
        {
            _image.enabled = false;
        }
    }

    public void ActivateBossHealthObject()
    {
        _healthBarObj.SetActive(true);
    }
}
