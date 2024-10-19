using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spike : MonoBehaviour
{
    private bool state;
    public bool State
    {
        get { return state; }
        set
        {
            state = value;
            SwitchState();
        }
    }
    public Animator animator;
    

    public void SwitchState()
    {
        if (state)
        {
            animator.Play("Enabled");
        }
        else
        {
            animator.Play("Disabled");
        }
    }
}
