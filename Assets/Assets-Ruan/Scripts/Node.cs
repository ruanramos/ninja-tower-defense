using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour {

    private bool hasClone;
    public GameObject prefabClone;
    private GameObject player;
    private GameObject gameController;

    private void Start()
    {
        hasClone = false;
        player = GameObject.FindGameObjectWithTag("player");
        gameController = GameObject.Find("GameController");
    }

    private void OnMouseOver()
    {
        float dist = DistanceToPlayer(player);
        gameController.GetComponent<CursorController>().SetCursorToHover();

        // Visual feedback for the player if he can or cannot create a clone on the node
        if (dist > PlayerController.buildingRange && !hasClone)
        {
            this.GetComponent<SpriteRenderer>().color = Color.red;
        }
        else if (dist <= PlayerController.buildingRange && !hasClone && gameController.GetComponent<GameController>().cloneCost <= gameController.GetComponent<GameController>().kujiKiri)
        {
            this.GetComponent<SpriteRenderer>().color = new Color(0/255f, 191/255f, 255/255f);
        }
        else if (dist <= PlayerController.buildingRange && !hasClone && gameController.GetComponent<GameController>().cloneCost > gameController.GetComponent<GameController>().kujiKiri)
        {
            this.GetComponent<SpriteRenderer>().color = Color.red;
        }
        else if (hasClone)
        {
            this.GetComponent<SpriteRenderer>().color = Color.white;
        }

        if (Input.GetMouseButtonDown(0) && !hasClone && dist <= PlayerController.buildingRange && gameController.GetComponent<GameController>().cloneCost <= gameController.GetComponent<GameController>().kujiKiri)
        {
            hasClone = true;
            gameController.GetComponent<GameController>().kujiKiri -= gameController.GetComponent<GameController>().cloneCost;
            GameObject clone = Instantiate(prefabClone, transform.position, transform.rotation);
        }
    }

    private void OnMouseExit()
    {
        this.GetComponent<SpriteRenderer>().color = Color.white;
        gameController.GetComponent<CursorController>().SetCursorToDefault();
    }

    private float DistanceToPlayer(GameObject player)
    {
        return Vector2.Distance(transform.position, player.transform.position);
    }

}
