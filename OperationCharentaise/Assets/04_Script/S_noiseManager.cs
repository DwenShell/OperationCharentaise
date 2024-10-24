using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_noiseManager : MonoBehaviour
{
    [SerializeField] private float globalNoiseFactor;
    [SerializeField] private GameObject playerCharacter;
    [SerializeField] private float detectionRadius = 1f;
    [SerializeField] private LayerMask soundDetectionLayer;
    [SerializeField] private LayerMask chaserLayer;
    public bool isSoundable;

    private List<Collider> detectedColliders = new List<Collider>();
    private Collider[] newHitColliders;

    private List<S_playerChaser> chaserList = new List<S_playerChaser>();
    private List<S_playerChaser> previousChaserList = new List<S_playerChaser>();
    private Collider[] newChaserColliders;

    void Update()
    {
        globalNoiseFactor = 0f;

        newHitColliders = Physics.OverlapSphere(playerCharacter.transform.position, detectionRadius, soundDetectionLayer);
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
            S_soundEmiter soundEmiter = col.gameObject.GetComponent<S_soundEmiter>();
            if (soundEmiter != null)
            {
                globalNoiseFactor += soundEmiter.noiseObjectFactor;
            }
        }

        UpdateChasers();
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

    private void UpdateChasers()
    {
        newChaserColliders = Physics.OverlapSphere(playerCharacter.transform.position, detectionRadius, chaserLayer);

        previousChaserList.Clear();
        previousChaserList.AddRange(chaserList);

        chaserList.Clear(); 

        foreach (Collider chaserCollider in newChaserColliders)
        {
            S_playerChaser chaser = chaserCollider.GetComponent<S_playerChaser>();
            if (chaser != null)
            {
                chaserList.Add(chaser); 
                chaser.isPlayerSoundable = isSoundable;  
                chaser.player = playerCharacter.transform;
            }
        }

        foreach (S_playerChaser prevChaser in previousChaserList)
        {
            if (!chaserList.Contains(prevChaser))
            {
                prevChaser.isPlayerSoundable = false;
            }
        }
    }

    public float GetCurrentNoiseLevel()
    {
        return Mathf.Clamp(globalNoiseFactor, 0f, 2f);
    }
}
