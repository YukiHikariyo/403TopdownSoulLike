using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SkillTreePanel : BasePanel
{
    public TextMeshProUGUI skillPointText;

    public override void OnClose()
    {

    }

    public override void OnOpen()
    {
        skillPointText.text = SkillManager.Instance.skillPoint.ToString();
    }
}
