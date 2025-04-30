using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [Header("Configuración")]
    [SerializeField] private float dragSpeed = 2f;       // Velocidad de arrastre
    [SerializeField] private float minZoom = 5f;         // Zoom mínimo
    [SerializeField] private float maxZoom = 20f;        // Zoom máximo
    [SerializeField] private float zoomSpeed = 5f;       // Velocidad del zoom

    private Vector3 dragOrigin;                          // Punto inicial del arrastre
    private Camera cam;                                  // Referencia a la cámara

    private void Awake()
    {
        cam = Camera.main;
        dragOrigin = Vector3.zero;
    }

    private void Update()
    {
        HandleDragMovement();
        HandleZoom();
    }

    // Maneja el movimiento al arrastrar con el botón derecho del ratón
    private void HandleDragMovement()
    {
        if (Input.GetMouseButtonDown(1)) // botón derecho presionado por primera vez
        {
            dragOrigin = Input.mousePosition;
        }

        if (Input.GetMouseButton(1)) // botón derecho mantenido
        {
            Vector3 difference = (dragOrigin - Input.mousePosition) * dragSpeed * Time.deltaTime;

            difference.z = difference.y;
            difference.y = 0;

            transform.position += difference;

            dragOrigin = Input.mousePosition; // actualizar para el siguiente frame
        }
    }

    // Maneja el zoom con la rueda del ratón
    private void HandleZoom()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");

        if (scroll != 0f)
        {
            float newSize = cam.fieldOfView - scroll * zoomSpeed;
            cam.fieldOfView = Mathf.Clamp(newSize, minZoom, maxZoom);
        }
    }
}
