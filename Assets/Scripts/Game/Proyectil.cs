using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Proyectil : MonoBehaviour
{
    public float velocidad = 10f; // Velocidad del proyectil
    private Vector2 direccion; // Dirección del proyectil
    private Animator animacion; // Referencia al Animator del proyectil
    public AudioClip sonidoKill; // Sonido al matar al enemigo

    void Start()
    {
        animacion = GetComponent<Animator>();
    }
    // Configurar la dirección hacia donde viajará el proyectil
    public void ConfigurarDireccion(Vector2 direccionDisparo)
    {
        direccion = direccionDisparo.normalized; // Normalizar para que la magnitud sea 1
    }

    void Update()
    {
        // Mover el proyectil en la dirección configurada
        transform.Translate(direccion * velocidad * Time.deltaTime);
        Destroy(gameObject, 1f);

    }

    // Funcion para esperar 1 segundo antes de destruir el proyectil
    IEnumerator EsperaDestruirProyectil()
    {
        yield return new WaitForSeconds(1f);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Si colisiona con un enemigo
        if (other.CompareTag("Enemy"))
        {
            // Reproducir sonido de sonidoKill
            AudioSource.PlayClipAtPoint(sonidoKill, transform.position);
            // Destruir el proyectil y el enemigo colisionado
            Destroy(gameObject);
            //cambiar color del enemigo a rojo
            other.GetComponent<SpriteRenderer>().color = Color.red;
            Destroy(other.gameObject);
        }
        else if (other.CompareTag("Pared"))
        {
            // Destruir el proyectil si colisiona con una pared
            Destroy(gameObject);
        }
    }
}
