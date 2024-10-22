using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class S_noiseDisplay : MonoBehaviour
{
    public Slider noisePlaySlider;
    public Slider noiseAmbSlider;
    public S_charaController player; 
    public S_noiseManager noiseManager;

    public float lerpSpeed = 2f;         
    public float noiseFluctuation = 0.05f; 
    public float fluctuationSpeed = 0.1f;  

    [SerializeField] private float playerNoise;
    [SerializeField] private float targetPlayNoise;
    [SerializeField] private float ambNoise;
    [SerializeField] private float targetAmbNoise;

    void Start()
    {
        playerNoise = player.GetCurrentNoiseLevel();
        ambNoise = noiseManager.GetCurrentNoiseLevel();
    }

    void Update()
    {
        targetPlayNoise = player.GetCurrentNoiseLevel();

        playerNoise = Mathf.Lerp(playerNoise, targetPlayNoise, Time.deltaTime * lerpSpeed);

        if (Mathf.Abs(targetPlayNoise - playerNoise) < 0.01f)
        {
            playerNoise += Mathf.PerlinNoise(Time.time * fluctuationSpeed, 0f) * noiseFluctuation - (noiseFluctuation / 2);
        }

        noisePlaySlider.value = Mathf.Clamp(playerNoise, 0f, 2f);
        
        //===========================
        
        targetAmbNoise = noiseManager.GetCurrentNoiseLevel();

        ambNoise = Mathf.Lerp(ambNoise, targetAmbNoise, Time.deltaTime * lerpSpeed);

        if (Mathf.Abs(targetAmbNoise - ambNoise) < 0.01f)
        {
            ambNoise += Mathf.PerlinNoise(Time.time * fluctuationSpeed, 0f) * noiseFluctuation - (noiseFluctuation / 2);
        }

        noiseAmbSlider.value = Mathf.Clamp(ambNoise, 0f, 2f);
    }
}
