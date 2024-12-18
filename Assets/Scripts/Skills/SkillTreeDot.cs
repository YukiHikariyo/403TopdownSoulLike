using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.UI.Extensions;

public class SkillTreeDot : MonoBehaviour,IPointerClickHandler,IPointerEnterHandler,IPointerExitHandler
{
    public StaticSkillData thisSkillData;
    private Image thisSkillImage;
    private Image thisSkillShadow;
    public Image selectionTip;
    public UILineRenderer lineRenderer;

    private Sequence pointerEnterSequence;
    private Sequence pointerExitSequence;
    public void OnPointerClick(PointerEventData eventData)
    {
        SkillManager.Instance.SelectSkill(thisSkillData.skillDotID, thisSkillData);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        pointerExitSequence.Pause();
        pointerEnterSequence.Restart();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        pointerEnterSequence.Pause();
        pointerExitSequence.Restart();
    }


    private void Awake()
    {
        thisSkillImage = gameObject.transform.GetChild(4).GetComponent<Image>();
        thisSkillShadow = gameObject.transform.GetChild(3).GetComponent<Image>();
        thisSkillImage.sprite = thisSkillData.skillSpite;
        thisSkillShadow.sprite = thisSkillData.skillSpite;
        thisSkillImage.type = Image.Type.Filled;
        thisSkillImage.fillMethod = Image.FillMethod.Vertical;
        thisSkillImage.fillAmount = 0;
        thisSkillShadow.color = new Color(0, 0, 0, 1);

        lineRenderer = GetComponentInChildren<UILineRenderer>();
        lineRenderer.color = new Color(0, 0, 0, 1);

        pointerEnterSequence = DOTween.Sequence();
        pointerExitSequence = DOTween.Sequence();
        pointerEnterSequence.Pause();
        pointerExitSequence.Pause();
        pointerEnterSequence.SetAutoKill(false);
        pointerExitSequence.SetAutoKill(false);
        pointerEnterSequence.Append(selectionTip.DOFade(1, 0.5f));
        pointerExitSequence.Append(selectionTip.DOFade(0, 0.5f));
    }


    private void Start()
    {
        DrawTree();
    }

    public void Initialize()
    {
        thisSkillImage = gameObject.transform.GetChild(4).GetComponent<Image>();
        thisSkillShadow = gameObject.transform.GetChild(3).GetComponent<Image>();
        thisSkillImage.sprite = thisSkillData.skillSpite;
        thisSkillShadow.sprite = thisSkillData.skillSpite;
        thisSkillImage.type = Image.Type.Filled;
        thisSkillImage.fillMethod = Image.FillMethod.Vertical;
        thisSkillImage.fillAmount = 0;
        thisSkillShadow.color = new Color(0, 0, 0, 1);

        lineRenderer = GetComponentInChildren<UILineRenderer>();
        lineRenderer.color = new Color(1, 1, 1, 0.7f);
    }

    public void DrawTree()
    {
        lineRenderer.LineList = true;
        var pointList = new List<Vector2>();

        if (thisSkillData.preIDs.Length > 0)
        {
            Transform mainTrans = SkillManager.Instance.skillDotList[thisSkillData.skillDotID];
            for (int j = 0; j < thisSkillData.preIDs.Length; j++)
            {
                var pointBegin = new Vector2(0,0);
                var pointEnd = new Vector2(SkillManager.Instance.skillDotList[thisSkillData.preIDs[j]].position.x - mainTrans.position.x, SkillManager.Instance.skillDotList[thisSkillData.preIDs[j]].position.y - mainTrans.position.y);
                pointList.Add(pointBegin);
                pointList.Add(pointEnd);
            }
        }

        if (pointList != null)
        {
            lineRenderer.Points = pointList.ToArray();
        }
    }

}
