using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_charaController : MonoBehaviour
{
    public float moveSpeed = 5f;           
    public float rotationSpeed = 360f;     
    public float frictionFactor = 0.9f;    
    public float noiseFactor = 1f;    
    public LayerMask floorLayerMask;
    public LayerMask wallLayer;
    public AudioSource audioSource;       

    private Vector3 moveDirection = Vector3.zero;
    private float currentSpeed = 0f;
    private float currentNoise = 0f;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();    // Assurez-vous que le joueur a un Rigidbody
    }

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
            float targetAngle = Mathf.Atan2(moveDirection.x, moveDirection.z) * Mathf.Rad2Deg;

            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref rotationSpeed, 0.2f);

            transform.rotation = Quaternion.Euler(0, angle, 0);

            currentSpeed = moveSpeed * (moveDirection.magnitude);
            if (!IsCollidingWithWall())
            {
                transform.Translate(moveDirection * moveSpeed * Time.deltaTime, Space.World);
            }
        }
        else
        {
            currentSpeed *= frictionFactor;
            if (!IsCollidingWithWall())
            {
                transform.Translate(moveDirection * moveSpeed * Time.deltaTime, Space.World);
            }
        }
    }

    void HandleNoise()
    {
        currentNoise = Mathf.Clamp(currentSpeed * noiseFactor, 0, 1);

        if (audioSource != null)
        {
            audioSource.volume = currentNoise;
        }

        Ray ray = new Ray(transform.position, Vector3.down);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 1f, floorLayerMask))
        {
            string surfaceTag = hit.collider.tag;
            switch (surfaceTag)
            {
                case "Carpet":
                    noiseFactor = 0.5f;  // Moins de bruit sur la moquette
                    break;
                case "Tile":
                    noiseFactor = 1.5f;  // Plus de bruit sur le carrelage
                    break;
                default:
                    noiseFactor = 1f;
                    break;
            }
        }
    }
    bool IsCollidingWithWall()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, moveDirection, out hit, 0.5f, wallLayer))
        {
            return true; // Collision détectée avec un mur
        }
        return false;
    }
}
