using System;
using System.Collections;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

[RequireComponent(typeof(PlayerController))]
public class PlayerInput : MonoBehaviour
{
    private PlayerController m_Character;
    public GameObject prefabShuriken;
    [SerializeField]
    public GameObject prefabMegaShuriken;

    public float shootingForce;
    private float shurikenShootTime;
    public float shurikenShootCooldown;

    public float meleeRange;
    public float meleeRangeCooldown;
    private float meleeHitTime;
    public float meleeRangeAnimationSpeed;
    public string currentWeapon;

    private GameObject gameController;

    public bool megaShurikenWasShoot = false;

    private void Awake()
    {
        m_Character = GetComponent<PlayerController>();
        gameController = GameObject.Find("GameController");
        currentWeapon = "shuriken";

    }

    private void Update()
    {
        meleeHitTime += Time.deltaTime;
        shurikenShootTime += Time.deltaTime;
        megaShurikenWasShoot = false;
        // shooting shuriken with left mouse button
        if (Input.GetMouseButtonDown(0) && gameController.GetComponent<CursorController>().cursorTypeDefault && currentWeapon == "shuriken")
        {
            //shoot shuriken
            if (shurikenShootTime >= shurikenShootCooldown)
            {
                Fire_Shuriken(prefabShuriken);
                shurikenShootTime = 0;
            }
        }
        // Sword atack if sword selected
        else if (Input.GetMouseButtonDown(0) && gameController.GetComponent<CursorController>().cursorTypeDefault && currentWeapon == "sword")
        {
            // Sword atacks only if not on cd
            if  (meleeHitTime >= meleeRangeCooldown)
            {
                StartCoroutine(StartSwordAtack(meleeRangeAnimationSpeed));
            }
        }
        // Mega shuriken checking, the most powerfull skill
        if (Input.GetMouseButtonUp(0))
        {
            if (gameController.GetComponent<GameController>().specialEnergyQuantityCharged >= gameController.GetComponent<GameController>().maxSpecialEnergyQuantity)
            {
                // shoot mega shuriken
                Fire_Mega_Shuriken(prefabMegaShuriken);
                gameController.GetComponent<GameController>().specialEnergyQuantityCharged = 0;
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            currentWeapon = "sword";
            meleeHitTime = meleeRangeCooldown;
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            currentWeapon = "shuriken";
            shurikenShootTime = shurikenShootCooldown;
        }
    }

    private void FixedUpdate()
    {
        // Read the inputs.
        float h = CrossPlatformInputManager.GetAxis("Horizontal");
        float v = CrossPlatformInputManager.GetAxis("Vertical");
        // Pass all parameters to the character control script.
        m_Character.Move(h, v);
    }

    private void Fire_Shuriken(GameObject prefabShuriken)
    {
        GameObject shuriken = Instantiate(prefabShuriken, transform.position, transform.rotation);
        
        Vector3 objPos = transform.position;
        Vector3 mousePos = Input.mousePosition;
        mousePos = Camera.main.ScreenToWorldPoint(mousePos);
        float mousePosX = mousePos.x - objPos.x;
        float mousePosY = mousePos.y - objPos.y;
        shuriken.GetComponent<Rigidbody2D>().velocity = ((new Vector2 (mousePosX, mousePosY).normalized) * shootingForce);
    }

    private void Fire_Mega_Shuriken(GameObject prefabMegaShuriken)
    {
        GameObject megaShuriken = Instantiate(prefabMegaShuriken, transform.position, transform.rotation);
        gameController.GetComponent<GameController>().specialEnergyQuantity -= 99;
        Vector3 objPos = transform.position;
        Vector3 mousePos = Input.mousePosition;
        mousePos = Camera.main.ScreenToWorldPoint(mousePos);
        float mousePosX = mousePos.x - objPos.x;
        float mousePosY = mousePos.y - objPos.y;
        megaShuriken.GetComponent<Rigidbody2D>().velocity = ((new Vector2(mousePosX, mousePosY).normalized) * shootingForce / 2);
        megaShurikenWasShoot = true;
    }

    IEnumerator StartSwordAtack(float meleeRangeAnimationSpeed)
    {
        transform.Find("MeleeAtack").GetComponent<Animator>().enabled = true;
        transform.Find("MeleeAtack").GetComponent<BoxCollider2D>().enabled = true;
        transform.Find("MeleeAtack").GetComponent<SpriteRenderer>().enabled = true;
        meleeHitTime = 0;
        yield return new WaitForSeconds(meleeRangeAnimationSpeed);
        transform.Find("MeleeAtack").GetComponent<BoxCollider2D>().enabled = false;
        transform.Find("MeleeAtack").GetComponent<SpriteRenderer>().enabled = false;
        transform.Find("MeleeAtack").GetComponent<Animator>().enabled = false;
    }


}

