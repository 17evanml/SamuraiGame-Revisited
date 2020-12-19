using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerScript : MonoBehaviour
{
    Text time;
    Image visual;

    float maxTime = 0;
    // Start is called before the first frame update
    void Start()
    {
        visual = GetComponent<Image>();
        time = GetComponentInChildren<Text>();
    }
        
    public void StartTimer(float _maxTime)
    {
        maxTime = _maxTime;
        StartCoroutine(Timer(maxTime));
    }

    public IEnumerator Timer(float timeRemaining)
    {
        yield return 0;
        timeRemaining -= Time.deltaTime;
        visual.fillAmount = timeRemaining / maxTime;
    }
}
