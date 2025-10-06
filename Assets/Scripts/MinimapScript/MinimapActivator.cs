using UnityEngine;
using UnityEngine.InputSystem;

public class MinimapActivator : MonoBehaviour
{
    public InputActionReference minimapActivatorRef;
    private Player player;
    private bool mapActivated = false;
    [SerializeField] private CanvasGroup minimapCanvasGroup;

    void OnEnable()
    {
        minimapActivatorRef.action.performed += TryToOpenMinimap;
        minimapActivatorRef.action.Enable();

    }

    private void Awake()
{
    if (player == null) player = GetComponent<Player>();
    if (player == null) player = FindAnyObjectByType<Player>();

    if (player == null)
        Debug.LogError("[MinimapActivator] Player bulunamadı.");
    if (minimapCanvasGroup == null)
        Debug.LogError("[MinimapActivator] CanvasGroup atanmamış.");
}


    void OnDisable()
    {
        minimapActivatorRef.action.performed -= TryToOpenMinimap;
        minimapActivatorRef.action.Disable();
    }
    void Start()
    {
        
    }

    private void TryToOpenMinimap(InputAction.CallbackContext _)
    {
        if (player == null || minimapCanvasGroup == null) return;

        var stats = player.playerStats;
        var input = player.GetComponent<PlayerInput>();   // Player üstündeki PlayerInput
        if(input == null || input.currentActionMap == null) return;
        var currentMap = input.currentActionMap.name;

        if (stats == null || input == null) return;

       

        if (currentMap == "DialogueControl")
            return;

    if (!mapActivated && currentMap == "Player")
    {
        // Minimap AÇ
        mapActivated = true;
        minimapCanvasGroup.alpha = 1f;
        input.SwitchCurrentActionMap("MinimapControls");


        var activatorMap = input.actions.FindActionMap("Activators", true);
        activatorMap?.Enable();
        return;
    }

    if (mapActivated && currentMap == "MinimapControls")
    {
        // Minimap KAPAT
        mapActivated = false;
        minimapCanvasGroup.alpha = 0f;
        input.SwitchCurrentActionMap("Player");


        var activatorMap = input.actions.FindActionMap("Activators", true);
        activatorMap?.Enable();
        return;
        
    }
}

}
