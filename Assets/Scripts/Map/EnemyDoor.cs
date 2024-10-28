using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDoor : InteractiveComponent
{
    public EnemySpawner spawner;

    private void Update()
    {
        if(UpperState && State)
        {
            State = false;
        }

        if (!UpperState)
        {
            if(State != spawner.isSpawned ^ spawner.isDead)
            {
                State = spawner.isSpawned ^ spawner.isDead;
            }

            if(spawner.isSpawned && spawner.isDead)
                UpperState = true;
        }
    }
}
