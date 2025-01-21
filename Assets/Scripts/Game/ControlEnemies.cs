using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlEnemies : MonoBehaviour
{
    public float velocidad = 1f; // Velocidad del enemigo
    public float rangoDeteccion = 5f; // Rango dentro del cual el enemigo persigue al jugador

    private Rigidbody2D enemyRigidBody; // Referencia al Rigidbody2D del enemigo
    private Animator animacion; // Referencia al Animator del enemigo
    private Transform jugador; // Referencia al transform del jugador

    void Start()
    {
        enemyRigidBody = GetComponent<Rigidbody2D>();
        animacion = GetComponent<Animator>();

        // Encuentra al jugador una vez al iniciar
        GameObject player = GameObject.Find("Player");
        if (player != null)
        {
            jugador = player.transform;
        }
        else
        {
            Debug.LogError("No se encontró un objeto llamado 'Player' en la escena.");
        }
    }

    void FixedUpdate()
    {
        if (jugador == null) return; // Si no hay jugador, no hacer nada

        float distanciaAlJugador = Vector2.Distance(transform.position, jugador.position);

        if (distanciaAlJugador <= rangoDeteccion)
        {
            MovimientoEnemigo();
        }
        else
        {
            // Detener movimiento y animación si está fuera del rango
            enemyRigidBody.velocity = new Vector2(0, enemyRigidBody.velocity.y); // Solo detener en X
            animacion.SetBool("isWalking", false);
            animacion.SetBool("isIdle", true);
        }
    }

    private void MovimientoEnemigo()
    {
        // Calcular dirección hacia el jugador
        Vector2 direccion = (jugador.position - transform.position).normalized;

        // Solo moverse en el eje X
        enemyRigidBody.velocity = new Vector2(direccion.x * velocidad, enemyRigidBody.velocity.y);

        // Manejar animaciones
        animacion.SetBool("isWalking", true);
        animacion.SetBool("isIdle", false);

        // Opcional: Ajustar la dirección de la animación para que el enemigo mire hacia el jugador
        if (direccion.x > 0)
            transform.localScale = new Vector2(-1, 1); // Mirar hacia la derecha
        else if (direccion.x < 0)
            transform.localScale = new Vector2(1, 1); // Mirar hacia la izquierda
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Rigidbody2D jugadorRB = collision.gameObject.GetComponent<Rigidbody2D>();

            // Verificar si el jugador está cayendo desde arriba
            if (jugadorRB != null && jugadorRB.velocity.y < 0) // Si el jugador está cayendo
            {
                // Activar la animación de muerte del enemigo
                Animator enemyAnimator = GetComponent<Animator>();
                if (enemyAnimator != null)
                {
                    enemyAnimator.SetBool("isDead", true);
                }

                // Destruir al enemigo después de un pequeño retraso (si tienes una animación)
                Destroy(gameObject, 0.5f);

                // Aplicar un rebote al jugador
                float fuerzaRebote = 7f; // Ajusta la fuerza de rebote
                jugadorRB.velocity = new Vector2(jugadorRB.velocity.x, fuerzaRebote);
            }
            else
            {
                // Si el jugador no está cayendo, aplicar daño al jugador
                PlayerMovement playerScript = collision.gameObject.GetComponent<PlayerMovement>();
                if (playerScript != null)
                {
                    playerScript.RecibirDaño();
                }
            }
        }
    }

}
