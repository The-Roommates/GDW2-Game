using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GarbagePile : MonoBehaviour
{
    public float cleanliness;
    [SerializeField] float decay;
    Coroutine cleanlinessDecayCoroutine; // references in case we need to stop these coroutines ever
    Coroutine cleanCorutine;
    [SerializeField] bool isCleaning;
    [SerializeField] bool inRange;
    public float cleanRange;
    public Slider cleanlinessBar;
    public List<Enemy> nearbyEnemies = new List<Enemy>();

    // Start is called before the first frame update
    void Start()
    {
        GameObject cleanlinessBarObject = Instantiate(GameManager.instance.cleanlinessBarPrefab,
                         GameManager.instance.cleanlinessBarHolder);
        cleanlinessBar = cleanlinessBarObject.GetComponent<Slider>();
        cleanlinessBarObject.GetComponentInChildren<TextMeshProUGUI>().text = gameObject.name;
        
        cleanlinessDecayCoroutine = StartCoroutine(CleanlinessDecay());
        cleanCorutine = StartCoroutine(Clean());
    }

    private void Update()
    {
        inRange = (Vector3.Distance(Player.playerTransform.position, transform.position) < cleanRange);
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.TryGetComponent(out Enemy enemy))
        {
            nearbyEnemies.Add(enemy);
            enemy.enemyInstance.SetDestination(transform.position);
            enemy.enemyInstance.targetGarbagePile = this;
        }
    }
    private void OnTriggerExit(Collider col)
    {
        if (col.TryGetComponent(out Enemy enemy))
        {
            nearbyEnemies.Remove(enemy);
            enemy.enemyInstance.targetGarbagePile = null;
        }
    }


    IEnumerator CleanlinessDecay()
    {
        while (true)
        {
            if (isCleaning) { yield return null;  continue; } // if you are actively cleaning this pile, skip decaying it.
            SetBarValue(cleanliness = Mathf.Clamp(cleanliness -= decay, 0, 100));
            if(cleanliness <= 0) {

                GameManager.instance.CleanlinessTimeout();
            // whatever end game/kill player stuff  we need.
            
            }
            yield return new WaitForSecondsRealtime(1);
        }
    }
    void KillPlayer()
    {

    }

    public void DecayFromEnemy(float decay)
    {
        SetBarValue(cleanliness = Mathf.Clamp(cleanliness -= decay, 0, 100));
    }

    public IEnumerator Clean()
    {
        while (true)
        {
            if (inRange && Input.GetKey(KeyCode.Space))
            {
                SetBarValue(cleanliness = Mathf.Clamp(cleanliness += Player.instance.playerStats.cleanStrength, 0, 100));
                yield return new WaitForSecondsRealtime(1);
            }
            else { yield return null; }
            
        }
    }

    void SetBarValue(float cleanliness)
    {
        float value = cleanliness / 100;
        cleanlinessBar.value = value;
        ColorBlock colorBlock = cleanlinessBar.colors;
        colorBlock.disabledColor = GameManager.instance.cleanlinessBarGradient.Evaluate(value); // can't directly modify the
        cleanlinessBar.colors = colorBlock;                                            //sliders colour block, so gotta assign a new one.
    }



}
