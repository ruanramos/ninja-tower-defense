using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Marker : MonoBehaviour {

    GameObject gameController;


    private void Start()
    {
        gameController = GameObject.Find("GameController");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            gameController.GetComponent<GameController>().currentLifes--;
            Destroy(collision.gameObject);
        }
    }
}
