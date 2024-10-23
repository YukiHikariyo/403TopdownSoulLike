using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : InteractiveComponent
{
    [SerializeField] public Spike[] spikes;
    public override void Initialization()
    {
        base.Initialization();
        SwitchState();
    }

    public override void SwitchState()
    {
        base.SwitchState();
        foreach (Spike spike in spikes)
        {
            if(state != spike.State)
            {
                spike.State = state;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            stateMachine = collision.gameObject.GetComponent<PlayerStateMachine>();
        }
    }
}
