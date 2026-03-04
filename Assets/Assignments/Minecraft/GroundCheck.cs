using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GroundCheck : MonoBehaviour
{
    public rigidbodyController playerController;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == playerController.gameObject) return;

        playerController.SetGrounded(true);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == playerController.gameObject) return;

        playerController.SetGrounded(false);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject == playerController.gameObject) return;

        playerController.SetGrounded(true);
    }
}


