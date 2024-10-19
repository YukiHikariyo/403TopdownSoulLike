using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : InteractiveComponent
{
    public override void Initialization()
    {
        base.Initialization();
    }

    protected override void SwitchState()
    {
        base.SwitchState();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {

        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            
        }
    }
}
