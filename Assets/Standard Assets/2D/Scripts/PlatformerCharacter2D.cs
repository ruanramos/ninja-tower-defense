using System;
using UnityEngine;

public class PlatformerCharacter2D : MonoBehaviour
{
    [SerializeField] private float m_MaxSpeed = 10f;                    // The fastest the player can travel in the x axis.
    private bool m_Grounded;            // Whether or not the player is grounded.
    private Rigidbody2D m_Rigidbody2D;

    private void Awake()
    {
        // Setting up references.
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
    }


    private void FixedUpdate()
    {
        
    }


    public void Move(float move_horizontal, float move_vertical)
    {
        // Move the character
        m_Rigidbody2D.velocity = new Vector2(move_horizontal*m_MaxSpeed, move_vertical * m_MaxSpeed);
    }
}

