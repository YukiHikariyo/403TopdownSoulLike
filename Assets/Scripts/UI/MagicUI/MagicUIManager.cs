using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MagicUIManager : MonoSingleton<MagicUIManager>
{

    public Sprite[] magicSprites;

    public Image[] magicImages;

    public Image[] mask;
    
    public void UpdateUnlockState(int id ,bool state)
    {
        if (state)
        {
            magicImages[id].fillAmount = 1;
        }
        else
        {
            magicImages[id].fillAmount = 0;
        }
    }
    public void UpdateMask(int id ,float value)
    {
        mask[id].fillAmount = value;
    }
}
