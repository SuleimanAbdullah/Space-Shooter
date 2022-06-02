using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ThrusterBar : MonoBehaviour
{
    [SerializeField]
    private Slider _thrusterBarSlider;
   
   
    private Player _player;

    private int _currentValue;
    private int _maxValue = 100;

    private int _maxValueCoolDown = 100;
    private int _currentValueCoolDown;

    private Coroutine _increaseRoutine;
    private Coroutine _decreaseRoutine;

    [SerializeField]
    private GameObject _thrusterCoolDownObj;
    [SerializeField] 
    private Slider _thrusterBarCoolDownSlider;
    [SerializeField]
    private Text _coolDownText;
    private Coroutine _activateCoolDownRoutine;
    private void Start()
    {
        _currentValue = _maxValue;
        _thrusterBarSlider.maxValue = _maxValue;
        _thrusterBarSlider.value = _currentValue;

        _currentValueCoolDown = 0;
        _thrusterBarCoolDownSlider.maxValue = _maxValueCoolDown;
        _thrusterBarCoolDownSlider.value = _currentValueCoolDown;
        _thrusterCoolDownObj.SetActive(false);
        _coolDownText.text = "";
        _player = GameObject.Find("Player").GetComponent<Player>();
        if (_player == null)
        {
            Debug.LogError("Player is Null:");
        }
       
    }
    public void DecreasethrusterBar()
    {
        if (_increaseRoutine != null)
        {
            StopCoroutine(_increaseRoutine);
        }
        if (_activateCoolDownRoutine != null)
        {
            StopCoroutine(_decreaseRoutine);
        }
        _decreaseRoutine = StartCoroutine(DecreaseThrusterBarRoutine(1));
    }
    public void IncreaseThrusterBar()
    {
        if (_decreaseRoutine != null)
        {
            StopCoroutine(_decreaseRoutine);
        }
        if (_activateCoolDownRoutine != null)
        {
            StopCoroutine(_increaseRoutine);
            StopCoroutine(_decreaseRoutine);
        }

        _increaseRoutine = StartCoroutine(IncreaseThrusterBarRoutine(1));


    }
    public IEnumerator DecreaseThrusterBarRoutine(int amount)
    {
        while (_currentValue - amount >= 0)
        {
            _currentValue -= amount;
            _thrusterBarSlider.value = _currentValue;
            if (_currentValue - amount < 1)
            {
                if (_player != null)
                {
                    _player.SetShipSpeed(false);
                }
            }
            yield return new WaitForSeconds(0.01f);
        }

        _increaseRoutine = null;
    }

    private IEnumerator IncreaseThrusterBarRoutine(int amount)
    {
        yield return new WaitForSeconds(4);

        while (_currentValue < _maxValue)
        {
            _currentValue += _maxValue / 100;
            _thrusterBarSlider.value = _currentValue;
            yield return new WaitForSeconds(0.1f);
        }
        _decreaseRoutine = null;
    }

    public bool IsThrusting()
    {
        return _thrusterBarSlider.value > 0;
    }

    public bool IsEngineOverHeat()
    {
        return _thrusterBarSlider.value < 1;
    }


    public void ActivateThrusterCoolDown()
    {
        _currentValueCoolDown = 0;
        _thrusterBarCoolDownSlider.maxValue = _maxValueCoolDown;
        _thrusterBarCoolDownSlider.value = _currentValueCoolDown;

        if (_activateCoolDownRoutine != null)
        {
            StopCoroutine(_increaseRoutine);
            StopCoroutine(_decreaseRoutine);
        }

        _activateCoolDownRoutine = StartCoroutine(ThrusterBarCoolDownRoutine());
    }

    private IEnumerator ThrusterBarCoolDownRoutine()
    {
        _thrusterCoolDownObj.SetActive(true);
        _coolDownText.gameObject.SetActive(true);
        _coolDownText.text = "Engine Cool Down";

        yield return new WaitForSeconds(2f);
        while (_currentValueCoolDown < _maxValueCoolDown)
        {
            _currentValueCoolDown += _maxValueCoolDown / 100;
            _thrusterBarCoolDownSlider.value = _currentValueCoolDown;
            yield return new WaitForSeconds(0.1f);
        }
        yield return new WaitForSeconds(2f);
        _thrusterCoolDownObj.SetActive(false);
        _coolDownText.text = "";
        _coolDownText.gameObject.SetActive(false);
        _thrusterBarCoolDownSlider.value = 0;
        _activateCoolDownRoutine = null;
       _increaseRoutine = StartCoroutine(IncreaseThrusterBarRoutine(1));
    }
}


