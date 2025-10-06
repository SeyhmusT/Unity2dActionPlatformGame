using UnityEngine;
using UnityEngine.InputSystem;

public class MinimapController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Camera minimapCamera;
    [Header("Move Settings")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float dragSpeed;

    [Header("Zoom Settings")]
    [SerializeField] private float zoomSpeed;
    [SerializeField] private float minZoom;
    [SerializeField] private float maxZoom;
    private PlayerInput playerInput;


    public InputActionReference moveActionRef;
    public InputActionReference zoomActionRef;
    public InputActionReference middleClickRef;
    public InputActionReference deltaActionRef;



    private void Awake() // ADD
    {
        playerInput = FindAnyObjectByType<PlayerInput>();
    }
    
    void Update()
    {
        if (playerInput == null || playerInput.currentActionMap == null ||
            playerInput.currentActionMap.name != "MinimapControls")
        {
            if (playerInput != null && playerInput.currentActionMap != null)
            {
                Debug.Log($"[MinimapController] Aktif ActionMap: {playerInput.currentActionMap.name} - MinimapControls bekleniyor");
            }
            return;
        }
            
        HandleZoom();
        HandleMove();
    }

    void OnEnable()
    {
        Debug.Log("[MinimapController] OnEnable - ActionRef'leri enable ediliyor");
        // ActionRef'leri enable et
        if (moveActionRef != null && moveActionRef.action != null)
        {
            moveActionRef.action.Enable();
            Debug.Log("[MinimapController] MoveActionRef enabled");
        }
        if (zoomActionRef != null && zoomActionRef.action != null)
        {
            zoomActionRef.action.Enable();
            Debug.Log("[MinimapController] ZoomActionRef enabled");
        }
        if (middleClickRef != null && middleClickRef.action != null)
        {
            middleClickRef.action.Enable();
            Debug.Log("[MinimapController] MiddleClickRef enabled");
        }
        if (deltaActionRef != null && deltaActionRef.action != null)
        {
            deltaActionRef.action.Enable();
            Debug.Log("[MinimapController] DeltaActionRef enabled");
        }
    }
    
    void OnDisable()
    {
        Debug.Log("[MinimapController] OnDisable - ActionRef'leri disable ediliyor");
        // ActionRef'leri disable et
        if (moveActionRef != null && moveActionRef.action != null)
            moveActionRef.action.Disable();
        if (zoomActionRef != null && zoomActionRef.action != null)
            zoomActionRef.action.Disable();
        if (middleClickRef != null && middleClickRef.action != null)
            middleClickRef.action.Disable();
        if (deltaActionRef != null && deltaActionRef.action != null)
            deltaActionRef.action.Disable();
    }

    private void HandleZoom()
    {
        if (zoomActionRef == null || zoomActionRef.action == null) return;
        
        float scroll = zoomActionRef.action.ReadValue<float>();
        if (scroll != 0)
        {
            Debug.Log($"[MinimapController] Zoom input: {scroll}");
            minimapCamera.orthographicSize = Mathf.Clamp(minimapCamera.orthographicSize - scroll * zoomSpeed, minZoom, maxZoom);
        }
    }

    private void HandleMove()
    {
        if (moveActionRef == null || moveActionRef.action == null) return;
        
        float zoomFactor = minimapCamera.orthographicSize;
        Vector2 moveInput = moveActionRef.action.ReadValue<Vector2>();
        if (moveInput != Vector2.zero)
        {
            Debug.Log($"[MinimapController] Move input: {moveInput}");
            Vector3 move = new Vector3(moveInput.x, moveInput.y, 0) * zoomFactor * moveSpeed * Time.deltaTime;
            minimapCamera.transform.position += move;
        }
        
        if (middleClickRef != null && middleClickRef.action != null && middleClickRef.action.IsPressed())
        {
            if (deltaActionRef != null && deltaActionRef.action != null)
            {
                Vector2 delta = deltaActionRef.action.ReadValue<Vector2>();
                if (delta != Vector2.zero)
                {
                    Debug.Log($"[MinimapController] Middle click drag: {delta}");
                    Vector3 dragMove = new Vector3(-delta.x, -delta.y, 0) * zoomFactor * dragSpeed * Time.deltaTime;
                    minimapCamera.transform.position += dragMove;
                }
            }
        }
    }
}
