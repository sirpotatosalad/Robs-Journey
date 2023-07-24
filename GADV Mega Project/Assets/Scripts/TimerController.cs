using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class TimerController : MonoBehaviour
{
    float currentTime;
    public TextMeshProUGUI currentTimeText;

    public HealthController player;
    

    // Start is called before the first frame update
    void Start()
    {
        currentTime = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (!player.isDead)
        {
            currentTime += Time.deltaTime;
            TimeSpan time = TimeSpan.FromSeconds(currentTime);
            currentTimeText.text = time.ToString(@"mm\:ss\:fff"); currentTime += Time.deltaTime;
        }
    }
}
