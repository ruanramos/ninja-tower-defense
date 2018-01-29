using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clone : MonoBehaviour {

    public float range;
    private float rateOfFire = 0.5f;
    private float nextFire = 0;
    public GameObject prefabShurikenClone;
    public float shootingForce = 6f;

    private bool showingRange = false;

    private GameObject gameController;

    private GameObject cloneParticle;
    private float particleTime = 0;

    private bool selected = false;
    private bool hovered = false;

    private void Awake()
    {
        this.GetComponent<SpriteRenderer>().enabled = false;
    }

    // Use this for initialization
    void Start () {
        gameController = GameObject.Find("GameController");
        cloneParticle = transform.FindChild("CloneParticle").gameObject;
    }

    private void FixedUpdate()
    {
        // Find closest enemies to objective and put them on a list
        //List<GameObject> closestEnemiesToObjective = FindCloserEnemiesToObjective();
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject enemyToShoot = null;
        List<GameObject> possibleTargets = new List<GameObject>();
        List<float> possibleTargetsX = new List<float>();
        // check what enemy to shoot. Actually will shoot the enemy that's further on the patch AND inside range
        foreach (GameObject enemy in enemies)
        {
            if(DistanceToEnemy(enemy) < range + 0.5f)
            {
                possibleTargets.Add(enemy);
                possibleTargetsX.Add(enemy.transform.position.x);
                //enemyToShoot = enemy;
            }
        }
        possibleTargetsX.Sort();
        foreach (GameObject enemy in possibleTargets)
        {
            if(enemy.transform.position.x == possibleTargetsX[0])
            {
                enemyToShoot = enemy;
            }
        }
        
        // make ranges of selected clones apear
        if (selected && transform.Find("RangeMarker").gameObject.GetComponent<LineRenderer>().enabled == false)
        {
            transform.Find("RangeMarker").gameObject.GetComponent<LineRenderer>().enabled = true;
            showingRange = true;
        }
        else if (!selected && transform.Find("RangeMarker").gameObject.GetComponent<LineRenderer>().enabled == true)
        { 
            transform.Find("RangeMarker").gameObject.GetComponent<LineRenderer>().enabled = false;
            showingRange = false;
        }

        // rotate to target
        if (enemyToShoot != null)
        {
            RotateToEnemy(enemyToShoot);
        }

        // Shoot locked enemy with fire rate delay
        if (Time.time > nextFire)
        {
            if (enemyToShoot != null)
            {
                nextFire = Time.time + rateOfFire;
                Fire_Shuriken(prefabShurikenClone, enemyToShoot);
            }
        }
    }

    private void Update()
    {
        particleTime += Time.deltaTime;
        if(particleTime >= 0.7f)
        {
            Destroy(cloneParticle);
            this.GetComponent<SpriteRenderer>().enabled = true;
        }
    }

    private GameObject FindClosestEnemy()
    {
        GameObject[] enemies;
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject closest = null;
        float distance = Mathf.Infinity;
        Vector3 position = transform.position;
        foreach (GameObject enemy in enemies)
        {
            Vector3 diff = enemy.transform.position - position;
            float curDistance = diff.sqrMagnitude;
            if (curDistance < distance)
            {
                closest = enemy;
                distance = curDistance;
            }
        }
        return closest;
    }
/*
    private List<GameObject> FindCloserEnemiesToObjective()
    {
        GameObject[] enemies;
        List<GameObject> orderedEnemies = new List<GameObject>();
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        float waypoints = Mathf.Infinity;
        GameObject closest = null;
        foreach (GameObject enemy in enemies)
        {
            if (enemy.GetComponent<EnemyController>().totalWaypointsToObjective < waypoints)
            {
                waypoints = enemy.GetComponent<EnemyController>().totalWaypointsToObjective;
                closest = enemy;
                orderedEnemies.Add(closest);
            }
        }

        return orderedEnemies;
    }
*/
    private void RotateToEnemy(GameObject enemy)
    {
        Vector3 target = enemy.transform.position;
        float targetX = target.x - transform.position.x;
        float targetY = target.y - transform.position.y;
        transform.Rotate(new Vector3(targetX, targetY, 0));

        // Get Angle in Radians
        float AngleRad = Mathf.Atan2(target.y - transform.position.y, target.x - transform.position.x);
        // Get Angle in Degrees
        float AngleDeg = (180 / Mathf.PI) * AngleRad;
        // Rotate Object
        this.transform.rotation = Quaternion.Euler(0, 0, AngleDeg - 90);
    }

    private void Fire_Shuriken(GameObject prefabShurikenClone, GameObject closestEnemy)
    {

        GameObject shuriken = Instantiate(prefabShurikenClone, transform.position, transform.rotation);

        Vector3 objPos = transform.position;
        Vector3 target = closestEnemy.transform.position;
        float targetX = target.x - objPos.x;
        float targetY = target.y - objPos.y;
        shuriken.GetComponent<Rigidbody2D>().velocity = ((new Vector2(targetX, targetY).normalized) * shootingForce);
    }

    private float DistanceToEnemy(GameObject enemy)
    {
        return Vector2.Distance(transform.position, enemy.transform.position);
    }

    private void OnMouseDown()
    {
        
        
    }

    private void OnMouseOver()
    {
        gameController.GetComponent<CursorController>().SetCursorToHover();
        hovered = true;
        if (Input.GetMouseButtonDown(0))
        {
            int numberOfClonesShowingRange = 0;

            foreach (GameObject shurikenClone in GameObject.FindGameObjectsWithTag("Shuriken Clone"))
            {
                if (shurikenClone.GetComponent<Clone>().showingRange)
                {
                    numberOfClonesShowingRange++;
                }
            }
            // if there is no clone showing range
            if (!showingRange && numberOfClonesShowingRange < 1)
            {
                selected = true;
            }
            // if there is one or more clone showing range and you want to change wich one it is
            else if (!showingRange && numberOfClonesShowingRange >= 1)
            {
                foreach (GameObject shurikenClone in GameObject.FindGameObjectsWithTag("Shuriken Clone"))
                {
                    shurikenClone.GetComponent<Clone>().selected = false;
                }
                selected = true;
            }
            // if you want to hide one clone's range that's showing
            else if (showingRange)
            {
                selected = false;
            }
        }
    }

    private void OnMouseExit()
    {
        gameController.GetComponent<CursorController>().SetCursorToDefault();
        hovered = false;
    }
}
