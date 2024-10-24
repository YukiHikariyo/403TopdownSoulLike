using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MagicUIManager : MonoSingleton<MagicUIManager>
{
    public Sprite emptySprite;

    public Sprite[] magicSprites;

    public Image[] magicImages;

    public Image[] mask;
    
    public void UpdateUnlockState(int id ,bool state)
    {
        if (state)
        {
            magicImages[id].sprite = magicSprites[id];
        }
        else
        {
            magicImages[id].sprite = emptySprite;
        }
    }
    public void UpdateMask(int id ,float value)
    {
        mask[id].fillAmount = value;
    }
}
