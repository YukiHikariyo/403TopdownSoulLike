using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class PlayerBar : MonoBehaviour
{
    public Slider maxValueSlider;
    public Slider currentValueSlider;
    public Slider bufferSlider;

    public void OnMaxValueChange(float newValue, float maxValue)
    {
        maxValueSlider.DOValue(newValue / maxValue, 0.5f);
    }

    public void OnCurrentValueChange(float newValue, float maxValue)
    {
        currentValueSlider.DOValue(newValue / maxValue, 0.25f);

        if (bufferSlider.value < currentValueSlider.value)
            bufferSlider.DOValue(newValue / maxValue, 0.25f);
        else
            bufferSlider.DOValue(newValue / maxValue, 5);
    }
}
