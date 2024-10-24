using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using UnityEngine;
using UnityEngine.UI;

public class S_charaController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;           
    [SerializeField] private float rotationSpeed = 360f;     
    [SerializeField] private float frictionFactor = 0.9f;    
    [SerializeField] private float noiseFactor = 1f;    
    [SerializeField] private LayerMask floorLayerMask;
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private AudioSource audioSource;

    [SerializeField] private Transform spawnPoint;

    private Vector3 moveDirection = Vector3.zero;
    [SerializeField] private float currentSpeed = 0f;
    public float currentNoise = 0f;

    void Update()
    {
        HandleMovement();
        HandleNoise();
    }

    void HandleMovement()
    {
        float horizontal = Input.GetAxis("Horizontal");  // A/D ou Q/D
        float vertical = Input.GetAxis("Vertical");      // W/S ou Z/S

        moveDirection = new Vector3(horizontal, 0, vertical).normalized;
        if (moveDirection.magnitude >= 0.1f)
        {
            CheckGroundType();
            float targetAngle = Mathf.Atan2(moveDirection.x, moveDirection.z) * Mathf.Rad2Deg;

            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref rotationSpeed, 0.2f);

            transform.rotation = Quaternion.Euler(0, angle, 0);

            currentSpeed = moveSpeed * (moveDirection.magnitude);
            currentSpeed *= frictionFactor;
            if (!IsCollidingInFrontOfWhat(1))
            {
                transform.Translate(moveDirection * currentSpeed * Time.deltaTime, Space.World);
            }
        }
        else
        {
            currentSpeed *= frictionFactor;
            if (!IsCollidingInFrontOfWhat(1))
            {
                transform.Translate(moveDirection * moveSpeed * Time.deltaTime, Space.World);
            }
        }
    }

    void HandleNoise()
    {
        currentNoise = Mathf.Clamp((currentSpeed*0.5f) * noiseFactor, 0f, 2f);

        if (audioSource != null)
        {
            audioSource.volume = currentNoise;
        }
    }

    private void CheckGroundType()
    {
        Ray ray = new Ray(transform.position, Vector3.down);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 1f, floorLayerMask))
        {
            string surfaceTag = hit.collider.tag;
            switch (surfaceTag)
            {
                case "Carpet":
                    frictionFactor = 0.2f;
                    noiseFactor = 0.5f;
                    break;
                case "Tile":
                    frictionFactor = 0.9f;
                    noiseFactor = 1.5f; 
                    break;
                case "Lino":
                    frictionFactor = 0.5f;
                    noiseFactor = 1f;
                    break;
                default:
                    noiseFactor = 1f;
                    break;
            }
        }
    }

    bool IsCollidingInFrontOfWhat(int typeOfCollideAsk)
    {
        RaycastHit hit;
        switch (typeOfCollideAsk)
        {
            case 1:
                if (Physics.Raycast(transform.position, moveDirection, out hit, 0.5f, wallLayer))
                {
                    return true;
                }
                return false;
            case 2:
                if (Physics.Raycast(transform.position, moveDirection, out hit, 1.5f, wallLayer))
                {
                    if(hit.collider.tag == "Pickable")
                    return true;
                }
                return false;
            default: 
                return false;
        }

    }
    public float GetCurrentNoiseLevel()
    {
        return Mathf.Clamp(currentNoise, 0f, 2f);
    }
    public void playerIsCapture()
    {
        transform.position = spawnPoint.position;
        transform.rotation = spawnPoint.rotation;
    }
}
