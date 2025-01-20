using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;  // Necesario para cargar escenas

public class PlayerMovement : MonoBehaviour
{
    // Variables existentes
    float HorizontalInput;
    float moveSpeed = 5f;
    bool isFacingRight = true;
    float jumpForce = 10f;
    bool isGrounded = false;

    Rigidbody2D rb;
    Animator animator;

    [SerializeField] private LayerMask groundLayer;    // Capa para el suelo
    [SerializeField] private Transform feetTransform;  // Punto para detectar el suelo

    // Variables nuevas
    public int vida = 5;  // Vida del jugador
    public float tiempoCongelado = 2f;  // Tiempo que se congela el juego al llegar a 0 de vida
    private bool estaCongelado = false;  // Para evitar que se ejecute más de una vez el congelado

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        // Movimiento horizontal
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

    // Detectar colisión con el enemigo
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && !estaCongelado)
        {
            // Restar 1 de vida al jugador
            vida--;

            // Si la vida llega a 0
            if (vida <= 0)
            {
                // Congelar el juego por 2 segundos y cargar Game Over
                StartCoroutine(GameOver());
            }
        }
    }

    // Coroutine para manejar el Game Over
    IEnumerator GameOver()
    {
        // Congelar el juego (pausar todo)
        estaCongelado = true;
        Time.timeScale = 0f;  // Congela el tiempo del juego

        // Esperar 2 segundos
        yield return new WaitForSecondsRealtime(tiempoCongelado);  // Usa WaitForSecondsRealtime para no afectar el tiempo congelado

        // Cambiar la escena a "Game Over"
        SceneManager.LoadScene("GameOver");  // Asegúrate de que tienes una escena llamada "GameOver"

        // Reactivar el tiempo
        Time.timeScale = 1f;
    }
}
