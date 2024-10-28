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

    public void OnMaxValueChange(float newValue, float maxValue, bool isInstantChange = false)
    {
        if (!isInstantChange)
            maxValueSlider.DOValue(newValue / maxValue, 0.5f);
        else
            maxValueSlider.value = newValue / maxValue;
    }

    public void OnCurrentValueChange(float newValue, float maxValue, bool isInstantChange = false)
    {
        if (!isInstantChange)
            currentValueSlider.DOValue(newValue / maxValue, 0.25f);
        else
            currentValueSlider.value = newValue / maxValue;

        if (bufferSlider != null)
        {
            if (!isInstantChange)
            {
                if (bufferSlider.value < currentValueSlider.value)
                    bufferSlider.DOValue(newValue / maxValue, 0.3f);
                else
                    bufferSlider.DOValue(newValue / maxValue, 5);
            }
            else
            {
                if (bufferSlider.value < currentValueSlider.value)
                    bufferSlider.value = newValue / maxValue;
                else
                    bufferSlider.DOValue(newValue / maxValue, 5);
            }
        }
    }
}
