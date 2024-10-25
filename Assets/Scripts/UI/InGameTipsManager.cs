using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InGameTipsManager : MonoSingleton<InGameTipsManager>
{
    public GameObject tips;
    public Transform canvas;
    [SerializeField] string emptystr;


    public TextMeshProUGUI GenerateTips()
    {
        Debug.Log("!");
        GameObject b = Instantiate(tips,canvas);
        return b.GetComponent<TextMeshProUGUI>();
    }
}
