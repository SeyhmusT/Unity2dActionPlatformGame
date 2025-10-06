using UnityEngine;

public class Trader : MonoBehaviour, IInteractable
{
    [SerializeField] private DialogueObject traderDialogue;
    public void CustomInteract()
    {
        //Debug.Log("You interact with TRADER");
        DialogueManager.dialogueManagerInstance.StartDialogue(traderDialogue);
    }
}
