using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {

    /* Kuji-kiri is an esoteric practice which, when performed with an array of hand "seals" (kuji-in),
     * was meant to allow the ninja to enact superhuman feats.
     * The application of kuji to produce a desired effect was called "cutting" (kiri) the kuji.
     * Intended effects range from physical and mental concentration, to more incredible claims about rendering an
     * opponent immobile, or even the casting of magical spells.[108] These legends were captured in popular culture,
     * which interpreted the kuji-kiri as a precursor to magical acts.
     */

    public int kujiKiri; // kind of the magical power the player has
    public int cloneCost = 30;
    private GameObject kujiKiriQuantityText;
    private GameObject lifesText;

    public int totalLifes;
    public int currentLifes;

    // special energy bar for player skills usage
    public float specialEnergyQuantity;
    public float maxSpecialEnergyQuantity;
    public float specialEnergyQuantityCharged;
    public GameObject specialEnergyQuantityImage;
    public GameObject specialEnergyChargeImage;

    public GameObject player;

    // Use this for initialization
    void Start () {
        kujiKiri = 100;
        kujiKiriQuantityText = GameObject.Find("Kuji Kiri Quantity");
        lifesText = GameObject.Find("Lifes");
        totalLifes = 20;
        currentLifes = totalLifes;
        specialEnergyQuantityCharged = 0;

        player = GameObject.Find("Player");
    }
	
	// Update is called once per frame
	void Update () {
        // get the charge of the energy
        if (Input.GetMouseButton(0))
        {
            if (specialEnergyQuantityCharged < specialEnergyQuantity)
            {
                specialEnergyQuantityCharged += Time.deltaTime * 50;
            }
        }
        else if (!player.GetComponent<PlayerInput>().megaShurikenWasShoot && specialEnergyQuantityCharged < maxSpecialEnergyQuantity)
        {
            specialEnergyQuantityCharged = 0;
        }

        if (specialEnergyQuantity < maxSpecialEnergyQuantity)
        {
            specialEnergyQuantity += Time.deltaTime * 5;
        }

        kujiKiriQuantityText.GetComponent<Text>().text = "Kuji Kiri: " + kujiKiri;
        lifesText.GetComponent<Text>().text = "Lifes: " + currentLifes + " / " + totalLifes;
        specialEnergyQuantityImage.GetComponent<Image>().fillAmount = specialEnergyQuantity / maxSpecialEnergyQuantity;
        specialEnergyChargeImage.GetComponent<Image>().fillAmount = specialEnergyQuantityCharged / maxSpecialEnergyQuantity;

        // Show what weapon is equipped
        if(player.GetComponent<PlayerInput>().currentWeapon == "shuriken")
        {
            GameObject.Find("ShurikenImage").GetComponent<Image>().enabled = true;
        }
        else
        {
            GameObject.Find("ShurikenImage").GetComponent<Image>().enabled = false;
        }
        if (player.GetComponent<PlayerInput>().currentWeapon == "sword")
        {
            GameObject.Find("SwordImage").GetComponent<Image>().enabled = true;
        }
        else
        {
            GameObject.Find("SwordImage").GetComponent<Image>().enabled = false;
        }

        if (currentLifes <= 0)
        {
            SceneManager.LoadScene("GameOver");
        }
    }
}
