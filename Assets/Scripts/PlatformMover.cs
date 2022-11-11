using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformMover : MonoBehaviour
{
    [SerializeField] Transform[] pts;
    [SerializeField] int startPt;
    [SerializeField] float switchDist = .02f;
    public float speed;
    private int i;


    void Start()
    {
        transform.position = pts[startPt].position;
    }

    void FixedUpdate()
    {
        MovePlatform();
    }

    void OnTriggerEnter(Collider col)
    {
        if (!col.CompareTag("PlayerFeet")) return;
        col.transform.GetComponentInParent<PlayerMovementCC>().SetParentObject(transform);
    }
    void OnTriggerExit(Collider col)
    {
        if (!col.CompareTag("PlayerFeet")) return;
        col.transform.GetComponentInParent<PlayerMovementCC>().SetParentObject(null);
    }

    void MovePlatform()
    {
        if (Vector3.Distance(transform.position, pts[i].position) < switchDist)
        {
            i++;
            if (i == pts.Length)
            {
                i = 0;
            }
        }
        transform.position = Vector3.MoveTowards(transform.position, pts[i].position, speed * Time.deltaTime);
    }
}
