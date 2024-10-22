using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_noiseManager : MonoBehaviour
{
    [SerializeField] private float globalNoiseFactor;
    [SerializeField] private GameObject playerCharacter;
    [SerializeField] private float detectionRadius = 1f; 
    [SerializeField] private LayerMask detectionLayer;

    private List<Collider> detectedColliders = new List<Collider>(); 

    void Update()
    {
        Collider[] newHitColliders = Physics.OverlapSphere(playerCharacter.transform.position, detectionRadius, detectionLayer);

        detectedColliders.RemoveAll(collider => !IsInOverlapSphere(collider, newHitColliders));

        foreach (Collider collider in newHitColliders)
        {
            if (!detectedColliders.Contains(collider))
            {
                detectedColliders.Add(collider);
            }
        }

        foreach (Collider col in detectedColliders)
        {
            Debug.Log(col.gameObject.name);
        }
    }

    private bool IsInOverlapSphere(Collider collider, Collider[] hitColliders)
    {
        foreach (Collider hitCollider in hitColliders)
        {
            if (hitCollider == collider)
            {
                return true; 
            }
        }
        return false; 
    }

    public float GetCurrentNoiseLevel()
    {
        return Mathf.Clamp(globalNoiseFactor, 0f, 2f);
    }
}
