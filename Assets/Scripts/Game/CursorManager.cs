using UnityEngine;

public class CursorManager : MonoBehaviour
{
    public Texture2D crosshair; // Imagen del crosshair
    public Vector2 hotSpot = Vector2.zero; // Punto de anclaje del cursor (normalmente el centro)
    public CursorMode cursorMode = CursorMode.Auto; // Modo del cursor (Auto para usar la configuración predeterminada del sistema)

    void Start()
    {
        // Cambia el cursor al crosshair
        Cursor.SetCursor(crosshair, hotSpot, cursorMode);
    }

    void OnApplicationFocus(bool hasFocus)
    {
        // Vuelve a aplicar el cursor si el jugador regresa al juego
        if (hasFocus)
        {
            Cursor.SetCursor(crosshair, hotSpot, cursorMode);
        }
    }
}
