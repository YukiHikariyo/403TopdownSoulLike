using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class ExpBar : MonoBehaviour
{
    private Sequence sequence;
    private Slider slider;

    private void Awake()
    {
        slider = GetComponent<Slider>();
    }

    public void OnExpUp(int targetExp, int maxExp)
    {
        slider.DOValue(targetExp / maxExp, 2);
    }

    public void OnLevelUp(int targetExp, int maxExp, int times, int finalLevel)
    {
        sequence = DOTween.Sequence();
        sequence.SetAutoKill(false);
        sequence.Pause();

        for (int i = 0; i < times; i++)
        {
            sequence.Append(slider.DOValue(1, 2));
            sequence.AppendCallback(() => { UIManager.Instance.levelText.text = (finalLevel - times - i + 1).ToString(); });
        }
        sequence.Append(slider.DOValue(targetExp / maxExp, 2));

        sequence.Play();
    }
}
