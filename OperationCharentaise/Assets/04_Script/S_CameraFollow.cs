using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_CameraFollow : MonoBehaviour
{
    public Transform target;      
    public Vector3 offset;          
    public float followSpeed = 5f;   

    private Vector3 currentVelocity = Vector3.zero;

    void LateUpdate()
    {
        if (target != null)
        {
            Vector3 targetPosition = target.position + offset;

            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref currentVelocity, 1 / followSpeed);
        }
    }
}
