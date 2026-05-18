using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DialogueHandler : MonoBehaviour
{
    private DialogueSO dialogueSO;
    [SerializeField] GameObject dialoguePanel;
    [SerializeField] TextMeshProUGUI dialogueText;
    [SerializeField] TextMeshProUGUI speakerName;
    [SerializeField] Image potrait;
    int currentLineIndex = 0;

    public bool isDialogueActive;

    public void StartDialogue(DialogueSO NPCdialogue)
    {
        // THIS METHOD GRABS THE SO FROM THE NPC and begins the dialogue
        isDialogueActive = true;
        dialogueSO = NPCdialogue;
        currentLineIndex = 0;
        dialoguePanel.SetActive(true);
        DisplayLine();
    }

    public void DisplayLine()
    {
        if (currentLineIndex >= dialogueSO.lines.Length)
        {
            EndDialogue();
            return;
        }

        dialogueText.text = dialogueSO.lines[currentLineIndex].text;
        speakerName.text = dialogueSO.lines[currentLineIndex].speakerName;
        potrait.sprite = dialogueSO.lines[currentLineIndex].potrait;
    }

    public void AdvanceDialogue()
    {
        currentLineIndex++;
        DisplayLine();
    }

    public void EndDialogue()
    {
        isDialogueActive = false;
        dialoguePanel.SetActive(false);
    }
}
