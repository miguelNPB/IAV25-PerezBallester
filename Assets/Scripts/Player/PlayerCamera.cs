using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [Header("Configuración")]
    [SerializeField] private float movementSpeed = 2f;       // Velocidad de arrastre
    [SerializeField] private float rotationSpeed = 2f;       // Velocidad de rotacion
    [SerializeField] private float minZoom = 5f;         // Zoom mínimo
    [SerializeField] private float maxZoom = 20f;        // Zoom máximo
    [SerializeField] private float zoomSpeed = 5f;       // Velocidad del zoom

    private Camera cam;
    private Vector3 lastMousePosition;

    private void Awake()
    {
        cam = Camera.main;
    }

    private void Update()
    {
        HandleMoveCamera();
        HandleRotateCamera();
        HandleZoom();
    }

    private void HandleMoveCamera()
    {
        Vector3 direction = Vector3.zero;
        if (Input.GetKey(KeyCode.W))
        {
            direction += transform.forward;
        } 
        else if (Input.GetKey(KeyCode.S))
        {
            direction -= transform.forward;
        }

        if (Input.GetKey(KeyCode.A))
        {
            direction -= transform.right;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            direction += transform.right;
        }

        transform.position += (direction * movementSpeed * Time.deltaTime);
    }

    private void HandleRotateCamera()
    {
        
        if (Input.GetMouseButtonDown(1))
        {
            lastMousePosition = Input.mousePosition;
        } 

        if (Input.GetMouseButton(1))
        {
            Vector3 delta = Input.mousePosition - lastMousePosition;

            float yaw = delta.x * rotationSpeed * Time.deltaTime;
            float pitch = -delta.y * rotationSpeed * Time.deltaTime;

            transform.Rotate(Vector3.up, yaw, Space.World); // horizontal
            //transform.Rotate(Vector3.right, pitch, Space.Self); // vertical

            lastMousePosition = Input.mousePosition;
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
