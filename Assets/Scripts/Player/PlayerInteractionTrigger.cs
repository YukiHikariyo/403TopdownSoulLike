using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractionTrigger : MonoBehaviour
{
    public PlayerStateMachine playerStateMachine;

    private void Awake()
    {
        playerStateMachine = transform.parent.GetComponent<PlayerStateMachine>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Interaction"))
        {
            playerStateMachine.CanInterAction = true;
            playerStateMachine.interactionObj = collision.gameObject.GetComponent<InteractiveComponent>();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Interaction"))
        {
            playerStateMachine.CanInterAction = false;
            playerStateMachine.interactionObj = null;
        }
    }
}
