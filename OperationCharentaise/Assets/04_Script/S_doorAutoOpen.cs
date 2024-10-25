using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_doorAutoOpen : MonoBehaviour
{
    public float openAngle = 90f; 
    public float openSpeed = 2f;  
    public float detectionRadius = 3f; 

    private Quaternion closedRotation;
    private Quaternion openRotation;
    private bool isOpening = false;
    private bool isPlayerOrChaserNear = false;
    private Collider doorCollider;

    public LayerMask detectionLayer;

    void Start()
    {
        closedRotation = transform.rotation;
        openRotation = Quaternion.Euler(0, openAngle, 0) * closedRotation;
        doorCollider = GetComponent<Collider>();
    }

    void Update()
    {
        DetectPlayerOrChaser();
        UpdateDoorState();
    }

    void DetectPlayerOrChaser()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, detectionRadius, detectionLayer);

        isPlayerOrChaserNear = colliders.Length > 0; 
    }

    void UpdateDoorState()
    {
        if (isPlayerOrChaserNear && !isOpening)
        {
            StartCoroutine(OpenDoor());
        }
        else if (!isPlayerOrChaserNear && isOpening)
        {
            StartCoroutine(CloseDoor());
        }
    }

    IEnumerator OpenDoor()
    {
        isOpening = true;
        doorCollider.enabled = false;

        float elapsedTime = 0f;
        while (elapsedTime < 1f / openSpeed)
        {
            transform.rotation = Quaternion.Lerp(closedRotation, openRotation, elapsedTime * openSpeed);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.rotation = openRotation;
    }

    IEnumerator CloseDoor()
    {
        float elapsedTime = 0f;
        while (elapsedTime < 1f / openSpeed)
        {
            transform.rotation = Quaternion.Lerp(openRotation, closedRotation, elapsedTime * openSpeed);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.rotation = closedRotation; // S'assurer que l'angle est bien atteint
        doorCollider.enabled = true;
        isOpening = false;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
