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
        playerNoise = UpdateNoise(playerNoise, player.GetCurrentNoiseLevel(), noisePlaySlider);

        ambNoise = UpdateNoise(ambNoise, noiseManager.GetCurrentNoiseLevel(), noiseAmbSlider);
        if(playerNoise > ambNoise) { noiseManager.isSoundable = true; } else { noiseManager.isSoundable = false;}
    }

    private float UpdateNoise(float currentNoise, float targetNoise, Slider noiseSlider)
    {
        currentNoise = Mathf.Lerp(currentNoise, targetNoise, Time.deltaTime * lerpSpeed);

        if (Mathf.Abs(targetNoise - currentNoise) < 0.01f)
        {
            currentNoise += Mathf.PerlinNoise(Time.time * fluctuationSpeed, 0f) * noiseFluctuation - (noiseFluctuation / 2);
        }

        noiseSlider.value = Mathf.Clamp(currentNoise, 0f, 2f);

        return currentNoise;
    }
}
