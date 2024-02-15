using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoToDemo : MonoBehaviour
{
    public void NextScene()
    {
        SceneManager.LoadScene("TechDemo");
    }
}
