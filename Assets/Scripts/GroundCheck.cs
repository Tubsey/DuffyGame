using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(BoxCollider))]
public class GroundCheck : MonoBehaviour
{
    [SerializeField]
    string layerToCheck = "Ground";

    HashSet<Collider> collidingObjects = new HashSet<Collider>();

    public bool IsGrounded { get; set; }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer(layerToCheck))
        {
            collidingObjects.Add(other);
        }

        IsGrounded = collidingObjects.Count > 0;
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer(layerToCheck))
        {
            collidingObjects.Remove(other);
        }
        IsGrounded = collidingObjects.Count > 0;
    }
}
