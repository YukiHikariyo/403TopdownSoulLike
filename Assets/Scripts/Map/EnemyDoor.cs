using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDoor : InteractiveComponent
{
    public EnemySpawner spawner;
    private bool doorState = false;

    public override void Initialization()
    {
        animator.Play("Disabled");
    }
    private void Update()
    {
        if (!state)
        {
            if(doorState != spawner.isDead ^ spawner.isSpawned)
            {
                doorState = spawner.isDead ^ spawner.isSpawned;
                if (doorState)
                {
                    animator.Play("Enabled");
                }
                else
                {
                    animator.Play("Disabled");
                    state = true;
                }
            }
        }
    }
}
