using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float horizontal_MaxSpeed = 10f;                    // The fastest the player can travel in the x axis.
    [SerializeField] private float vertical_MaxSpeed = 10f;                    // The fastest the player can travel in the y axis.
    private bool m_Grounded;            // Whether or not the player is grounded.
    private Rigidbody2D m_Rigidbody2D;

    public static float buildingRange = 2f;

    private void Awake()
    {
        // Setting up references.
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
    }

    public void Move(float move_horizontal, float move_vertical)
    {
        // Move the character
        m_Rigidbody2D.velocity = new Vector2(move_horizontal * horizontal_MaxSpeed, move_vertical * vertical_MaxSpeed);

        Vector3 mousePos = Input.mousePosition;
        mousePos = Camera.main.ScreenToWorldPoint(mousePos);
        mousePos.z = 0;
        // Get Angle in Radians
        float AngleRad = Mathf.Atan2(mousePos.y - transform.position.y, mousePos.x - transform.position.x);
        // Get Angle in Degrees
        float AngleDeg = (180 / Mathf.PI) * AngleRad;
        // Rotate Object
        this.transform.rotation = Quaternion.Euler(0, 0, AngleDeg - 90);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        m_Rigidbody2D.isKinematic = true;
        m_Rigidbody2D.velocity = Vector3.zero;
        m_Rigidbody2D.angularVelocity = 0;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        m_Rigidbody2D.isKinematic = false;
    }
}

