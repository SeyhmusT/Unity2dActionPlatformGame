using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager dialogueManagerInstance;
    private GatherInput gatherInput;

    [Header("Dialogue")]
    [SerializeField] private GameObject dialogueUI;
    [SerializeField] private Image speakerImage;
    [SerializeField] private TextMeshProUGUI speakerNameText;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private float typingSpeed = 0.03f;

    private DialogueObject currentDialogue;
    private int currentLineIndex;
    private bool isTyping;
    private Coroutine typingCoroutine;

    public void RegisterGatherInput(GatherInput gatherInputInstance)
    {
        gatherInput = gatherInputInstance;
    }


    private void Awake()
    {
        if (dialogueManagerInstance == null)
            dialogueManagerInstance = this;
        else
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    public void StartDialogue(DialogueObject dialogue)
    {
        currentDialogue = dialogue;
        currentLineIndex = 0;
        dialogueUI.SetActive(true);
        gatherInput.DialogueActive();
        ShowLine();
    }
    private void ShowLine()
    {
        DialogueLine line = currentDialogue.lines[currentLineIndex];
        speakerNameText.text = line.speakerName;
        speakerImage.sprite = line.speakerIcon;
        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);
        
        typingCoroutine = StartCoroutine(TypeLine(line.dialogueText));
    }

    private void NextLine()
    {
        currentLineIndex++;
        if (currentLineIndex >= currentDialogue.lines.Length)
        {
            EndDialogue();
        }
        else
        {
            ShowLine();
        }
    }

    public void ContinueDialog()
    {
        if (isTyping)
        {
            FinishTyping();
        }
        else
        {
            NextLine();
        }
    }

    private void FinishTyping()
    {
        StopCoroutine(typingCoroutine);
        dialogueText.text = currentDialogue.lines[currentLineIndex].dialogueText;
        isTyping = false;
    }

    private void EndDialogue()
    {
        dialogueUI.SetActive(false);
        currentDialogue = null;
        gatherInput.DialogueNotActive();
    }

    private IEnumerator TypeLine(string line)
    {
        isTyping = true;
        dialogueText.text = "";
        foreach (char c in line)
        {
            dialogueText.text += c;
            yield return new WaitForSeconds(typingSpeed);
        }
        isTyping = false;
    }
}
