using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class enemyHealthBar : MonoBehaviour
{
    [SerializeField] private Slider slider;

    public void updateHealthBar(float currentValue, float maxValue)
    {
        slider.value = currentValue / maxValue;
    }

    private void Start()
    {
        slider.value = 1f;
    }

    private void Update()
    {
        
    }
}
