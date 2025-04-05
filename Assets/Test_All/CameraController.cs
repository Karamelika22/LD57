using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float movementSpeed = 5f;
    public float screenEdgeMargin = 50f;

    [Header("Zoom Settings")]
    public float zoomSpeed = 5f;
    public float minZoom = 2f;
    public float maxZoom = 5f;

    [Space(10)]
    
    [Header("Horizontal Limits")]
    [Tooltip("'Limits min' only with '-' or 0")]
    public float minXPosition = 0f;
    [Tooltip("'Limits max' only with '+' or 0")]
    public float maxXPosition = 0f;

    [Header("Vertical Limits")]
    [Tooltip("'Limits min' only with '-' or 0")]
    public float minYPosition = -5f;
    [Tooltip("'Limits max' only with '+' or 0")]
    public float maxYPosition = 5f;
    
    private Camera mainCamera;

    void Start()
    {
        mainCamera = GetComponent<Camera>();
    }

    void Update()
    {
        HandleMovement();
        HandleZoom();
        ClampCameraPosition();
    }

    void HandleMovement()
    {
        Vector3 mousePosition = Input.mousePosition;
        Vector3 moveDirection = Vector3.zero;

        // Проверка границ экрана
        if (mousePosition.x < screenEdgeMargin)
            moveDirection.x -= 1;
        if (mousePosition.x > Screen.width - screenEdgeMargin)
            moveDirection.x += 1;
        if (mousePosition.y < screenEdgeMargin)
            moveDirection.y -= 1;
        if (mousePosition.y > Screen.height - screenEdgeMargin)
            moveDirection.y += 1;

        // Нормализация и перемещение
        moveDirection.Normalize();
        transform.Translate(movementSpeed * Time.deltaTime * moveDirection, Space.World);
    }

    void HandleZoom()
    {
        float scrollData = Input.GetAxis("Mouse ScrollWheel");
        float newSize = mainCamera.orthographicSize - scrollData * zoomSpeed;
        mainCamera.orthographicSize = Mathf.Clamp(newSize, minZoom, maxZoom);
    }

    void ClampCameraPosition()
    {
        Vector3 clampedPosition = transform.position;
        clampedPosition.x = Mathf.Clamp(clampedPosition.x, minXPosition, maxXPosition);
        clampedPosition.y = Mathf.Clamp(clampedPosition.y, minYPosition, maxYPosition);
        transform.position = clampedPosition;
    }
}
