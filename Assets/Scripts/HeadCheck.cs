using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(BoxCollider))]
public class HeadCheck : MonoBehaviour
{

    HashSet<Collider> collidingObjects = new HashSet<Collider>();

    public bool IsHeadTouched { get; set; }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("Player") && !other.isTrigger)
        {
            collidingObjects.Add(other);
        }

        IsHeadTouched = collidingObjects.Count > 0;
    }
    private void OnTriggerExit(Collider other)
    {
        if (!other.gameObject.CompareTag("Player") && !other.isTrigger)
        {
            collidingObjects.Remove(other);
        }
        IsHeadTouched = collidingObjects.Count > 0;
    }
}
