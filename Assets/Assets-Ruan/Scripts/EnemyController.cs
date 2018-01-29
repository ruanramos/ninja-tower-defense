using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour
{
    GameObject player;
    public float speed;
    private Transform yellowTarget;
    private int waypointIndex = 0;
    public int waypointCount;

    public float totalHealth;
    public float currentHealth;
    private bool willDie = false;

    public float timeTookDamage;
    public float healthBarDuration;

    //public float totalWaypointsToObjective;

    public GameObject totalHealthImage;
    public GameObject damageTakenImage;
    public GameObject barBorder;

    private GameObject gameController;


    void Start()
    {
        player = GameObject.FindGameObjectWithTag("player");
        yellowTarget = Waypoints.yellowWaypoints[0];
        gameController = GameObject.Find("GameController");
        speed = 2f;
        totalHealth = 10f;
        currentHealth = totalHealth;
        //totalWaypointsToObjective = GameObject.FindGameObjectsWithTag("Waypoint").Length;
        waypointCount = GameObject.Find("YellowWaypoints").transform.childCount;

        totalHealthImage.GetComponent<Image>().enabled = false;
        damageTakenImage.GetComponent<Image>().enabled = false;
        barBorder.GetComponent<Image>().enabled = false;
    }

    void Update()
    {
        // positions the health bar on top of the enemy
        totalHealthImage.GetComponent<Image>().transform.position = Camera.main.WorldToScreenPoint(transform.position) + Vector3.up * (Screen.height * 0.04f) + Vector3.left * (Screen.width * 0.008f);
        damageTakenImage.GetComponent<Image>().transform.position = Camera.main.WorldToScreenPoint(transform.position) + Vector3.up * (Screen.height * 0.04f) + Vector3.left * (Screen.width * 0.008f);
        barBorder.GetComponent<Image>().transform.position = Camera.main.WorldToScreenPoint(transform.position) + Vector3.up * (Screen.height * 0.04f) + Vector3.left * (Screen.width * 0.008f);

        // calculates the fill amount of the health bar
        if((totalHealth - currentHealth) != 0)
        {
            damageTakenImage.GetComponent<Image>().fillAmount = (totalHealth - currentHealth) / totalHealth;
        }
        else
        {
            damageTakenImage.GetComponent<Image>().fillAmount = 0;
        }

        Vector3 yellowDirection = yellowTarget.position - transform.position;

        // actually move to waypoint
        transform.Translate(yellowDirection.normalized * speed * Time.deltaTime, Space.World);

        if (Vector3.Distance(transform.position, yellowTarget.position) <= 0.1f)
        {
            GetNextWaypoint();
        }
        
        if(currentHealth <= 0)
        {
            gameController.GetComponent<GameController>().kujiKiri += 10;
            willDie = true;
            Destroy(gameObject);
        }



        // Follow the player
        /*
        Vector3 target = player.position;
        float targetX = target.x - transform.position.x;
        float targetY = target.y - transform.position.y;
        transform.Translate(new Vector3(targetX, targetY, 0).normalized * Time.deltaTime * speed);
        */
    }

    void GetNextWaypoint()
    {
        if(waypointIndex >= Waypoints.yellowWaypoints.Length - 1)
        {
            Destroy(gameObject);
            return;
        }
        waypointIndex++;
        //totalWaypointsToObjective = WaypointsToObjective();
        yellowTarget = Waypoints.yellowWaypoints[waypointIndex];
    }

    //private float WaypointsToObjective()
    //{
    //    totalWaypointsToObjective -= waypointIndex;
    //    return totalWaypointsToObjective;
    //}

    
    // Coroutine to show the health bar after taking damage
    public IEnumerator ShowHealthBar(GameObject healthBarImage, GameObject damageImage, GameObject border, float delay)
    {
        healthBarImage.GetComponent<Image>().enabled = true;
        damageImage.GetComponent<Image>().enabled = true;
        border.GetComponent<Image>().enabled = true;
        yield return new WaitForSeconds(delay);
        if (!willDie)
        {
            healthBarImage.GetComponent<Image>().enabled = false;
            damageImage.GetComponent<Image>().enabled = false;
            border.GetComponent<Image>().enabled = false;
        }
    }

    public void TakeDamage(int damage)
    {
        // deal damage to enemy
        currentHealth -= damage;
        // make health bar pop
        StartCoroutine(ShowHealthBar(totalHealthImage, damageTakenImage, barBorder, 1.5f));
    }

}
