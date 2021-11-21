using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    
    void Start()
    {
        
    }

    [Header("Distance Score")]
    public float distanceCounter;
    public TextMeshProUGUI textDistance;

    [Header("Best Distance Score")]
    public float bestDistanceCounter;
    public TextMeshProUGUI textBestDistance;

    void Update()
    {
        UpdateTimerText();
    }

    private void UpdateTimerText()
    {
        distanceCounter += 0.05f;
        textDistance.SetText($"{distanceCounter:F0} M");
    }







}
