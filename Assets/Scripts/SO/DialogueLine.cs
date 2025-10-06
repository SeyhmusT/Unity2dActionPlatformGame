using UnityEngine;

[CreateAssetMenu(menuName ="Dialogue/Dialogue Line")]
public class DialogueLine : ScriptableObject
{
    public string speakerName;
    [TextArea(2, 5)]
    public string dialogueText;
    public Sprite speakerIcon;
}
