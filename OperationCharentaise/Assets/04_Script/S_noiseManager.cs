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
        // Initialisation du niveau de bruit global � 0 au d�but de chaque Update
        globalNoiseFactor = 0f;

        // D�tection des nouveaux objets dans le rayon
        Collider[] newHitColliders = Physics.OverlapSphere(playerCharacter.transform.position, detectionRadius, detectionLayer);

        // Mise � jour de la liste des objets d�tect�s
        detectedColliders.RemoveAll(collider => !IsInOverlapSphere(collider, newHitColliders));

        foreach (Collider collider in newHitColliders)
        {
            if (!detectedColliders.Contains(collider))
            {
                detectedColliders.Add(collider);
            }
        }

        // Calcul du globalNoiseFactor en fonction des objets toujours dans la zone
        foreach (Collider col in detectedColliders)
        {
            S_soundEmiter soundEmiter = col.gameObject.GetComponent<S_soundEmiter>();
            if (soundEmiter != null)
            {
                globalNoiseFactor += soundEmiter.noiseObjectFactor;
            }
        }

        // Debug : Afficher les objets d�tect�s
        foreach (Collider col in detectedColliders)
        {
            Debug.Log(col.gameObject.name);
        }
    }

    // V�rifie si un collider est toujours dans la sph�re de d�tection
    private bool IsInOverlapSphere(Collider collider, Collider[] hitColliders)
    {
        foreach (Collider hitCollider in hitColliders)
        {
            if (hitCollider == collider)
            {
                return true;
            }
        }
        return false; // L'objet n'est plus dans la zone
    }

    public float GetCurrentNoiseLevel()
    {
        return Mathf.Clamp(globalNoiseFactor, 0f, 2f);
    }
}
