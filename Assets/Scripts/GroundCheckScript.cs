using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheckScript : MonoBehaviour
{
    //important bool for resetting jumpCounter integer from the PlayerMovement script when the player lands.
    public static bool isGrounded = true;

    //If GameObject is in contact with "Ground" GameObject, sets "isGrounded" bool to true and resets jumpCount. This change affects PlayerMovement script.
    public void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            PlayerMovement.jumpCount = 0;
        }

    }

    //if GameObject is in contact with "Ground" GameObject, sets the "isGrounded" bool to false. This change affects PlayerMovement script.
    public void OnTriggerExit(Collider other)
    {
        if(other.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }
    
    
}
