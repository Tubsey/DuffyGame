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

    void Update()
    {
        MovePlatform();
    }

    void OnCollisionEnter(Collision col)
    {
        col.transform.SetParent(transform);
    }

    void OnCollisionExit(Collision col)
    {
        col.transform.SetParent(null);
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
