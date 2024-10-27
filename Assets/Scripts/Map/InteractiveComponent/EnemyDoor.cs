using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDoor : InteractiveComponent
{
    public EnemySpawner spawner;

    private void Update()
    {
        if(State != spawner.isSpawned ^ spawner.isDead)
        {
            State = spawner.isSpawned ^ spawner.isDead;
        }
    }
}
