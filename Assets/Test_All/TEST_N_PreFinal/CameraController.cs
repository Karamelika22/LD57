using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class CameraController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float movementSpeed = 5f;
    public float screenEdgeMargin = 25f;

    [Header("Zoom Settings")]
    public float minZoom = 2f;
    public float maxZoom = 6f;

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
    public string Tag;
    private PlayerSettings settings;
    bool iss;
    void Start()
    {
        mainCamera = GetComponent<Camera>();
        GameObject settObject = GameObject.FindWithTag(Tag);
        if (settObject != null)
        {
            if (!settObject.TryGetComponent(out settings))
                Debug.LogError($"Объект с тегом {Tag} не имеет компонента Image!");
        }
        else
        {
            Debug.Log($"Не найден объект с тегом {Tag}!");
        }
        if (settings != null && settings.isHandleMovement) iss= true;
    }
    
    void Update()
    {
        if(iss) HandleMovement();
        HandleScrollMove();
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

    void HandleScrollMove()
    {
        float scrollData = Input.GetAxis("Mouse ScrollWheel");

        if (scrollData != 0)
        {
            Vector3 move = new(0, scrollData * movementSpeed, 0);
            transform.Translate(move, Space.World);
        }
    }

    void ClampCameraPosition()
    {
        Vector3 clampedPosition = transform.position;
        clampedPosition.x = Mathf.Clamp(clampedPosition.x, minXPosition, maxXPosition);
        clampedPosition.y = Mathf.Clamp(clampedPosition.y, minYPosition, maxYPosition);
        transform.position = clampedPosition;
    }
    public void HandleZoom(float newSize)
    {
        mainCamera.orthographicSize = Mathf.Clamp(newSize, minZoom, maxZoom);
    }
}
