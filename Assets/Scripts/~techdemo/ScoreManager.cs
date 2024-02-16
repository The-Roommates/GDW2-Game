using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public Text score;
    int scoreCount = 0;

    public AudioSource coinsnd;
    public AudioClip coin_;


    // Start is called before the first frame update
    void Start()
    {
        score.text = scoreCount.ToString();

    }

    // Update is called once per frame
    void Update()
    {
        score.text = scoreCount.ToString();

        //score.text = 

        if (Input.GetKeyUp(KeyCode.O))
        {
            scoreCount++;
        }
        
        


    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            gameObject.SetActive(false);
            scoreCount = scoreCount + 1;

            coinsnd.PlayOneShot(coin_);

        }
    }
}
