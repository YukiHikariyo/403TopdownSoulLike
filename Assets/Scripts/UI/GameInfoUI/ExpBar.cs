using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class ExpBar : MonoBehaviour
{
    private Sequence sequence;
    public Slider slider;

    public void OnExpUp(int targetExp, int maxExp, bool isInstantChange = false)
    {
        if (!isInstantChange)
            slider.DOValue((float)targetExp / (float)maxExp, 2);
        else
            slider.value = (float)targetExp / (float)maxExp;
    }

    public void OnLevelUp(int targetExp, int maxExp, int times, int finalLevel)
    {
        List<int> levelList = new List<int>();
        int a = 0;

        sequence = DOTween.Sequence();
        sequence.SetAutoKill(false);
        sequence.Pause();

        for (int i = 0; i < times; i++)
        {
            levelList.Add(finalLevel - times + i + 1);
            sequence.Append(slider.DOValue(1, 2));
            sequence.AppendCallback(() => { slider.value = 0; UIManager.Instance.levelText.text = levelList[a++].ToString();});
        }
        sequence.Append(slider.DOValue((float)targetExp / (float)maxExp, 2));

        sequence.Play();
    }
}
