using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShurikenController : MonoBehaviour {

    private Rigidbody2D shurikenRigidbody;
    private float lifetime;
    private bool shouldRotate;

    void Start () {
        shurikenRigidbody = this.GetComponent<Rigidbody2D>();
        shouldRotate = true;
	}
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        lifetime += Time.deltaTime;
        if(lifetime >= 0.7f && this.GetComponent<SpriteRenderer>().enabled && (this.tag == "shuriken" || this.tag == "shuriken clone"))
        {
            Destroy(gameObject);
        }
        else if (this.tag == "Mega Shuriken" && lifetime >= 7f)
        {
            Destroy(gameObject);
        }

        // rotate sprite
        if (shouldRotate)
        {
            transform.Rotate(0, 0, 3600 * Time.deltaTime, Space.World);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.gameObject.tag == "Enemy" && (this.tag == "shuriken" || this.tag == "shuriken clone" ))
        {
            this.GetComponent<Collider2D>().enabled = false;

            collision.GetComponent<EnemyController>().TakeDamage(1);
            
            this.GetComponent<SpriteRenderer>().enabled = false;
            this.GetComponent<BoxCollider2D>().enabled = false;
            Destroy(gameObject, 1); // aumentar o valor do tempo para algo maior que o delay da corrotina se quiser que suma, se não menor
        }
        if (collision.gameObject.tag == "Enemy" && this.tag == "Mega Shuriken")
        {
            collision.GetComponent<EnemyController>().TakeDamage(5);
        }
        if(this.tag != "Mega Shuriken" && this.tag != "shuriken clone")
        {
            shurikenRigidbody.velocity = new Vector2(0, 0);
            shouldRotate = false;
        }
        
        if (collision.gameObject.tag == "Node" && this.tag != "Shuriken Clone")
        {
            this.GetComponent<Collider2D>().enabled = false;
            shurikenRigidbody.isKinematic = true;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy" && this.tag == "Mega Shuriken")
        {
            collision.GetComponent<EnemyController>().TakeDamage(5);
        }
    }
}
