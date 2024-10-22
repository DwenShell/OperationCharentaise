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
        // Initialisation du niveau de bruit global à 0 au début de chaque Update
        globalNoiseFactor = 0f;

        // Détection des nouveaux objets dans le rayon
        Collider[] newHitColliders = Physics.OverlapSphere(playerCharacter.transform.position, detectionRadius, detectionLayer);

        // Mise à jour de la liste des objets détectés
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

        // Debug : Afficher les objets détectés
        foreach (Collider col in detectedColliders)
        {
            Debug.Log(col.gameObject.name);
        }
    }

    // Vérifie si un collider est toujours dans la sphère de détection
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
