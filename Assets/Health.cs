using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    public static Health instance;
    static Text healthText;

    private void Start()
    {
        healthText = GetComponent<Text>();
    }
    public static void UpdateHealthText()
    {
        PlayerStats stats = T_PlayerMovement.instance.playerStats;
        healthText.text = "%" + ((stats.currentHP*100f/ stats.maxHP)).ToString();
    }
}
