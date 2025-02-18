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
    private bool invulnerable = false; // Indica si el jugador es invulnerable
    bool isWallSliding = false;

    Rigidbody2D rb;
    Animator animacion;

    // imagenes de la vida del jugador en HUD
    public GameObject[] vidaHUD;

    // Sonidos
    public AudioClip sonidoDisparo;
    public AudioClip sonidoDaño;
    public AudioClip sonidoMuerte;

    [SerializeField] private LayerMask groundLayer;    // Capa para el suelo
    [SerializeField] private Transform feetTransform;  // Punto para detectar el suelo
    [SerializeField] private Transform wallCheck;      // Punto para detectar paredes
    [SerializeField] private float wallSlideSpeed = 5f;  // Velocidad al deslizarse por la pared

    // Variables nuevas
    public int vida = 5;  // Vida del jugador
    public float tiempoCongelado = 2f;  // Tiempo que se congela el juego al llegar a 0 de vida

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animacion = GetComponent<Animator>();

        // desocultar el gameobject de la vida del jugador de la posicion 5
        vidaHUD[5].SetActive(true);
        vidaHUD[4].SetActive(false);
        vidaHUD[3].SetActive(false);
        vidaHUD[2].SetActive(false);
        vidaHUD[1].SetActive(false);
        vidaHUD[0].SetActive(false);

        animacion = GetComponent<Animator>();

        Time.timeScale = 1;
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
            animacion.SetBool("isJumping", true);
        }
        else
        {
            animacion.SetBool("isJumping", false);
        }
    }
    private bool IsTouchingWall()
    {
        return Physics2D.OverlapCircle(wallCheck.position, 0.2f, groundLayer);
    }

    private void WallSlide()
    {
        if (IsTouchingWall() && !isGrounded)
        {
            isWallSliding = true;
            rb.velocity = new Vector2(rb.velocity.x, -wallSlideSpeed);
        }
        else
        {
            isWallSliding = false;
        }
    }

    private void FixedUpdate()
    {
        // Movimiento horizontal
        rb.velocity = new Vector2(HorizontalInput * moveSpeed, rb.velocity.y);

        animacion.SetFloat("xVelocity", Mathf.Abs(rb.velocity.x));
        animacion.SetFloat("yVelocity", rb.velocity.y);

        WallSlide();
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
    public void RecibirDaño()
    {
        if (!invulnerable)
        {
            vida -= 1;
        }

        // Actualizar HUD
        for (int i = 5; i >= 0; i--)
        {
            vidaHUD[i].SetActive(i == vida);
        }

        if (vida > 0 && !invulnerable)
        {
            // Reproducir sonido de daño
            AudioSource.PlayClipAtPoint(sonidoDaño, transform.position);
            //Tiempo de invulnerabilidad despues de recibir daño
            StartCoroutine(Invulnerable());
        }

        // Si la vida llega a 0
        if (vida == 0)
        {
            vidaHUD[0].SetActive(true);
            // Reproducir sonido de muerte
            AudioSource.PlayClipAtPoint(sonidoMuerte, transform.position);
            //esperar 2 segundos antes de cambiar de escena
            StartCoroutine(EsperarGameOver());
        }
    }

    private void DañoAgua()
    {
        
        vida = 0;
        vidaHUD[0].SetActive(true);

        AudioSource.PlayClipAtPoint(sonidoMuerte, transform.position);
        StartCoroutine(EsperarGameOver());
    }

    IEnumerator Invulnerable()
    {

        invulnerable = true; // El jugador es invulnerable
        // cambiar el color del jugador al recibir daño
        GetComponent<SpriteRenderer>().color = Color.red;

        // Esperar 2 segundos
        yield return new WaitForSeconds(1);
        GetComponent<SpriteRenderer>().color = Color.white;
        invulnerable = false; // El jugador ya no es invulnerable

    }

    IEnumerator EsperarGameOver()
    {
        Time.timeScale = 0f;
        yield return new WaitForSecondsRealtime(2); // Espera 2 segundos
        SceneManager.LoadScene("GameOver");// Después de esperar, carga la escena de Game Over
        Time.timeScale = 1;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Water"))
        {
            DañoAgua();
        }
        if (collision.gameObject.CompareTag("Goal"))
        {
            //cargar la escena de victoria
            SceneManager.LoadScene("Win");
        }
    }


    // Coroutine para manejar el Game Over
    IEnumerator GameOver()
    {
        Time.timeScale = 0f;  // Congela el tiempo del juego

        // Esperar 2 segundos
        yield return new WaitForSecondsRealtime(tiempoCongelado);  // Usa WaitForSecondsRealtime para no afectar el tiempo congelado

        // Cambiar la escena a "Game Over"
        SceneManager.LoadScene("GameOver");  // Asegúrate de que tienes una escena llamada "GameOver"

        // Reactivar el tiempo
        Time.timeScale = 1f;
    }
}
