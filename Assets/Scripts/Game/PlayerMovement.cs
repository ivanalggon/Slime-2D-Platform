using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    float HorizontalInput;
    float moveSpeed = 5f;
    bool isFacingRight = true;
    float jumpForce = 5f;
    bool isGrounded = false;

    Rigidbody2D rb;
    Animator animator;

    [SerializeField] private LayerMask groundLayer;    // Capa para el suelo
    [SerializeField] private Transform feetTransform;  // Punto para detectar el suelo

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        HorizontalInput = Input.GetAxis("Horizontal");
        FlipSprite();

        // Detectar si está tocando el suelo
        isGrounded = CheckIfGrounded();

        // Movimiento y salto
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);  // Solo cambia la componente Y para saltar
            isGrounded = false;  // Ya no está en el suelo
            animator.SetBool("isJumping", true);
        }
    }

    private void FixedUpdate()
    {
        // Movimiento horizontal
        rb.velocity = new Vector2(HorizontalInput * moveSpeed, rb.velocity.y);

        animator.SetFloat("xVelocity", Mathf.Abs(rb.velocity.x));
        animator.SetFloat("yVelocity", rb.velocity.y);
    }

    void FlipSprite()
    {
        if (isFacingRight && HorizontalInput < 0f || !isFacingRight && HorizontalInput > 0f)
        {
            isFacingRight = !isFacingRight;
            Vector3 ls = transform.localScale;
            ls.x *= -1;
            transform.localScale = ls;
        }
    }

    // Detectar si está en el suelo usando OverlapCircle
    private bool CheckIfGrounded()
    {
        // Usamos un OverlapCircle para detectar colisiones con el suelo
        Collider2D hit = Physics2D.OverlapCircle(feetTransform.position, 0.1f, groundLayer);
        return hit != null;  // Si toca algo en la capa "GroundLayer", devuelve verdadero
    }
}