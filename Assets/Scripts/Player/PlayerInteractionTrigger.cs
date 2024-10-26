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
            if(playerStateMachine.interactionObj.showTips)
                playerStateMachine.playerTip.enabled = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Interaction"))
        {
            InteractiveComponent outer = collision.gameObject.GetComponent<InteractiveComponent>();
            if (playerStateMachine.interactionObj == outer)
            {
                playerStateMachine.CanInterAction = false;
                playerStateMachine.interactionObj = null;
                playerStateMachine.playerTip.enabled = false;
            }
        }
    }
}
