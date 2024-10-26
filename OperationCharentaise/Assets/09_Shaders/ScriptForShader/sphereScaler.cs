using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereScaler : MonoBehaviour
{
    public float miniScale = 0f;
    public float maxScale = 2.0f;
    public float scaleSpeed = 0.5f;
    public float speedReducWhenDeflate = 1f;
    public Color sphereColor;

    private bool scalingUp = true;

    public bool isSimpleRay;
    [Header("Don't complete bellow if simple ray")]

    public GameObject prefabSphereRay;
    public int numberOfRay;
    public float delayBetweenSpawn;
    public bool isPalmSphere;

    private List<GameObject> spawnedRays = new List<GameObject>();

    private Material principleMaterial;
    private Material rayMaterial;
    private void Start()
    {
        if (!isSimpleRay)
        {
            InvokeRepeating("RaySpawner", 0, delayBetweenSpawn);
            principleMaterial = new Material(GetComponent<Renderer>().material);
            principleMaterial.SetColor("_ColorLine", sphereColor);
            GetComponent<Renderer>().material = principleMaterial;
            rayMaterial = new Material(principleMaterial);
            rayMaterial.SetFloat("_Range", 12.92f);
        }
    }
    void Update()
    {
        ScaleSphere();
        CheckRayScale();
    }

    void ScaleSphere()
    {
        Vector3 currentScale = transform.localScale;

        if (scalingUp)
        {
            currentScale += Vector3.one * scaleSpeed * Time.deltaTime;

            if (currentScale.x >= maxScale)
            {
                currentScale = new Vector3(maxScale, maxScale, maxScale);
                scalingUp = false;
            }
        }
        else
        {
            currentScale -= Vector3.one * (scaleSpeed * speedReducWhenDeflate) * Time.deltaTime;

            if (currentScale.x <= miniScale)
            {
                currentScale = new Vector3(miniScale, miniScale, miniScale);
                scalingUp = true;
                if(isPalmSphere)
                {
                    for (int i = spawnedRays.Count - 1; i >= 0; i--)
                    {
                        GameObject sphereRays = spawnedRays[i];
                        if (sphereRays != null)
                        {
                            Destroy(sphereRays);
                            spawnedRays.RemoveAt(i);
                        }
                        else
                        {
                            spawnedRays.RemoveAt(i);
                        }
                    }
                    CancelInvoke();
                    Destroy(gameObject);
                }
            }
        }

        transform.localScale = currentScale;
    }
    void RaySpawner()
    {
        if (spawnedRays.Count < numberOfRay)
        {
            GameObject newRay = Instantiate(prefabSphereRay, transform.position, Quaternion.identity);
            spawnedRays.Add(newRay);
            newRay.GetComponent<Renderer>().material = rayMaterial;
        }
    }
    void CheckRayScale()
    {
        for (int i = spawnedRays.Count - 1; i >= 0; i--)
        {
            GameObject sphereRays = spawnedRays[i];
            if (sphereRays != null)
            {
                if (sphereRays.transform.localScale.x > transform.localScale.x)
                {
                    Destroy(sphereRays);
                    spawnedRays.RemoveAt(i);
                }
            }
            else
            {
                spawnedRays.RemoveAt(i);
            }
        }
    }
}