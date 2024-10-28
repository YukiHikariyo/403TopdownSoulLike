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

    private Tweener bufferTween;

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
            currentValueSlider.DOValue(newValue / maxValue, 0.2f);
        else
            currentValueSlider.value = newValue / maxValue;

        if (bufferSlider != null)
        {
            if (!isInstantChange)
            {
                bufferTween.Kill();

                if (bufferSlider.value < newValue / maxValue)
                    bufferTween = bufferSlider.DOValue(newValue / maxValue, 0.2f);
                else
                    bufferTween = bufferSlider.DOValue(newValue / maxValue, 5);

                bufferTween.Play();
            }
            else
            {
                bufferTween.Kill();

                if (bufferSlider.value < newValue / maxValue)
                    bufferSlider.value = newValue / maxValue;
                else
                    bufferTween = bufferSlider.DOValue(newValue / maxValue, 5);

                bufferTween.Play();
            }
        }
    }
}
