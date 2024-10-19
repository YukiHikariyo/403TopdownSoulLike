using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spike : InteractiveComponent
{
    
    public Collider2D collider;
    public override void Initialization()
    {
        base.Initialization();
        SwitchState();
    }

    protected override void SwitchState()
    {
        if (state)
        {
            collider.enabled = false;
            animator.Play("Enabled");
        }
        else
        {
            collider.enabled = true;
            animator.Play("Disabled");
        }
    }
}
