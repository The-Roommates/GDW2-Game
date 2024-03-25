using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    Text timerText;
    int time;
    // Start is called before the first frame update
    void Start()
    {
        timerText = GetComponent<Text>();   
        StartCoroutine(timer()); 
    }

    IEnumerator timer()
    {
        while (true)
        {
            AddTime();
            yield return new WaitForSeconds(1f);
        }
    }

    void AddTime()
    {
        time++;
        timerText.text = time.ToString();
    }
}
