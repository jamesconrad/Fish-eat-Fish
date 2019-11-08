using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTracker : MonoBehaviour
{
    public Transform target;
    public Transform fishContainer;
    public float camSpeed = 0.1f;

    void Update()
    {
        Vector3 pos = target.position;
        pos.z = transform.position.z;
        transform.position = Vector3.MoveTowards(transform.position, pos, camSpeed);
    }
}
