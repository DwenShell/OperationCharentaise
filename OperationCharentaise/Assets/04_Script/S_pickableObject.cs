using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_pickableObject : MonoBehaviour
{
    public int objectCode;
    public float floatAmplitude = 0.5f;       // Hauteur du mouvement de flottement
    public float floatFrequency = 1f;         // Vitesse du mouvement de flottement
    public float rotationAmplitude = 10f;     // Amplitude de la rotation en degrés
    public float rotationFrequency = 1f;      // Vitesse de l'oscillation en rotation

    private Vector3 startPosition;
    private Quaternion startRotation;

    void Start()
    {
        startPosition = transform.position;
        startRotation = transform.rotation;
    }

    void Update()
    {
        float newY = startPosition.y + Mathf.Sin(Time.time * floatFrequency) * floatAmplitude;
        transform.position = new Vector3(startPosition.x, newY, startPosition.z);

        float rotationY = Mathf.Sin(Time.time * rotationFrequency) * rotationAmplitude;
        float rotationX = Mathf.Sin(Time.time * rotationFrequency) * rotationAmplitude;
        transform.rotation = startRotation * Quaternion.Euler(rotationX, rotationY, 0);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<S_charaController>().getOrGiveObject(objectCode, true);
            Destroy(gameObject);
        }
    }
}
