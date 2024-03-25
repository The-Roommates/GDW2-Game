using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RequirementMessageHolder : MonoBehaviour
{
    public static RequirementMessageHolder instance;
    [SerializeField] GameObject panel;
    [SerializeField] Text coinCount, ratCount, cockroachCount;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else { Destroy(this); }
    }
    public void ShowRequirements(RequirementMessage message)
    {
        panel.SetActive(true);
        coinCount.text = message.coinCount.ToString();
        cockroachCount.text = message.cockroachCount.ToString();
        ratCount.text = message.ratCount.ToString();

    }
    public void HideRequirements()
    {
        panel.SetActive(false);
    }
}
