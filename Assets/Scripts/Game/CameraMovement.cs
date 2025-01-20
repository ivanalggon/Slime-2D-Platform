using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Transform player; // Referencia al jugador
    public Vector3 offset;   // Offset para ajustar la posición de la cámara respecto al jugador
    float smoothTime = 0.125f; // Tiempo de suavizado
    Vector3 velocity = Vector3.zero; // Velocidad de la cámara

    void LateUpdate()
    {
        if (player != null)
        {
            Vector3 desiredPosition = player.position + offset;
            transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, smoothTime);
        }
        // si la posicion y es menor que 0 la camara no se mueve en x ni en y
        if (transform.position.y < -2)
        {
            transform.position = new Vector3(transform.position.x, -2, transform.position.z);
        }
    }
}

