using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;
    public Text score;
    int scoreCount = 0;

    public AudioSource coinsnd;
    public AudioClip coin_;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }
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

        }




    }
    public void EnemyKilled()
    {
        scoreCount+=3;
        score.text=scoreCount.ToString();
    }
    public void CoinCollect()
    {
        scoreCount ++;
        score.text = scoreCount.ToString();
        Debug.Log(scoreCount);
        coinsnd.PlayOneShot(coin_);
    }
}
