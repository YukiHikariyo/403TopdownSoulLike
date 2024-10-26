using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDoor : MonoBehaviour
{
    public Animator animator;
    public EnemySpawner spawner;
    [SerializeField] bool state;
    public bool State
    {
        get => state;
        set
        {
            if(spawner != null)
                state = spawner.isSpawned ^ spawner.isDead;
            else 
                state = false;
        }
    }

    private void Update()
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
