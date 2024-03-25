using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Unity.Mathematics;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public static EnemyTypes[] enemyTypesArray = (EnemyTypes[])Enum.GetValues(typeof(EnemyTypes)); // on awake, turns the EnemyTypes enum into an array so that i can randomly select them with an index number.
    public static Transform[] garbagePiles;
    public static float difficultyModifier;
    public GameObject cleanlinessBarPrefab;
    public Transform cleanlinessBarHolder;
    public Gradient cleanlinessBarGradient;
    public LayerMask enemyLayerMask;

    public CanvasGroup gameOverGroup;
    public GameObject gameOverText, anyKeyContinueText;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else { Destroy(this); }

        Application.targetFrameRate = 60;

        // fill garbagepiles with the children of gamemanaager object.
        garbagePiles = GetComponentsInChildren<Transform>().Where(childTransform => childTransform != transform).ToArray();
    }


    public static Vector3 GetGarbagePilePosition()
    {

        return garbagePiles[UnityEngine.Random.Range(0, garbagePiles.Length)].position;
    }
    public static Vector3 GetGarbagePilePosition(int index)
    {
        return garbagePiles[index].position;
    }

    public void CleanlinessTimeout()
    {
        Debug.Log("game over");
    }
    Coroutine gameOverCoroutine;
    public void GameOver()
    {

        gameOverCoroutine = StartCoroutine(LerpAlpha(true));




    }
   public IEnumerator LerpAlpha(bool gameOver=false,float start=0, float end=0.5f)
    {
        gameOverGroup.alpha = start;
        while (gameOverGroup.alpha <= end-0.05f)
        {
            gameOverGroup.alpha += Time.deltaTime;
            yield return null;
        }
        gameOverGroup.alpha = end;
        if (gameOver) { 

            StartCoroutine(FlashGameOverText());
        }
        else { SceneManager.LoadScene("StartMenu");}
    }
    IEnumerator FlashGameOverText()
    {
        StartCoroutine(WaitForInput());
        anyKeyContinueText.SetActive(true);
        while (true)
        {
            gameOverText.SetActive(true);
            yield return new WaitForSecondsRealtime(0.33f);
            gameOverText.SetActive(false);
            yield return new WaitForSecondsRealtime(0.33f);
        }
    }

    IEnumerator WaitForInput()
    {
        yield return new WaitUntil(() => Input.anyKeyDown);
        SceneManager.LoadScene("StartMenu", LoadSceneMode.Single);
    }

}
