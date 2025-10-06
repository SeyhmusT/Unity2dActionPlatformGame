using UnityEngine;
using UnityEngine.InputSystem;

public class GatherInput : MonoBehaviour
{
    public PlayerInput playerInput;
    private InputActionMap playerMap;
    private InputActionMap uiMap;
    private InputActionMap miniMap;
    private InputActionMap activatorMap;
    private InputActionMap dialogueMap;

    public InputActionReference jumpActionRef;
    public InputActionReference moveActionRef;
    public InputActionReference verticalActionRef;
    public InputActionReference dialougeActionRef;


    [HideInInspector]
    public float horizontalInput;

    [HideInInspector]
    public float verticalInput;

    private void OnEnable()
    {
        // PlayerInput'u Player map ile başlat
        playerInput.SwitchCurrentActionMap("Player");
        dialougeActionRef.action.performed += TryToContinueDialogue;

        // ActionRef'leri manuel olarak enable et
        if (jumpActionRef != null && jumpActionRef.action != null)
            jumpActionRef.action.Enable();
        if (moveActionRef != null && moveActionRef.action != null)
            moveActionRef.action.Enable();
        if (verticalActionRef != null && verticalActionRef.action != null)
            verticalActionRef.action.Enable();
        if (dialougeActionRef != null && dialougeActionRef.action != null)
            dialougeActionRef.action.Enable();

        // Activator map'i enable et (minimap toggle için)
        activatorMap = playerInput.actions.FindActionMap("Activators", true);
        if (activatorMap != null)
            activatorMap.Enable();

        // Asset kontrolü
        var piAsset = playerInput.actions;
        var moveAsset = moveActionRef?.action?.actionMap?.asset;
        var vertAsset = verticalActionRef?.action?.actionMap?.asset;
        var jumpAsset = jumpActionRef?.action?.actionMap?.asset;
        
        if (jumpAsset != piAsset)
            Debug.LogError("[GatherInput] jumpActionRef asset uyumsuz! PlayerInput ile aynı asset'i kullanın.");

        if (moveAsset != piAsset)
            Debug.LogError("[GatherInput] moveActionRef, PlayerInput'un Actions asset'iyle AYNI değil! " +
                           "Inspector'da moveActionRef'i PlayerInput'un kullandığı asset'ten seçin.");
        if (vertAsset != piAsset)
            Debug.LogError("[GatherInput] verticalActionRef, PlayerInput'un Actions asset'iyle AYNI değil!");
    }

    void OnDisable()
    {
        dialougeActionRef.action.performed -= TryToContinueDialogue;

        // ActionRef'leri disable et
        if (jumpActionRef != null && jumpActionRef.action != null)
            jumpActionRef.action.Disable();
        if (moveActionRef != null && moveActionRef.action != null)
            moveActionRef.action.Disable();
        if (verticalActionRef != null && verticalActionRef.action != null)
            verticalActionRef.action.Disable();
        if (dialougeActionRef != null && dialougeActionRef.action != null)
            dialougeActionRef.action.Disable();


        // Activator map'i disable et
        if (activatorMap != null)
            activatorMap.Disable();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerMap = playerInput.actions.FindActionMap("Player");
        uiMap = playerInput.actions.FindActionMap("UI");
        miniMap = playerInput.actions.FindActionMap("MinimapControls");
        activatorMap = playerInput.actions.FindActionMap("Activators");
        dialogueMap = playerInput.actions.FindActionMap("DialogueControl");

        DialogueManager.dialogueManagerInstance.RegisterGatherInput(this);
        if (miniMap != null)
            miniMap.Disable();
    }

    private void TryToContinueDialogue(InputAction.CallbackContext value)
    {
        DialogueManager.dialogueManagerInstance.ContinueDialog();
    }

    // Update is called once per frame
    void Update()
    {
        var cam = playerInput?.currentActionMap;
        if (cam != null && cam.name == "Player")
        {
            horizontalInput = moveActionRef.action.ReadValue<float>();
            verticalInput = verticalActionRef.action.ReadValue<float>();
        }
        else
        {
            horizontalInput = 0f;
            verticalInput = 0f;
        }
    }

    public void DisablePlayerMap()
    {
        playerMap.Disable();
    }

    public void EnablePlayerMap()
    {
        playerMap.Enable();
    }

    public void EnableMinimap()
    {
        miniMap.Enable();
    }

    public void DisableMinimap()
    {
        miniMap.Disable();
    }
    public void DialogueActive()
    {
        playerInput.SwitchCurrentActionMap("DialogueControl");
        activatorMap?.Disable();
    }
    public void DialogueNotActive()
    {
        playerInput.SwitchCurrentActionMap("Player");
        activatorMap?.Enable();
    }
}
