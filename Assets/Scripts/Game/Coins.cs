using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Coins : MonoBehaviour
{
    private int valor = 1; // Valor de la moneda
    public AudioClip sonido; // Sonido al recoger la moneda

    // Texto que se actualiza en la interfaz de usuario (UI)
    private TMPro.TextMeshProUGUI textoPuntaje;

    private Animator animacion; // Animator de la moneda
    private static int puntaje = 0; // Puntaje global estático (para acumular el puntaje a través de monedas)

    private bool monedaRecogida = false; // Controla si la moneda ya ha sido recogida

    void Start()
    {
        // Reiniciar el puntaje al cargar una nueva escena
        puntaje = 0;

        // Actualizar el texto del puntaje en la UI
        textoPuntaje = GameObject.FindWithTag("CoinsText").GetComponent<TMPro.TextMeshProUGUI>();
        textoPuntaje.text = puntaje.ToString();

        animacion = GetComponent<Animator>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Verificar si la moneda ya ha sido recogida para no activar el trigger nuevamente
        if (monedaRecogida) return;

        // Si el objeto que colisiona tiene la etiqueta "Player"
        if (other.CompareTag("Player"))
        {
            // Aumentar el puntaje global
            puntaje += valor;

            // Actualizar el texto en la interfaz de usuario
            textoPuntaje.text = puntaje.ToString();
            // Reproducir sonido
            AudioSource.PlayClipAtPoint(sonido, transform.position);

            // Activar la animación de la moneda al ser recogida
            animacion.SetTrigger("Collected");
            // Marcar que la moneda ya ha sido recogida
            monedaRecogida = true;

            // Llamar a la coroutine para destruir la moneda después de la animación
            StartCoroutine(DestruirMonedaDespuesDeAnimacion());
        }

        // Recoger todas las monedas disponibles en una variable
        GameObject[] monedas = GameObject.FindGameObjectsWithTag("Coin");

        // Si no quedan monedas en la escena
        if (monedas.Length == 1)
        {
            // Cargar la escena de victoria despues de 1 segundo
            StartCoroutine(CargarEscenaDespuesDeTiempo(1f, "Win"));

        }
    }

    // Coroutine que carga una escena después de un tiempo determinado
    IEnumerator CargarEscenaDespuesDeTiempo(float tiempo, string escena)
    {
        // Esperar el tiempo indicado
        yield return new WaitForSeconds(tiempo);

        // Cargar la escena indicada
        SceneManager.LoadScene(escena);
    }

    // Coroutine que espera que termine la animación antes de destruir la moneda
    IEnumerator DestruirMonedaDespuesDeAnimacion()
    {
        // Esperar a que termine la animación (ajustar el tiempo según la duración de la animación)
        yield return new WaitForSeconds(animacion.GetCurrentAnimatorStateInfo(0).length);

        // Destruir la moneda después de la animación
        Destroy(gameObject);
    }
}
