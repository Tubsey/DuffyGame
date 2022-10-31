using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClawAttackScript : MonoBehaviour
{
    //When gameObject this script is attached to appears, this function checks if it touches the colliders of any "Target" GameObjects and destroys them if so.
    //The coding that makes this object (ClawAttakObj) appear and disappear is in PlayerMovement script
    public void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Target"))
        {
            Destroy(other.gameObject);
        }
    }
    
}
