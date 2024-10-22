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
    [SerializeField] private float targetNoise;        

    void Start()
    {
        playerNoise = player.GetCurrentNoiseLevel();
    }

    void Update()
    {
        targetNoise = player.GetCurrentNoiseLevel();

        playerNoise = Mathf.Lerp(playerNoise, targetNoise, Time.deltaTime * lerpSpeed);

        if (Mathf.Abs(targetNoise - playerNoise) < 0.01f)
        {
            playerNoise += Mathf.PerlinNoise(Time.time * fluctuationSpeed, 0f) * noiseFluctuation - (noiseFluctuation / 2);
        }

        noisePlaySlider.value = Mathf.Clamp(playerNoise, 0f, 2f);
    }
}
