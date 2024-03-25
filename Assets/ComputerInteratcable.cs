using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ComputerInteratcable : MonoBehaviour
{
    public float radius;
    [SerializeField] CanvasGroup group;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(T_PlayerMovement.instance.transform.position, transform.position) < radius)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (CheckComplete(out RequirementMessage message))
                {
                    StartCoroutine(GameManager.instance.LerpAlpha(false, 0, 1));
                }
                else { RequirementMessageHolder.instance.ShowRequirements(message); }
            }
        }
        else
        {
            RequirementMessageHolder.instance.HideRequirements();
        }
    }


    bool CheckComplete(out RequirementMessage message)
    {
        Enemy[] enemies = FindObjectsOfType<Enemy>();
        Coin[] coins = FindObjectsOfType<Coin>();
        if (enemies.Length <=0 && coins.Length <=0)
        {
            message = null;
            return true;
        }
        message = ProcessRequirements(enemies, coins);
        return false;
    }

    RequirementMessage ProcessRequirements(Enemy[] enemies, Coin[]coin)
    {
        int coinCount = coin.Length, cockroachCount=0, ratCount=0, raccoonCount=0;
        foreach(Enemy e in enemies)
        {
            switch (e.enemyType)
            {
                case EnemyTypes.Cockroach:
                    cockroachCount++;
                    break;
                case EnemyTypes.Rat:
                    ratCount++;
                    break;
                case EnemyTypes.Raccoon:
                    raccoonCount++; 
                    break;
                default:
                    Debug.LogError("Enemy with unassigned type detected");
                    break;
            }
        }
        return new RequirementMessage(coinCount, cockroachCount, ratCount, raccoonCount);

    }
}

public class RequirementMessage
{
    public int coinCount, cockroachCount, ratCount, raccoonCount;
    public RequirementMessage(int coinCount, int cockroachCount, int ratCount, int raccoonCount)
    {
        this.coinCount = coinCount;
        this.cockroachCount = cockroachCount;
        this.ratCount = ratCount;
        this.raccoonCount = raccoonCount;
    }
}
